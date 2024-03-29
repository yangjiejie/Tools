﻿using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
namespace Psd
{
    public class ButtonLayerImport : ILayerImport
    {
        PSDImportCtrl ctrl;
        public ButtonLayerImport(PSDImportCtrl ctrl)
        {
            this.ctrl = ctrl;
        }

        public void DrawLayer(Layer layer, GameObject parent)
        {
            Button button = PSDImportUtility.LoadAndInstant<Button>(PSDImporterConst.ASSET_PATH_BUTTON, layer.name, parent);
            if (layer.layers != null)
            {
                for (int imageIndex = 0; imageIndex < layer.layers.Length; imageIndex++)
                {
                    PSImage image = layer.layers[imageIndex].image;
                    if (image.imageType != ImageType.Label && image.imageType != ImageType.Texture)
                    {
                            var p = PSDImportCtrl.Instance.GetFilePath(image);
                            Sprite sprite = AssetDatabase.LoadAssetAtPath(p, typeof(Sprite)) as Sprite;
                            if (image.name.ToLower().Contains("normal"))
                            {
                                button.image.sprite = sprite;                                
                                RectTransform rectTransform = button.GetComponent<RectTransform>();
                                rectTransform.sizeDelta = new Vector2(image.size.width, image.size.height);
                                rectTransform.anchoredPosition = new Vector2(image.position.x, image.position.y);
                                adjustButtonBG(image.imageType,button);
                            }
                            else if (image.name.ToLower().Contains("pressed"))
                            {
                                button.transition = Selectable.Transition.SpriteSwap;
                                SpriteState state = button.spriteState;
                                state.pressedSprite = sprite;
                                button.spriteState = state;
                            }
                            else if (image.name.ToLower().Contains("disabled"))
                            {
                                button.transition =Selectable.Transition.SpriteSwap;
                                SpriteState state = button.spriteState;
                                state.disabledSprite = sprite;
                                button.spriteState = state;
                            }
                            else if (image.name.ToLower().Contains("highlighted"))
                            {
                                button.transition = Selectable.Transition.SpriteSwap;
                                SpriteState state = button.spriteState;
                                state.highlightedSprite = sprite;
                                button.spriteState = state;
                            }
                        
                    }
                    else
                    {
                        ctrl.DrawLayer(layer.layers[imageIndex], button.gameObject);
                    }
                }
            }

        }

        //调整按钮背景
        private void adjustButtonBG(ImageType imageType,UnityEngine.UI.Button button)
        {
            if (imageType == ImageType.SliceImage)
            {
                button.image.type = UnityEngine.UI.Image.Type.Sliced;
            }
            else if (imageType == ImageType.LeftHalfImage)
            {
                var mirror = button.gameObject.AddComponent<Mirror>();
                mirror.mirrorType = Mirror.MirrorType.Horizontal;
                mirror.SetNativeSize();
            }
            else if (imageType == ImageType.BottomHalfImage)
            {
                var mirror = button.gameObject.AddComponent<Mirror>();
                mirror.mirrorType = Mirror.MirrorType.Vertical;
                mirror.SetNativeSize();
            }
            else if (imageType == ImageType.QuarterImage)
            {
                var mirror = button.gameObject.AddComponent<Mirror>();
                mirror.mirrorType = Mirror.MirrorType.Quarter;
                mirror.SetNativeSize();
            }
        }
    }


    [ExecuteInEditMode]
    public class PosLoader : MonoBehaviour
    {
        public Vector2 worldPos;
        void Start()
        {
            transform.position = worldPos;
            DestroyImmediate(this);
        }
    }
}