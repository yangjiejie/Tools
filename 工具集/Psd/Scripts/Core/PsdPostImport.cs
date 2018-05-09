using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Psd
{
    //导入后处理九宫格
    public class PsdPostImport : AssetPostprocessor
    {
        private static Dictionary<string, string[]> asset2Proccess = new Dictionary<string, string[]>();
        public static void OnPostprocessAllAssets(string[] importedAsset, string[] deletedAssets, string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            if (PSDImportCtrl.Instance == null || asset2Proccess.Count == 0)
                return;
            var tmp = new List<string>();
            foreach (var s in asset2Proccess)
            {
                for (var i = 0; i < importedAsset.Length; i++)
                {
                    if (s.Key == importedAsset[i])
                    {
                        var p = s.Key;
                        var imp = AssetImporter.GetAtPath(p) as TextureImporter;
                        if (!PSDImportCtrl.Instance.IsSetBorder(imp))
                        {
                            PSDImportCtrl.Instance.SetSpriteBorder(imp,s.Value);
                            AssetDatabase.WriteImportSettingsIfDirty(p);
                            AssetDatabase.ImportAsset(p);
                            Debug.Log("设置九宫格:" + p);
                        }
                        else
                        {
                            Debug.Log("已设置九宫格:" + p);
                        }
                        tmp.Add(s.Key);
                    }
                }
            }
            for (var i = 0; i < tmp.Count; i++)
            {
                asset2Proccess.Remove(tmp[i]);
            }
        }

        public static void Add(string path, string[] param)
        {
            if (!asset2Proccess.ContainsKey(path))
            {
                asset2Proccess.Add(path, param);
            }
        }
    }
}

