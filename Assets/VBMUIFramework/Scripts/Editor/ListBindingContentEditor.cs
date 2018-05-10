using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomEditor(typeof(ListBindingContent), true)]
    public class ListBindingContentEditor : Editor {
        protected ReorderableList elementsList;

        protected virtual void OnEnable() {
            SerializedProperty elementsProperty = serializedObject.FindProperty("componentElements");
            elementsList = new ReorderableList(serializedObject, elementsProperty, true, true, true, true);
            elementsList.drawHeaderCallback += DrawMemberHeader;
            elementsList.drawElementCallback += DrawMemberDataListElement;
        }

        void DrawMemberHeader(Rect rect) {
            EditorGUI.LabelField(rect, "ViewModelBinding Elements");
        }

        void DrawMemberDataListElement(Rect rect, int index, bool isActive, bool isFocused) {
            SerializedProperty child = elementsList.serializedProperty.GetArrayElementAtIndex(index);
            EditorGUI.PropertyField(rect, child);
        }

        public override void OnInspectorGUI() {
            serializedObject.Update();

            EditorGUI.BeginChangeCheck();
            elementsList.DoLayoutList();
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }
    }
}