using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
namespace Psd
{
    public class TextureImport : IImageImport
    {
        public void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null)
        {
            RawImage pic;
            if (ownObj != null)
                pic =PSDImportUtility.AddMissingComponent<RawImage>(ownObj);
            else
                pic = PSDImportUtility.LoadAndInstant<RawImage>(PSDImporterConst.ASSET_PATH_RAWIMAGE, image.name, parent);
            Texture2D texture =PSDImportUtility.LoadAssetAtPath<Texture2D>(image) as Texture2D;

            pic.texture = texture as Texture;
            RectTransform rectTransform = pic.GetComponent<RectTransform>();
            PSDImportUtility.SetAnchorMiddleCenter(rectTransform);
            rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
            rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
        }
    }
}
