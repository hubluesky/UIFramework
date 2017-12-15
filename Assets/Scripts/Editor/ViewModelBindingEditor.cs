using UnityEditor;
using VBM;

namespace VBMEditor {
    [CustomEditor(typeof(ViewModelBinding), true), CanEditMultipleObjects]
    public class ViewModelBindingEditor : Editor {
        public override void OnInspectorGUI() {
            ViewModelBinding behavior = target as ViewModelBinding;

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            UnityEditor.SerializedProperty property = serializedObject.FindProperty("modelUniqueId");
            EditorGUILayout.PropertyField(property);
            if (EditorGUI.EndChangeCheck()) {
                serializedObject.ApplyModifiedProperties();
            }

            base.OnInspectorGUI();
        }
    }
}