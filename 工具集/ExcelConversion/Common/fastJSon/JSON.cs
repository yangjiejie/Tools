﻿using System;
using System.Collections;
using System.Collections.Generic;
//#if !SILVERLIGHT
//using System.Data;
//#endif
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
//using System.Collections.Specialized;

namespace fastJSON
{
    public delegate string Serialize(object data);
    public delegate object Deserialize(string data);

    public sealed class JSONParameters
    {
        /// <summary>
        /// Use the optimized fast Dataset Schema format (default = True)
        /// </summary>
        public bool UseOptimizedDatasetSchema = true;
        /// <summary>
        /// Use the fast GUID format (default = True)
        /// </summary>
        public bool UseFastGuid = true;
        /// <summary>
        /// Serialize null values to the output (default = True)
        /// </summary>
        public bool SerializeNullValues = true;
        /// <summary>
        /// Use the UTC date format (default = True)
        /// </summary>
        public bool UseUTCDateTime = true;
        /// <summary>
        /// Show the readonly properties of types in the output (default = False)
        /// </summary>
        public bool ShowReadOnlyProperties = false;
        /// <summary>
        /// Use the $types extension to optimise the output json (default = True)
        /// </summary>
        public bool UsingGlobalTypes = true;
        /// <summary>
        /// Ignore case when processing json and deserializing 
        /// </summary>
        public bool IgnoreCaseOnDeserialize = false;
        /// <summary>
        /// Anonymous types have read only properties 
        /// </summary>
        public bool EnableAnonymousTypes = false;
        /// <summary>
        /// Enable fastJSON extensions $types, $type, $map (default = True)
        /// </summary>
        public bool UseExtensions = true;
        /// <summary>
        /// Use escaped unicode i.e. \uXXXX format for non ASCII characters (default = True)
        /// </summary>
        public bool UseEscapedUnicode = true;
        /// <summary>
        /// Output string key dictionaries as "k"/"v" format (default = False) 
        /// </summary>
        public bool KVStyleStringDictionary = false;
        /// <summary>
        /// Output Enum values instead of names (default = False)
        /// </summary>
        public bool UseValuesOfEnums = false;
        /// <summary>
        /// Ignore attributes to check for (default : XmlIgnoreAttribute)
        /// </summary>
        public List<Type> IgnoreAttributes = new List<Type> { typeof(System.Xml.Serialization.XmlIgnoreAttribute) };
        /// <summary>
        /// If you have parametric and no default constructor for you classes (default = False)
        /// 
        /// IMPORTANT NOTE : If True then all initial values within the class will be ignored and will be not set
        /// </summary>
        public bool ParametricConstructorOverride = false;
        /// <summary>
        /// Serialize DateTime milliseconds i.e. yyyy-MM-dd HH:mm:ss.nnn (default = false)
        /// </summary>
        public bool DateTimeMilliseconds = false;

        public void FixValues()
        {
            if (UseExtensions == false) // disable conflicting params
                UsingGlobalTypes = false;
            if (EnableAnonymousTypes)
                ShowReadOnlyProperties = true;
        }
    }

    public static class JSON
    {
        /// <summary>
        /// Globally set-able parameters for controlling the serializer
        /// </summary>
        public static JSONParameters Parameters = new JSONParameters();
        /// <summary>
        /// Create a formatted json string (beautified) from an object
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToNiceJSON(object obj, JSONParameters param)
        {
            string s = ToJSON(obj, param);

            return Beautify(s);
        }
        /// <summary>
        /// Create a json representation for an object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJSON(object obj)
        {
            return ToJSON(obj, JSON.Parameters);
        }
        /// <summary>
        /// Create a json representation for an object with parameter override on this call
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static string ToJSON(object obj, JSONParameters param)
        {
            param.FixValues();
            Type t = null;

            if (obj == null)
                return "null";

            if (obj.GetType().IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(obj.GetType());
            if (t == typeof(Dictionary<,>) || t == typeof(List<>))
                param.UsingGlobalTypes = false;

            // FEATURE : enable extensions when you can deserialize anon types
            if (param.EnableAnonymousTypes) { param.UseExtensions = false; param.UsingGlobalTypes = false; }
            return new JSONSerializer(param).ConvertToJSON(obj);
        }
        /// <summary>
        /// Parse a json string and generate a Dictionary&lt;string,object&gt; or List&lt;object&gt; structure
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object Parse(string json)
        {
            return new JsonParser(json, JSON.Parameters.IgnoreCaseOnDeserialize).Decode();
        }
#if net4
        /// <summary>
        /// Create a .net4 dynamic object from the json string
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static dynamic ToDynamic(string json)
        {
            return new DynamicJson(json);
        }
#endif
        /// <summary>
        /// Create a typed generic object from the json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(string json)
        {
            return new deserializer(Parameters).ToObject<T>(json);
        }
        /// <summary>
        /// Create a typed generic object from the json with parameter override on this call
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T ToObject<T>(string json, JSONParameters param)
        {
            return new deserializer(param).ToObject<T>(json);
        }
        /// <summary>
        /// Create an object from the json
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static object ToObject(string json)
        {
            return new deserializer(Parameters).ToObject(json, null);
        }
        /// <summary>
        /// Create an object from the json with parameter override on this call
        /// </summary>
        /// <param name="json"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static object ToObject(string json, JSONParameters param)
        {
            return new deserializer(param).ToObject(json, null);
        }
        /// <summary>
        /// Create an object of type from the json
        /// </summary>
        /// <param name="json"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object ToObject(string json, Type type)
        {
            return new deserializer(Parameters).ToObject(json, type);
        }
        /// <summary>
        /// Fill a given object with the json represenation
        /// </summary>
        /// <param name="input"></param>
        /// <param name="json"></param>
        /// <returns></returns>
        //public static object FillObject(object input, string json)
        //{
        //    Dictionary<string, object> ht = new JsonParser(json, Parameters.IgnoreCaseOnDeserialize).Decode() as Dictionary<string, object>;
        //    if (ht == null) return null;
        //    return new deserializer(Parameters).ParseDictionary(ht, null, input.GetType(), input);
        //}
        /// <summary>
        /// Deep copy an object i.e. clone to a new object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static object DeepCopy(object obj)
        {
            return new deserializer(Parameters).ToObject(ToJSON(obj));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            return new deserializer(Parameters).ToObject<T>(ToJSON(obj));
        }

        /// <summary>
        /// Create a human readable string from the json 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Beautify(string input)
        {
            return Formatter.PrettyPrint(input);
        }
        /// <summary>
        /// Register custom type handlers for your own types not natively handled by fastJSON
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <param name="deserializer"></param>
        public static void RegisterCustomType(Type type, Serialize serializer, Deserialize deserializer)
        {
            Reflection.Instance.RegisterCustomType(type, serializer, deserializer);
        }
        /// <summary>
        /// Clear the internal reflection cache so you can start from new (you will loose performance)
        /// </summary>
        public static void ClearReflectionCache()
        {
            Reflection.Instance.ClearReflectionCache();
        }

        internal static long CreateLong(out long num, string s, int index, int count)
        {
            num = 0;
            bool neg = false;
            for (int x = 0; x < count; x++, index++)
            {
                char cc = s[index];

                if (cc == '-')
                    neg = true;
                else if (cc == '+')
                    neg = false;
                else
                {
                    num *= 10;
                    num += (int)(cc - '0');
                }
            }
            if (neg) num = -num;

            return num;
        }
    }

    internal class deserializer
    {
        public deserializer(JSONParameters param)
        {
            _params = param;
        }

        private JSONParameters _params;
        private bool _usingglobals = false;
        private Dictionary<object, int> _circobj = new Dictionary<object, int>();
        private Dictionary<int, object> _cirrev = new Dictionary<int, object>();
        //private bool _circular = true;

        public T ToObject<T>(string json)
        {
            Type t = typeof(T);
            var o = ToObject(json, t);

            if (t.IsArray)
            {
                if ((o as ICollection).Count == 0) // edge case for "[]" -> T[]
                {
                    Type tt = t.GetElementType();
                    object oo = Array.CreateInstance(tt, 0);
                    return (T)oo;
                }
                else
                    return (T)o;
            }
            else
                return (T)o;
        }

        public object ToObject(string json)
        {
            return ToObject(json, null);
        }


        object ChangeType1(object o,Type type)
        {
            if (type.IsPrimitive || type == typeof(string) || type.IsEnum)  //基本类型
            {
                if (o==null || o.GetType() == type)
                    return o;

                object v = System.ComponentModel.TypeDescriptor.GetConverter(type).ConvertFrom(o);
                return v;
            }
            else if (type.IsArray && o is List<object>) //数组
            {
                List<object> oo = (List<object>)o;
                Array v = Array.CreateInstance(type.GetElementType(), oo.Count);
                for (int i = 0; i < v.Length; i++)
                {
                    v.SetValue(ChangeType1(oo[i], type.GetElementType()), i);
                }
                return v;
            }
            else if ((type.IsValueType || type.IsClass) && o is Dictionary<string, object>)   //结构体
            {
                object v = Activator.CreateInstance(type);
                Dictionary<string, object> oo = (Dictionary<string, object>)o;
                foreach (var e in oo)
                {
                    FieldInfo fi = type.GetField(e.Key);
                    if (fi != null)
                    {
                        fi.SetValue(v, ChangeType1(e.Value, fi.FieldType));
                    }
                    else
                    {
                        PropertyInfo pi = type.GetProperty(e.Key, BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetField);
                        if(pi!=null)
                        {
                            pi.SetValue(v, ChangeType1(e.Value, pi.PropertyType),null);
                        }
                    }
                }
                return v;
            }
            else if((type.IsValueType || type.IsClass) && o == null)
            {
                return null;
            }
            else
                throw new Exception("UnExpected Type " + type.Name);
            return null;
        }

        public object ToObject(string json, Type type)
        {
            //_params = Parameters;
            _params.FixValues();
            Type t = null;
            if (type != null && type.IsGenericType)
                t = Reflection.Instance.GetGenericTypeDefinition(type);
            if (t == typeof(Dictionary<,>) || t == typeof(List<>))
                _params.UsingGlobalTypes = false;
            _usingglobals = _params.UsingGlobalTypes;

            object o = new JsonParser(json, _params.IgnoreCaseOnDeserialize).Decode();
            if (o == null)
                return null;

            return ChangeType1(o, type);


////#if !SILVERLIGHT
////            if (type != null && type == typeof(DataSet))
////                return CreateDataset(o as Dictionary<string, object>, null);

////            if (type != null && type == typeof(DataTable))
////                return CreateDataTable(o as Dictionary<string, object>, null);
////#endif
//            if (o is IDictionary)
//            {
//                if (type != null && t == typeof(Dictionary<,>)) // deserialize a dictionary
//                    return RootDictionary(o, type);
//                else // deserialize an object
//                    return ParseDictionary(o as Dictionary<string, object>, null, type, null);
//            }

//            if (o is List<object>)
//            {
//                if (type != null && t == typeof(Dictionary<,>)) // kv format
//                    return RootDictionary(o, type);

//                if (type != null && t == typeof(List<>)) // deserialize to generic list
//                    return RootList(o, type);

//                if (type == typeof(Hashtable))
//                    return RootHashTable((List<object>)o);
//                else if(type.IsArray)
//                {
//                    return CreateArray(o as List<object>, type, type.GetElementType(), null);
//                }
//                else
//                    return (o as List<object>).ToArray();
//            }

//            if (type != null && o.GetType() != type)
//                return ChangeType(o, type);

//            return o;
        }

        //#region [   p r i v a t e   m e t h o d s   ]
        //private object RootHashTable(List<object> o)
        //{
        //    Hashtable h = new Hashtable();

        //    foreach (Dictionary<string, object> values in o)
        //    {
        //        object key = values["k"];
        //        object val = values["v"];
        //        if (key is Dictionary<string, object>)
        //            key = ParseDictionary((Dictionary<string, object>)key, null, typeof(object), null);

        //        if (val is Dictionary<string, object>)
        //            val = ParseDictionary((Dictionary<string, object>)val, null, typeof(object), null);

        //        h.Add(key, val);
        //    }

        //    return h;
        //}

        //private object ChangeType(object value, Type conversionType)
        //{
        //    if (conversionType == typeof(int))
        //        return (int)((long)value);

        //    else if (conversionType == typeof(long))
        //        return (long)value;

        //    else if (conversionType == typeof(string))
        //        return (string)value;

        //    else if (conversionType == typeof(Guid))
        //        return CreateGuid((string)value);

        //    else if (conversionType.IsEnum)
        //        return CreateEnum(conversionType, value);

        //    else if (conversionType == typeof(DateTime))
        //        return CreateDateTime((string)value);

        //    else if (Reflection.Instance.IsTypeRegistered(conversionType))
        //        return Reflection.Instance.CreateCustom((string)value, conversionType);

        //    return Convert.ChangeType(value, conversionType, CultureInfo.InvariantCulture);
        //}

        //private object RootList(object parse, Type type)
        //{
        //    Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
        //    IList o = (IList)Reflection.Instance.FastCreateInstance(type);
        //    foreach (var k in (IList)parse)
        //    {
        //        _usingglobals = false;
        //        object v = k;
        //        if (k is Dictionary<string, object>)
        //            v = ParseDictionary(k as Dictionary<string, object>, null, gtypes[0], null);
        //        else
        //            v = ChangeType(k, gtypes[0]);

        //        o.Add(v);
        //    }
        //    return o;
        //}

        //private object RootDictionary(object parse, Type type)
        //{
        //    Type[] gtypes = Reflection.Instance.GetGenericArguments(type);
        //    Type t1 = null;
        //    Type t2 = null;
        //    if (gtypes != null)
        //    {
        //        t1 = gtypes[0];
        //        t2 = gtypes[1];
        //    }
        //    if (parse is Dictionary<string, object>)
        //    {
        //        IDictionary o = (IDictionary)Reflection.Instance.FastCreateInstance(type);

        //        foreach (var kv in (Dictionary<string, object>)parse)
        //        {
        //            object v;
        //            object k = ChangeType(kv.Key, t1);

        //            if (kv.Value is Dictionary<string, object>)
        //                v = ParseDictionary(kv.Value as Dictionary<string, object>, null, t2, null);

        //            else if (t2.IsArray)
        //                v = CreateArray((List<object>)kv.Value, t2, t2.GetElementType(), null);

        //            else if (kv.Value is IList)
        //                v = CreateGenericList((List<object>)kv.Value, t2, t1, null);

        //            else
        //                v = ChangeType(kv.Value, t2);

        //            o.Add(k, v);
        //        }

        //        return o;
        //    }
        //    if (parse is List<object>)
        //        return CreateDictionary(parse as List<object>, type, gtypes, null);

        //    return null;
        //}

//        internal object ParseDictionary(Dictionary<string, object> d, Dictionary<string, object> globaltypes, Type type, object input)
//        {
//            object tn = "";
//            //if (type == typeof(NameValueCollection))
//            //    return CreateNV(d);
//            //if (type == typeof(StringDictionary))
//            //    return CreateSD(d);
//            //if (_circular == false)
//            //    _circular = d.TryGetValue("$circular", out tn);

//            if (d.TryGetValue("$i", out tn))
//            {
//                object v = null;
//                _cirrev.TryGetValue((int)(long)tn, out v);
//                return v;
//            }

//            if (d.TryGetValue("$types", out tn))
//            {
//                _usingglobals = true;
//                globaltypes = new Dictionary<string, object>();
//                foreach (var kv in (Dictionary<string, object>)tn)
//                {
//                    globaltypes.Add((string)kv.Value, kv.Key);
//                }
//            }

//            bool found = d.TryGetValue("$type", out tn);
////#if !SILVERLIGHT
////            if (found == false && type == typeof(System.Object))
////            {
////                return d;   // CreateDataset(d, globaltypes);
////            }
////#endif
//            if (found)
//            {
//                if (_usingglobals)
//                {
//                    object tname = "";
//                    if (globaltypes != null && globaltypes.TryGetValue((string)tn, out tname))
//                        tn = tname;
//                }
//                type = Reflection.Instance.GetTypeFromCache((string)tn);
//            }

//            if (type == null)
//                throw new Exception("Cannot determine type");

//            string typename = type.FullName;
//            object o = input;
//            if (o == null)
//            {
//                if (_params.ParametricConstructorOverride)
//                    o = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
//                else
//                    o = Reflection.Instance.FastCreateInstance(type);
//            }
//            //if (_circular)
//            {
//                int i = 0;
//                if (_circobj.TryGetValue(o, out i) == false)
//                {
//                    i = _circobj.Count + 1;
//                    _circobj.Add(o, i);
//                    _cirrev.Add(i, o);
//                }
//            }

//            Dictionary<string, myPropInfo> props = Reflection.Instance.Getproperties(type, typename, _params.IgnoreCaseOnDeserialize, Reflection.Instance.IsTypeRegistered(type));
//            foreach (string n in d.Keys)
//            {
//                string name = n;
//                if (_params.IgnoreCaseOnDeserialize) name = name.ToLower();
//                if (name == "$map")
//                {
//                    ProcessMap(o, props, (Dictionary<string, object>)d[name]);
//                    continue;
//                }
//                myPropInfo pi;
//                if (props.TryGetValue(name, out pi) == false)
//                    continue;
//                if ((pi.Flags & (myPropInfoFlags.Filled | myPropInfoFlags.CanWrite)) != 0)
//                {
//                    object v = d[name];

//                    if (v != null)
//                    {
//                        object oset = null;

//                        switch (pi.Type)
//                        {
//                            case myPropInfoType.Int: oset = (int)((long)v); break;
//                            case myPropInfoType.Long: oset = (long)v; break;
//                            case myPropInfoType.String: oset = (string)v; break;
//                            case myPropInfoType.Bool: oset = (bool)v; break;
//                            case myPropInfoType.DateTime: oset = CreateDateTime((string)v); break;
//                            case myPropInfoType.Enum: oset = CreateEnum(pi.pt, v); break;
//                            case myPropInfoType.Guid: oset = CreateGuid((string)v); break;

//                            case myPropInfoType.Array:
//                                if (!pi.IsValueType)
//                                    oset = CreateArray((List<object>)v, pi.pt, pi.bt, globaltypes);
//                                // what about 'else'?
//                                break;
//                            case myPropInfoType.ByteArray: oset = Convert.FromBase64String((string)v); break;
////#if !SILVERLIGHT
////                            case myPropInfoType.DataSet: oset = CreateDataset((Dictionary<string, object>)v, globaltypes); break;
////                            case myPropInfoType.DataTable: oset = CreateDataTable((Dictionary<string, object>)v, globaltypes); break;
////                            case myPropInfoType.Hashtable: // same case as Dictionary
////#endif
//                            case myPropInfoType.Dictionary: oset = CreateDictionary((List<object>)v, pi.pt, pi.GenericTypes, globaltypes); break;
//                            case myPropInfoType.StringKeyDictionary: oset = CreateStringKeyDictionary((Dictionary<string, object>)v, pi.pt, pi.GenericTypes, globaltypes); break;
//                            //case myPropInfoType.NameValue: oset = CreateNV((Dictionary<string, object>)v); break;
//                            //case myPropInfoType.StringDictionary: oset = CreateSD((Dictionary<string, object>)v); break;
//                            case myPropInfoType.Custom: oset = Reflection.Instance.CreateCustom((string)v, pi.pt); break;
//                            default:
//                                {
//                                    if (pi.IsGenericType && pi.IsValueType == false && v is List<object>)
//                                        oset = CreateGenericList((List<object>)v, pi.pt, pi.bt, globaltypes);

//                                    else if (pi.IsClass && v is Dictionary<string, object>)
//                                        oset = ParseDictionary((Dictionary<string, object>)v, globaltypes, pi.pt, pi.getter(o));

//                                    else if (v is List<object>)
//                                        oset = CreateArray((List<object>)v, pi.pt, typeof(object), globaltypes);

//                                    else if (pi.IsValueType)
//                                        oset = ChangeType(v, pi.changeType);

//                                    else
//                                        oset = v;
//                                }
//                                break;
//                        }

//                        //o = pi.setter(o, oset);
//                        pi.setter(o, oset);
//                    }
//                }
//            }
//            return o;
//        }

//        //private StringDictionary CreateSD(Dictionary<string, object> d)
//        //{
//        //    StringDictionary nv = new StringDictionary();

//        //    foreach (var o in d)
//        //        nv.Add(o.Key, (string)o.Value);

//        //    return nv;
//        //}

//        //private NameValueCollection CreateNV(Dictionary<string, object> d)
//        //{
//        //    NameValueCollection nv = new NameValueCollection();

//        //    foreach (var o in d)
//        //        nv.Add(o.Key, (string)o.Value);

//        //    return nv;
//        //}

//        private void ProcessMap(object obj, Dictionary<string, myPropInfo> props, Dictionary<string, object> dic)
//        {
//            foreach (KeyValuePair<string, object> kv in dic)
//            {
//                myPropInfo p = props[kv.Key];
//                object o = p.getter(obj);
//                Type t = Type.GetType((string)kv.Value);
//                if (t == typeof(Guid))
//                    p.setter(obj, CreateGuid((string)o));
//            }
//        }

//        private int CreateInteger(out int num, string s, int index, int count)
//        {
//            num = 0;
//            bool neg = false;
//            for (int x = 0; x < count; x++, index++)
//            {
//                char cc = s[index];

//                if (cc == '-')
//                    neg = true;
//                else if (cc == '+')
//                    neg = false;
//                else
//                {
//                    num *= 10;
//                    num += (int)(cc - '0');
//                }
//            }
//            if (neg) num = -num;

//            return num;
//        }

//        private object CreateEnum(Type pt, object v)
//        {
//            // TODO : optimize create enum
////#if !SILVERLIGHT
////            return Enum.Parse(pt, v.ToString());
////#else
//            return Enum.Parse(pt, v.ToString(), true);
////#endif
//        }

//        private Guid CreateGuid(string s)
//        {
//            if (s.Length > 30)
//                return new Guid(s);
//            else
//                return new Guid(Convert.FromBase64String(s));
//        }

//        private DateTime CreateDateTime(string value)
//        {
//            bool utc = false;
//            //                   0123456789012345678 9012 9/3
//            // datetime format = yyyy-MM-dd HH:mm:ss .nnn  Z
//            int year;
//            int month;
//            int day;
//            int hour;
//            int min;
//            int sec;
//            int ms=0;
            
//            CreateInteger(out year, value, 0, 4);
//            CreateInteger(out month, value, 5, 2);
//            CreateInteger(out day, value, 8, 2);
//            CreateInteger(out hour, value, 11, 2);
//            CreateInteger(out min, value, 14, 2);
//            CreateInteger(out sec, value, 17, 2);
//            if (value.Length>21 &&  value[19] == '.' )  
//                CreateInteger(out ms, value, 20, 3);

//            //if (value.EndsWith("Z"))
//            if (value[value.Length - 1] == 'Z')
//                utc = true;

//            if (_params.UseUTCDateTime == false && utc == false)
//                return new DateTime(year, month, day, hour, min, sec, ms);
//            else
//                return new DateTime(year, month, day, hour, min, sec, ms, DateTimeKind.Utc).ToLocalTime();
//        }

//        private object CreateArray(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
//        {
//            Array col = Array.CreateInstance(bt, data.Count);
//            // create an array of objects
//            for (int i = 0; i < data.Count; i++)
//            {
//                object ob = data[i];
//                if (ob is IDictionary)
//                    col.SetValue(ParseDictionary((Dictionary<string, object>)ob, globalTypes, bt, null), i);
//                else
//                    col.SetValue(ChangeType(ob, bt), i);
//            }

//            return col;
//        }


//        private object CreateGenericList(List<object> data, Type pt, Type bt, Dictionary<string, object> globalTypes)
//        {
//            IList col = (IList)Reflection.Instance.FastCreateInstance(pt);
//            // create an array of objects
//            foreach (object ob in data)
//            {
//                if (ob is IDictionary)
//                    col.Add(ParseDictionary((Dictionary<string, object>)ob, globalTypes, bt, null));

//                else if (ob is List<object>)
//                {
//                    if (bt.IsGenericType)
//                        col.Add((List<object>)ob);//).ToArray());
//                    else
//                        col.Add(((List<object>)ob).ToArray());
//                }
//                else
//                    col.Add(ChangeType(ob, bt));
//            }
//            return col;
//        }

//        private object CreateStringKeyDictionary(Dictionary<string, object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
//        {
//            var col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
//            Type t1 = null;
//            Type t2 = null;
//            if (types != null)
//            {
//                t1 = types[0];
//                t2 = types[1];
//            }

//            foreach (KeyValuePair<string, object> values in reader)
//            {
//                var key = values.Key;
//                object val = null;

//                if (values.Value is Dictionary<string, object>)
//                    val = ParseDictionary((Dictionary<string, object>)values.Value, globalTypes, t2, null);

//                else if (types != null && t2.IsArray)
//                    val = CreateArray((List<object>)values.Value, t2, t2.GetElementType(), globalTypes);

//                else if (values.Value is IList)
//                    val = CreateGenericList((List<object>)values.Value, t2, t1, globalTypes);

//                else
//                    val = ChangeType(values.Value, t2);

//                col.Add(key, val);
//            }

//            return col;
//        }

//        private object CreateDictionary(List<object> reader, Type pt, Type[] types, Dictionary<string, object> globalTypes)
//        {
//            IDictionary col = (IDictionary)Reflection.Instance.FastCreateInstance(pt);
//            Type t1 = null;
//            Type t2 = null;
//            if (types != null)
//            {
//                t1 = types[0];
//                t2 = types[1];
//            }

//            foreach (Dictionary<string, object> values in reader)
//            {
//                object key = values["k"];
//                object val = values["v"];

//                if (key is Dictionary<string, object>)
//                    key = ParseDictionary((Dictionary<string, object>)key, globalTypes, t1, null);
//                else
//                    key = ChangeType(key, t1);

//                if (val is Dictionary<string, object>)
//                    val = ParseDictionary((Dictionary<string, object>)val, globalTypes, t2, null);
//                else
//                    val = ChangeType(val, t2);

//                col.Add(key, val);
//            }

//            return col;
//        }

//#if !SILVERLIGHT
//        private DataSet CreateDataset(Dictionary<string, object> reader, Dictionary<string, object> globalTypes)
//        {
//            DataSet ds = new DataSet();
//            ds.EnforceConstraints = false;
//            ds.BeginInit();

//            // read dataset schema here
//            var schema = reader["$schema"];

//            if (schema is string)
//            {
//                TextReader tr = new StringReader((string)schema);
//                ds.ReadXmlSchema(tr);
//            }
//            else
//            {
//                DatasetSchema ms = (DatasetSchema)ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
//                ds.DataSetName = ms.Name;
//                for (int i = 0; i < ms.Info.Count; i += 3)
//                {
//                    if (ds.Tables.Contains(ms.Info[i]) == false)
//                        ds.Tables.Add(ms.Info[i]);
//                    ds.Tables[ms.Info[i]].Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
//                }
//            }

//            foreach (KeyValuePair<string, object> pair in reader)
//            {
//                if (pair.Key == "$type" || pair.Key == "$schema") continue;

//                List<object> rows = (List<object>)pair.Value;
//                if (rows == null) continue;

//                DataTable dt = ds.Tables[pair.Key];
//                ReadDataTable(rows, dt);
//            }

//            ds.EndInit();

//            return ds;
//        }

//        private void ReadDataTable(List<object> rows, DataTable dt)
//        {
//            dt.BeginInit();
//            dt.BeginLoadData();
//            List<int> guidcols = new List<int>();
//            List<int> datecol = new List<int>();

//            foreach (DataColumn c in dt.Columns)
//            {
//                if (c.DataType == typeof(Guid) || c.DataType == typeof(Guid?))
//                    guidcols.Add(c.Ordinal);
//                if (_params.UseUTCDateTime && (c.DataType == typeof(DateTime) || c.DataType == typeof(DateTime?)))
//                    datecol.Add(c.Ordinal);
//            }

//            foreach (List<object> row in rows)
//            {
//                object[] v = new object[row.Count];
//                row.CopyTo(v, 0);
//                foreach (int i in guidcols)
//                {
//                    string s = (string)v[i];
//                    if (s != null && s.Length < 36)
//                        v[i] = new Guid(Convert.FromBase64String(s));
//                }
//                if (_params.UseUTCDateTime)
//                {
//                    foreach (int i in datecol)
//                    {
//                        string s = (string)v[i];
//                        if (s != null)
//                            v[i] = CreateDateTime(s);
//                    }
//                }
//                dt.Rows.Add(v);
//            }

//            dt.EndLoadData();
//            dt.EndInit();
//        }

//        DataTable CreateDataTable(Dictionary<string, object> reader, Dictionary<string, object> globalTypes)
//        {
//            var dt = new DataTable();

//            // read dataset schema here
//            var schema = reader["$schema"];

//            if (schema is string)
//            {
//                TextReader tr = new StringReader((string)schema);
//                dt.ReadXmlSchema(tr);
//            }
//            else
//            {
//                var ms = (DatasetSchema)this.ParseDictionary((Dictionary<string, object>)schema, globalTypes, typeof(DatasetSchema), null);
//                dt.TableName = ms.Info[0];
//                for (int i = 0; i < ms.Info.Count; i += 3)
//                {
//                    dt.Columns.Add(ms.Info[i + 1], Type.GetType(ms.Info[i + 2]));
//                }
//            }

//            foreach (var pair in reader)
//            {
//                if (pair.Key == "$type" || pair.Key == "$schema")
//                    continue;

//                var rows = (List<object>)pair.Value;
//                if (rows == null)
//                    continue;

//                if (!dt.TableName.Equals(pair.Key, StringComparison.InvariantCultureIgnoreCase))
//                    continue;

//                ReadDataTable(rows, dt);
//            }

//            return dt;
//        }
//#endif
       // #endregion
    }

}