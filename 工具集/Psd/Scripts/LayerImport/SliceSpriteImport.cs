using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
namespace Psd
{
    public class SliceSpriteImport : IImageImport
    {
        public void DrawImage(PSImage image, GameObject parent, GameObject ownObj)
        {
            Image pic;
            if (ownObj != null)
                pic =PSDImportUtility.AddMissingComponent<Image>(ownObj);
            else
                pic = PSDImportUtility.LoadAndInstant<Image>(PSDImporterConst.ASSET_PATH_IMAGE, image.name, parent);

            RectTransform rectTransform = pic.GetComponent<RectTransform>();
            PSDImportUtility.SetAnchorMiddleCenter(rectTransform);
            //var p = PSDImportCtrl.Instance.GetFilePath(image);
            //var imp = AssetImporter.GetAtPath(p) as TextureImporter;
            //if (!PSDImportCtrl.Instance.IsSetBorder(imp))
            //{
            //    PSDImportCtrl.Instance.SetSpriteBorder(imp, image.arguments);
            //    AssetDatabase.WriteImportSettingsIfDirty(p);
            //    AssetDatabase.ImportAsset(p);
            //}
            //else
            //{
            //    Debug.Log("已设置九宫格:"+p);
            //}
            Sprite sprite =PSDImportUtility.LoadAssetAtPath<Sprite>(image) as Sprite;

            pic.sprite = sprite;
            pic.type = Image.Type.Sliced;

            //RectTransform rectTransform = pic.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
            rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
        }
    }
}
