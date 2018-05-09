using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace 技能编辑器
{


    public class MyClassTypeConverter : System.ComponentModel.ExpandableObjectConverter
    {
        List<PropertyDescriptor> GetChildProperties1(object child, System.Attribute[] attributes)
        {
            if (child.GetType().IsArray)
            {
                List<PropertyDescriptor> pds = new List<PropertyDescriptor>();
                Array array = (Array)child;
                for (int i = 0; i < array.Length; i++)
                {
                    XArrayDescriptor xp = new XArrayDescriptor(array, i, child.GetType().GetElementType(), attributes);
                    pds.Add(xp);
                }

                return pds;
            }
            else
            {
                FieldInfo[] fi_list = child.GetType().GetFields();
                List<PropertyDescriptor> pds = new List<PropertyDescriptor>();
                for (int i = 0; i < fi_list.Length; i++)
                {
                    Type type = fi_list[i].FieldType;
                    object v = fi_list[i].GetValue(child);
                    if (type.IsPrimitive || type == typeof(string) || type.IsEnum)  //基本类型
                    {
                        XPropDescriptor xp = new XPropDescriptor(child, fi_list[i], attributes);
                        pds.Add(xp);
                    }
                    else if ((type.IsValueType || type.IsClass))   //结构体
                    {
                        //if (v == null)
                        //{
                        //    v = Activator.CreateInstance(type);
                        //    fi_list[i].SetValue(child, v);
                        //}
                        XPropDescriptor xp = new XPropDescriptor(child, fi_list[i], attributes);
                        pds.Add(xp);
                    }

                }
                return pds;
            }

        }

        public override PropertyDescriptorCollection GetProperties(ITypeDescriptorContext context, object value, Attribute[] attributes)
        {
            return new PropertyDescriptorCollection(GetChildProperties1(value, attributes).ToArray());
        }
    }

    class XArrayDescriptor : PropertyDescriptor
    {
        Array array;
        int index;
        Type ElementType;

        public XArrayDescriptor(Array ar, int idx, Type elType, Attribute[] atts)
            : base(elType.Name, atts)
        {
            array = ar;
            index = idx;
            ElementType = elType;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override System.Type ComponentType
        {
            get { return this.GetType(); }
        }

        public override object GetValue(object component)
        {
            return array.GetValue(index);
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override System.Type PropertyType
        {
            get { return ElementType; }
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            array.SetValue(value, index);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }

        public override TypeConverter Converter
        {
            get
            {
                Type type = ElementType;
                if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
                    return System.ComponentModel.TypeDescriptor.GetConverter(type);
                else
                    return new MyClassTypeConverter();
            }
        }

    }

    class XPropDescriptor : PropertyDescriptor
    {
        object obj;
        FieldInfo fi;

        public XPropDescriptor(object o, FieldInfo prop, Attribute[] attrs)
            : base(prop.Name, attrs)
        {
            obj = o;
            fi = prop;
        }

        public override bool CanResetValue(object component)
        {
            return false;
        }

        public override System.Type ComponentType
        {
            get { return this.GetType(); }
        }

        public override object GetValue(object component)
        {
            return fi.GetValue(obj);
        }

        public override bool IsReadOnly
        {
            get { return false; }
        }

        public override System.Type PropertyType
        {
            get { return fi.FieldType; }
        }

        public override void ResetValue(object component)
        {
        }

        public override void SetValue(object component, object value)
        {
            fi.SetValue(obj, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }


        public static T GetCustomAttribute<T>(FieldInfo fi) where T:class
        {
            object[] atts = fi.GetCustomAttributes(typeof(T),false);
            if (atts != null && atts.Length > 0)
                return (T)atts[0];
            return null;
        }

        public override string DisplayName
        {
            get
            {
                EditorNameAttribute da = GetCustomAttribute<EditorNameAttribute>(fi);
                if (da != null)
                    return da.EditorName;
                else
                    return base.DisplayName;
            }
        }
        public override string Description
        {
            get
            {
                EditorDescriptionAttribute da =GetCustomAttribute<EditorDescriptionAttribute>(fi);
                if (da != null)
                    return da.EditorDescription;
                else
                    return base.Description;
            }
        }

        public override TypeConverter Converter
        {
            get
            {
                Type type = fi.FieldType;
                if (type.IsPrimitive || type == typeof(string) || type.IsEnum)
                    return System.ComponentModel.TypeDescriptor.GetConverter(type);
                else
                    return new MyClassTypeConverter();
            }
        }


    }

    class XProps : ICustomTypeDescriptor
    {
        public object _obj;
        public XProps(object obj)
        {
            _obj = obj;
        }
        #region ICustomTypeDescriptor 成员

        public AttributeCollection GetAttributes()
        {
            return TypeDescriptor.GetAttributes(this, true);
        }

        public string GetClassName()
        {
            return TypeDescriptor.GetClassName(this, true);
        }

        public string GetComponentName()
        {
            return TypeDescriptor.GetComponentName(this, true);
        }

        public TypeConverter GetConverter()
        {
            return TypeDescriptor.GetConverter(this, true);
        }

        public EventDescriptor GetDefaultEvent()
        {
            return TypeDescriptor.GetDefaultEvent(this, true);
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return TypeDescriptor.GetDefaultProperty(this, true);
        }

        public object GetEditor(System.Type editorBaseType)
        {
            return TypeDescriptor.GetEditor(this, editorBaseType, true);
        }

        public EventDescriptorCollection GetEvents(System.Attribute[] attributes)
        {
            return TypeDescriptor.GetEvents(this, attributes, true);
        }

        public EventDescriptorCollection GetEvents()
        {
            return TypeDescriptor.GetEvents(this, true);
        }

        public PropertyDescriptorCollection GetProperties(System.Attribute[] attributes)
        {
            object child = _obj;
            FieldInfo[] fi_list = child.GetType().GetFields();
            List<PropertyDescriptor> pds = new List<PropertyDescriptor>();
            for (int i = 0; i < fi_list.Length; i++)
            {
                Type type = fi_list[i].FieldType;
                object v = fi_list[i].GetValue(child);
                XPropDescriptor xp = new XPropDescriptor(child, fi_list[i], attributes);
                pds.Add(xp);
            }
            return new PropertyDescriptorCollection(pds.ToArray());
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return TypeDescriptor.GetProperties(this, true);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }

        #endregion

        //public override string ToString()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    for (int i = 0; i < this.Count; i++)
        //    {
        //        sb.Append("[" + i + "] " + this[i].ToString() + System.Environment.NewLine);
        //    }
        //    return sb.ToString();
        //}
    }   

}
