using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomEditor(typeof(ViewConfigAsset), true), CanEditMultipleObjects]
    public class ViewConfigAssetEditor : Editor {
        private ReorderableList list;

        void OnEnable() {
            SerializedProperty configs = serializedObject.FindProperty("viewConfigs");
            list = new ReorderableList(serializedObject, configs, true, true, true, true);

            list.drawElementCallback += DrawElement;
            list.elementHeightCallback += ElementHeight;
        }

        private float ElementHeight(int index) {
            SerializedProperty childProperty = list.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(childProperty, null);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused) {
            EditorGUI.indentLevel++;
            SerializedProperty childProperty = list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty prefabPropety = childProperty.FindPropertyRelative("prefab");
            SerializedProperty viewNamePropety = childProperty.FindPropertyRelative("viewName");
            bool isSetPrefab = prefabPropety.objectReferenceValue == null && string.IsNullOrEmpty(viewNamePropety.stringValue);
            EditorGUI.PropertyField(rect, childProperty, true);
            if (isSetPrefab && prefabPropety.objectReferenceValue != null && string.IsNullOrEmpty(viewNamePropety.stringValue)) {
                GameObject prefabObject = prefabPropety.objectReferenceValue as GameObject;
                ViewModelBinding viewModelBinding = prefabObject.GetComponent<ViewModelBinding>();
                if(viewModelBinding != null) {
                    viewNamePropety.stringValue = viewModelBinding.modelId;
                    viewNamePropety.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.indentLevel--;
        }

        public override void OnInspectorGUI() {
            EditorGUI.BeginChangeCheck();
            list.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}