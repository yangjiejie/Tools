using UnityEditor;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Psd
{
    public class UIFile
    {
        public string System; //系统路径相对路径Assets/
        //根据命名规则命名后的文件名
        public List<string> Atlas = new List<string>();
        public List<string> Lantex = new List<string>();
        public List<string> Texture = new List<string>();
        //原始文件名
        public List<string> RawAtlas = new List<string>();
        public List<string> RawLantex = new List<string>();
        public List<string> RawTexture = new List<string>();

        public UIFile(string system)
        {
            System = system.Replace("\\", "/");
            var atlas = AssetDatabase.FindAssets("t:Sprite", new[] { System + "/atlas" });
            var arr = System.Split('/');
            var sysName = arr[arr.Length - 1];
            if (atlas != null)
            {
                for (var i = 0; i < atlas.Length; i++)
                {
                    var p = AssetDatabase.GUIDToAssetPath(atlas[i]);
                    var fileName = Path.GetFileNameWithoutExtension(p);
                    Atlas.Add(fileName);
                    var ver = ImportPSDEditor.AssetVersion;
                    var prefix = string.Format("alt{0}_{1}_", ver, sysName.ToLower());
                    RawAtlas.Add(fileName.Replace(prefix, ""));
                }
            }
            var lantex = AssetDatabase.FindAssets("t:Texture2D", new[] { System + "/lantex" });
            if (lantex != null)
            {
                for (var i = 0; i < lantex.Length; i++)
                {
                    var p = AssetDatabase.GUIDToAssetPath(lantex[i]);
                    var fileName = Path.GetFileNameWithoutExtension(p);
                    Lantex.Add(fileName);
                    var ver = ImportPSDEditor.AssetVersion;
                    var prefix = string.Format("lan{0}_{1}_", ver, sysName.ToLower());
                    RawLantex.Add(fileName.Replace(prefix, ""));
                }
            }
            var texs = AssetDatabase.FindAssets("t:Texture2D", new[] { System + "/texture" });
            if (texs != null)
            {
                for (var i = 0; i < texs.Length; i++)
                {
                    var p = AssetDatabase.GUIDToAssetPath(texs[i]);
                    var fileName = Path.GetFileNameWithoutExtension(p);
                    Texture.Add(fileName);
                    var ver = ImportPSDEditor.AssetVersion;
                    var prefix = string.Format("tex{0}_{1}_", ver, sysName.ToLower());
                    RawTexture.Add(fileName.Replace(prefix, ""));
                }
            }
        }
    }

    public class ImportPSDEditor : EditorWindow
    {
        public static int AssetVersion = 1;
        public static ImportPSDEditor Instance = null;
        private string _scrDir = "C://test.xml", _systemName = "";
        private const string WorkPath = "Assets/UIRes/";
        private const string PrefabPath = "Assets/UIPrefabs/";
        private const string configFilePath = "Assets/Psd/version.txt";
        private bool _needSavePrefab = false;

        [MenuItem("PSD/Import", false, 0)]
        static void Open()
        {
            if (Instance == null)
            {
                Instance = (ImportPSDEditor)EditorWindow.GetWindow(typeof(ImportPSDEditor));
                Instance.titleContent = new GUIContent("PSD导入工具");
            }
            Instance.minSize = Instance.maxSize = new Vector2(400, 400);
            if (Instance.Ini())
            {
                Instance.Show();
            }
        }

        public bool Ini()
        {
            bool isFoundConfig = false;
            if (File.Exists(configFilePath))
            {
                var lines = File.ReadAllLines(configFilePath);
                if (lines.Length <= 0)
                {
                    Debug.LogError("版本信息不存在");
                }
                else
                {
                    for (var i = 0; i < lines.Length; i++)
                    {
                        if (lines[i].Contains("version"))
                        {
                            var str = lines[0];
                            var arr = str.Split('=');
                            Int32.TryParse(arr[1], out AssetVersion);
                            isFoundConfig = true;
                            break;
                        }
                    }
                   
                }
            }
            if (!isFoundConfig)
            {
                Debug.LogError("找不到版本文件或版本信息填写有误:" + configFilePath);
            }
            Debug.Log("当前资源版本:"+AssetVersion);
            return true;
        }

        void OnGUI()
        {
            if (_needSavePrefab)
            {
                _needSavePrefab = false;
                var sp = PrefabPath + _systemName + "/" + PSDImportUtility.canvas.name + ".prefab";
                UnityEngine.Object prefab = PrefabUtility.CreatePrefab(sp, PSDImportUtility.canvas.gameObject);
                PrefabUtility.ReplacePrefab(PSDImportUtility.canvas.gameObject, prefab, ReplacePrefabOptions.ConnectToPrefab);
            }
            GUILayout.BeginVertical();
            GUILayout.Space(10);
            GUILayout.Label("选择导入的xml文件:");
            GUILayout.BeginHorizontal();
            _scrDir = GUILayout.TextField(_scrDir);
            if (GUILayout.Button("浏览"))
            {
                _scrDir = EditorUtility.OpenFilePanel("浏览", "C:/Users/Administrator/Destop/", "xml");
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("导入的系统名:", GUILayout.Width(80));
            _systemName = GUILayout.TextField(_systemName, GUILayout.Width(150));
            GUILayout.EndHorizontal();
            GUILayout.Space(250);
            GUILayout.BeginHorizontal();
            GUILayout.Space(150);
            if (GUILayout.Button("导入", GUILayout.Width(100)))
            {
                if (string.IsNullOrEmpty(_scrDir) || !_scrDir.EndsWith(".xml"))
                {
                    EditorUtility.DisplayDialog("注意!", "需要选择一个有效的XML文件", "确定");
                    return;
                }
                
                if (string.IsNullOrEmpty(_systemName))
                {
                    EditorUtility.DisplayDialog("注意!", "请输入系统名", "确定");
                    return;
                }
                if (!Directory.Exists(WorkPath + _systemName))
                {
                    var sys = WorkPath + _systemName;
                    Directory.CreateDirectory(sys);
                    Directory.CreateDirectory(sys + "/atlas");
                    Directory.CreateDirectory(sys + "/lantex");
                    Directory.CreateDirectory(sys + "/texture");
                    AssetDatabase.Refresh();
                }
                if (!Directory.Exists(PrefabPath + _systemName))
                {
                    Directory.CreateDirectory(PrefabPath + _systemName);
                    AssetDatabase.Refresh();
                }
                //开始导入
                using (PSDImportCtrl import = new PSDImportCtrl(_scrDir,_systemName, AssetVersion))
                {
                    import.BeginDrawUILayers();
                    import.BeginSetUIParents();
                    _needSavePrefab = true;//在下一帧保存预制体，立刻保存会出问题。
                }
               
            }
          
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();

        }
    }
}
