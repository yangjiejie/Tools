using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using Psd;

#if UNITY_5_3
using UnityEditor.SceneManagement;
#endif

namespace Psd
{
    public class PSDImportCtrl: IDisposable
    {
        public static PSDImportCtrl Instance;
        private PSDUI psdUI;
        private string _systemName;//导入的系统名
        private int _version;//当前资源版本
        private readonly List<UIFile> _existFiles = new List<UIFile>(); //已存在的文件，已存在的文件不会重新导入
        private readonly Dictionary<string, string> ScrFiles = new Dictionary<string, string>(); //源文件
        private IImageImport spriteImport;
        private IImageImport textImport;
        private IImageImport textureImport;
        private IImageImport slicedSpriteImport;
        private IImageImport halfSpriteImport;

        private ILayerImport buttonImport;
        private ILayerImport toggleImport;
        private ILayerImport panelImport;
        private ILayerImport scrollViewImport;
        private ILayerImport scrollBarImport;
        private ILayerImport sliderImport;
        private ILayerImport gridImport;
        private ILayerImport emptyImport;
        private ILayerImport groupImport;
        private ILayerImport inputFiledImport;
        private ILayerImport layoutElemLayerImport;

        public PSDImportCtrl(string xmlFilePath,string sysName,int ver)
        {
            _systemName = sysName;
            _version = ver;
            Instance = this;
            this.RecordScrFiles(xmlFilePath);//记录导入文件名及路径
            InitDataAndPath(xmlFilePath);//序列化
            InitCanvas();
            //LoadLayers();
            MoveLayers();
            this.RecordSysFiles();
            InitDrawers();
            PSDImportUtility.ParentDic.Clear();
        }
        //记录源文件及路径
        private void RecordScrFiles(string _scrDir)
        {
            ScrFiles.Clear();
            var dn = Path.GetDirectoryName(_scrDir);
            var files = Directory.GetFiles(dn);
            for (var i = 0; i < files.Length; i++)
            {
                var fileName = Path.GetFileNameWithoutExtension(files[i]);
                if(string.IsNullOrEmpty(fileName))
                    continue;
                if (!ScrFiles.ContainsKey(fileName))
                {
                    var p = files[i];
                    p = p.Replace("\\", "/");
                    if (p.EndsWith(".xml")||p.EndsWith(".meta"))
                        continue;
                    ScrFiles.Add(fileName,p);
                }
            }
        }
        //加载已有系统，记录系统文件
        private void RecordSysFiles()
        {
            _existFiles.Clear();
            var sys = Directory.GetDirectories(PSDImporterConst.RESOURECEDIR);
            foreach (var t in sys)
            {
                var uf = new UIFile(t);
                _existFiles.Add(uf);
            }
        }
        //根据导出文件名，获取文件路径
        public string GetFilePath(PSImage image)
        {
            string fileName = image.name;
            for (var i = 0; i < _existFiles.Count; i++)
            {
                var uf = _existFiles[i];
                switch (image.imageType)
                {
                    case ImageType.Texture:
                        for (var j = 0; j < uf.Texture.Count; j++)
                        {
                            if (fileName == uf.RawTexture[j])
                                return uf.System + "/texture/" + uf.Texture[j] + PSDImporterConst.PNG_SUFFIX;
                        }
                        break;
                    case ImageType.Label:
                        
                        break;
                    case ImageType.Image:
                    case ImageType.SliceImage:
                    case ImageType.LeftHalfImage:
                    case ImageType.BottomHalfImage:
                    case ImageType.QuarterImage:
                        for (var j = 0; j < uf.Atlas.Count; j++)
                        {
                            if (fileName == uf.RawAtlas[j])
                                return uf.System + "/atlas/" + uf.Atlas[j] + PSDImporterConst.PNG_SUFFIX;
                        }
                        break;
                    default:
                        break;
                }
            }
            Debug.LogError("can not find asset:"+fileName);
            return string.Empty;
        }
        //设置九宫格 
        public bool IsSetBorder(TextureImporter ti)
        {
            var v = ti.spriteBorder;
            var b = (int)v.x == 0 && (int)v.y == 0 && (int)v.z == 0 && (int)v.w == 0;
            return !b;
        }
        public void SetSpriteBorder(TextureImporter textureImporter, string[] args)
        {
            textureImporter.spriteBorder = new Vector4(float.Parse(args[0]), float.Parse(args[1]), float.Parse(args[2]),
                float.Parse(args[3]));
        }
        public void DrawLayer(Layer layer, GameObject parent)
        {
            switch (layer.type)
            {
                case LayerType.Panel:
                    panelImport.DrawLayer(layer, parent);
                    break;
                case LayerType.Normal:
                    emptyImport.DrawLayer(layer, parent);
                    break;
                case LayerType.Button:
                    buttonImport.DrawLayer(layer, parent);
                    break;
                case LayerType.Toggle:
                    toggleImport.DrawLayer(layer, parent);
                    break;
                case LayerType.Grid:
                    gridImport.DrawLayer(layer, parent);
                    break;
                case LayerType.ScrollView:
                    scrollViewImport.DrawLayer(layer, parent);
                    break;
                case LayerType.Slider:
                    sliderImport.DrawLayer(layer, parent);
                    break;
                case LayerType.Group:
                    groupImport.DrawLayer(layer, parent);
                    break;
                case LayerType.InputField:
                    inputFiledImport.DrawLayer(layer, parent);
                    break;
                case LayerType.ScrollBar:
                    scrollBarImport.DrawLayer(layer, parent);
                    break;
                case LayerType.LayoutElement:
                    layoutElemLayerImport.DrawLayer(layer, parent);
                    break;
                case LayerType.TabGroup:
                    
                    break;
                default:
                    break;

            }
        }

        public void DrawLayers(Layer[] layers, GameObject parent)
        {
            if (layers != null)
            {
                for (int layerIndex = 0; layerIndex < layers.Length; layerIndex++)
                {
                    DrawLayer(layers[layerIndex], parent);
                }
            }
        }

        public void DrawImage(PSImage image, GameObject parent, GameObject ownObj = null)
        {
            switch (image.imageType)
            {
                case ImageType.Image:
                    spriteImport.DrawImage(image, parent, ownObj);
                    break;
                case ImageType.Texture:
                    textureImport.DrawImage(image, parent, ownObj);
                    break;
                case ImageType.Label:
                    textImport.DrawImage(image, parent, ownObj);
                    break;
                case ImageType.SliceImage:
                    slicedSpriteImport.DrawImage(image, parent, ownObj);
                    break;
                case ImageType.LeftHalfImage:
                    halfSpriteImport.DrawImage(image, parent, ownObj);
                    break;
                case ImageType.BottomHalfImage:
                    halfSpriteImport.DrawImage(image, parent, ownObj);
                    break;
                case ImageType.QuarterImage:
                    halfSpriteImport.DrawImage(image, parent, ownObj);
                    break;
                default:
                    break;
            }
        }

        private void InitDataAndPath(string xmlFilePath)
        {
            psdUI = (PSDUI)PSDImportUtility.DeserializeXml(xmlFilePath, typeof(PSDUI));
            Debug.Log(psdUI.psdSize.width + "=====psdSize======" + psdUI.psdSize.height);
            if (psdUI == null)
            {
                Debug.Log("The file " + xmlFilePath + " wasn't able to generate a PSDUI.");
                return;
            }
#if UNITY_5_2
            if (EditorApplication.SaveCurrentSceneIfUserWantsTo() == false) { return; }
#elif UNITY_5_3
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo() == false) { return; }
#endif
            PSDImportUtility.baseFilename = Path.GetFileNameWithoutExtension(xmlFilePath);
            PSDImportUtility.baseDirectory = "Assets/" + Path.GetDirectoryName(xmlFilePath.Remove(0, Application.dataPath.Length + 1)) + "/";
        }

        private void InitCanvas()
        {
#if UNITY_5_2
            EditorApplication.NewScene();
#elif UNITY_5_3
            EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects);
#endif
            var root = GameObject.Find("UIRoot");
            Canvas temp = AssetDatabase.LoadAssetAtPath(PSDImporterConst.ASSET_PATH_CANVAS, typeof(Canvas)) as Canvas;
            PSDImportUtility.canvas = GameObject.Instantiate(temp) as Canvas;
            PSDImportUtility.canvas.name = _systemName;
            var rt = PSDImportUtility.canvas.GetComponent<RectTransform>();
            rt.anchorMax = Vector2.one;
            rt.anchorMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.offsetMin = Vector2.zero;
            rt.pivot = new Vector2(0.5f,0.5f);
            if (root != null)
            {
                rt.SetParent(root.transform, false);
            }
            else
            {
                Debug.LogError("请在:Assets/Main场景中导入PSD");
            }
            PSDImportUtility.canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            //UnityEngine.UI.CanvasScaler scaler = PSDImportUtility.canvas.GetComponent<UnityEngine.UI.CanvasScaler>();
            //scaler.screenMatchMode = UnityEngine.UI.CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            //scaler.matchWidthOrHeight = 1f;
            //scaler.referenceResolution = new Vector2(psdUI.psdSize.width, psdUI.psdSize.height);

            //GameObject go = AssetDatabase.LoadAssetAtPath(PSDImporterConst.ASSET_PATH_EVENTSYSTEM, typeof(GameObject)) as GameObject;
            //PSDImportUtility.eventSys = GameObject.Instantiate(go) as GameObject;
        }

        private void InitDrawers()
        {
            spriteImport = new SpriteImport();
            textImport = new TextImport();
            textureImport = new TextureImport();
            slicedSpriteImport = new SliceSpriteImport();
            halfSpriteImport = new HalfSpriteImport();


            buttonImport = new ButtonLayerImport(this);
            toggleImport = new ToggleLayerImport(this);
            panelImport = new PanelLayerImport(this);
            scrollViewImport = new ScrollViewLayerImport(this);
            scrollBarImport = new ScrollBarLayerImport(this);
            sliderImport = new SliderLayerImport(this);
            gridImport = new GridLayerImport(this);
            emptyImport = new DefultLayerImport(this);
            groupImport = new GroupLayerImport(this);
            inputFiledImport = new InputFieldLayerImport(this);
            layoutElemLayerImport = new LayoutElementLayerImport(this);
            //tabGroupLayerImport = new TabGroupLayerImport(this);
        }

        public void BeginDrawUILayers()
        {
            RectTransform obj = PSDImportUtility.LoadAndInstant<RectTransform>(PSDImporterConst.ASSET_PATH_EMPTY, PSDImportUtility.baseFilename, PSDImportUtility.canvas.gameObject);
            obj.offsetMin = Vector2.zero;
            obj.offsetMax = Vector2.zero;
            obj.anchorMin = Vector2.zero;
            obj.anchorMax = Vector2.one;

            for (int layerIndex = 0; layerIndex < psdUI.layers.Length; layerIndex++)
            {
                DrawLayer(psdUI.layers[layerIndex], obj.gameObject);
            }
            AssetDatabase.Refresh();
        }

        public void BeginSetUIParents()
        {
            foreach (var item in PSDImportUtility.ParentDic)
            {
                item.Key.SetParent(item.Value);
            }
        }

        private void MoveLayers()
        {
            for (int layerIndex = 0; layerIndex < psdUI.layers.Length; layerIndex++)
            {
                MoveAsset(psdUI.layers[layerIndex]);
            }

            AssetDatabase.Refresh();
        }
        //------------------------------------------------------------------
        //when it's a common psd, then move the asset to special folder
        //------------------------------------------------------------------
        private void MoveAsset(Layer layer)
        {
            if (layer.image != null)
            {
                string scrFile = "";
                if (ScrFiles.TryGetValue(layer.image.name, out scrFile))
                {
                    var fn = Path.GetFileName(scrFile);
                    string rn = "";
                    var src = layer.image.imageSource;
                    //若来源是公共资源，则拷贝到公共系统目录下
                    switch (layer.image.imageType)
                    {
                        case ImageType.Texture:
                            {
                                if (src == ImageSource.Common)
                                {
                                    rn = string.Format("tex{0}_{1}_", _version, PSDImporterConst.CommonSys.ToLower()) + fn;
                                    var desDir = PSDImporterConst.RESOURECEDIR + PSDImporterConst.CommonSys + "/texture/" + rn;
                                    if (File.Exists(desDir))
                                    {
                                        Debug.Log("覆盖文件:" + desDir);
                                    }
                                    File.Copy(scrFile,desDir, true);
                                }
                                else
                                {
                                    rn = string.Format("tex{0}_{1}_", _version, _systemName.ToLower()) + fn;
                                    var desDir = PSDImporterConst.RESOURECEDIR + _systemName + "/texture/" + rn;
                                    if (File.Exists(desDir))
                                    {
                                        Debug.Log("覆盖文件:" + desDir);
                                    }
                                    File.Copy(scrFile,desDir, true);
                                }
                               
                            }
                            break;
                        case ImageType.Label:
                            {
                            }
                            break;
                        case ImageType.SliceImage:
                            {
                                if (src == ImageSource.Common)
                                {
                                    rn = string.Format("alt{0}_{1}_", _version, PSDImporterConst.CommonSys.ToLower()) + fn;
                                    var desDir = PSDImporterConst.RESOURECEDIR + PSDImporterConst.CommonSys + "/atlas/" + rn;
                                    if (File.Exists(desDir))
                                    {
                                        Debug.Log("覆盖文件:" + desDir);
                                    }
                                    PsdPostImport.Add(desDir, layer.image.arguments);
                                    File.Copy(scrFile, desDir, true);
                                   
                                }
                                else
                                {
                                    rn = string.Format("alt{0}_{1}_", _version, _systemName.ToLower()) + fn;
                                    var desDir = PSDImporterConst.RESOURECEDIR + _systemName + "/atlas/" + rn;
                                    if (File.Exists(desDir))
                                    {
                                        Debug.Log("覆盖文件:" + desDir);
                                    }
                                    PsdPostImport.Add(desDir, layer.image.arguments);
                                    File.Copy(scrFile, desDir, true);
                                }
                                
                            }
                            break;
                        case ImageType.LeftHalfImage:
                        case ImageType.BottomHalfImage:
                        case ImageType.QuarterImage:
                        case ImageType.Image:
                            {
                                if (src == ImageSource.Common)
                                {
                                    rn = string.Format("alt{0}_{1}_", _version, PSDImporterConst.CommonSys.ToLower()) + fn;
                                    var desDir = PSDImporterConst.RESOURECEDIR + PSDImporterConst.CommonSys + "/atlas/" +rn;
                                    if (File.Exists(desDir))
                                    {
                                        Debug.Log("覆盖文件:"+desDir);
                                    }
                                    File.Copy(scrFile,desDir , true);
                                }
                                else
                                {
                                    rn = string.Format("alt{0}_{1}_", _version, _systemName.ToLower()) + fn;
                                    var desDir = PSDImporterConst.RESOURECEDIR + _systemName + "/atlas/" + rn;
                                    if (File.Exists(desDir))
                                    {
                                        Debug.Log("覆盖文件:" + desDir);
                                    }
                                    File.Copy(scrFile,desDir, true);
                                }
                            }
                            break;
                        default:
                            Debug.LogError("文件不存在:");
                            break;
                    }
                }
            }
            if (layer.layers != null)
            {
                for (int layerIndex = 0; layerIndex < layer.layers.Length; layerIndex++)
                {
                    MoveAsset(layer.layers[layerIndex]);
                }
            }
        }

        public void Dispose()
        {
            Instance = null;
            GC.Collect();
        }
    }
}
