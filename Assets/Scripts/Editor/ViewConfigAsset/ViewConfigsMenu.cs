using System.IO;
using UnityEditor;
using UnityEngine;
using VBM;

namespace VBMEditor {
    public static class ViewConfigsMenu {
        public const string filename = "ViewConfigAsset.asset";

        [MenuItem("Assets/Create/View Config Asset")]
        static void CreateViewConfigs() {
            if (Selection.activeObject == null) {
                EditorUtility.DisplayDialog("提示", "请选择要创建的路径", "确定");
                return;
            }

            string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
            FileInfo fileInfo = new FileInfo(assetPath);
            if ((fileInfo.Attributes & FileAttributes.Directory) == 0) {
                assetPath = Path.GetDirectoryName(assetPath);
            }
            assetPath = AssetDatabase.GenerateUniqueAssetPath(Path.Combine(assetPath, filename));
            CreateScriptableObjectAsset<ViewConfigAsset>(assetPath);
        }

        public static T CreateScriptableObjectAsset<T>(string assetPath) where T : ScriptableObject {
            T asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return asset;
        }
    }
}