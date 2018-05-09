using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace Psd
{
    public class SpriteImport : IImageImport
    {
        public void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null)
        {
            if (image.imageSource == ImageSource.Common || image.imageSource == ImageSource.Private)
            {
                Image pic;
                if (ownObj != null)
                    pic = PSDImportUtility.AddMissingComponent<Image>(ownObj);
                else
                    pic = PSDImportUtility.LoadAndInstant<Image>(PSDImporterConst.ASSET_PATH_IMAGE, image.name, parent);

                RectTransform rectTransform = pic.GetComponent<RectTransform>();
                rectTransform.offsetMin = new Vector2(0.5f, 0.5f);
                rectTransform.offsetMax = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                var p = PSDImportCtrl.Instance.GetFilePath(image);
                Sprite sprite = AssetDatabase.LoadAssetAtPath(p, typeof(Sprite)) as Sprite;

                if (sprite == null)
                {
                    Debug.Log("loading asset at path: " + p);
                }

                pic.sprite = sprite;

                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
            }
        }
    }
}
