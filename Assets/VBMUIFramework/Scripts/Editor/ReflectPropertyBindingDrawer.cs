using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(ReflectPropertyBinding), true)]
    public class ReflectPropertyBindingDrawer : PropertyBindingDrawer {
        private Component component;
        private string componentPath;

        protected override bool DrawProperty(Rect rect, SerializedProperty childProperty) {
            if (childProperty.propertyPath.EndsWith("component")) {
                if (childProperty.objectReferenceValue != null) {
                    component = childProperty.objectReferenceValue as Component;
                    componentPath = childProperty.propertyPath;
                }
            } else if (childProperty.propertyPath.EndsWith("memberName")) {
                EditorGUI.BeginDisabledGroup(component == null);
                rect = EditorGUI.PrefixLabel(rect, new GUIContent(childProperty.displayName));
                string propertyPath = childProperty.propertyPath;
                SerializedObject serializedObject = childProperty.serializedObject;
                string displayName = string.IsNullOrEmpty(childProperty.stringValue) ? string.Empty : component.GetType().Name + "." + childProperty.stringValue;
                if (GUI.Button(rect, displayName)) {
                    ShowAddMemberMenu(rect, component.gameObject, serializedObject, propertyPath);
                }
                EditorGUI.EndDisabledGroup();
                return true;
            } else if (childProperty.propertyPath.EndsWith("memberType")) {
                return false; // do nothing
            }
            return base.DrawProperty(rect, childProperty);
        }

        void ShowAddMemberMenu(Rect rect, GameObject gameObject, SerializedObject serializedObject, string propertyPath) {
            GenericMenu menu = new GenericMenu();
            foreach (Component component in gameObject.GetComponents(typeof(Component))) {
                List<MemberInfo> memberInfoList = ReflectionMemberUtility.GetMembers(component.GetType());
                foreach (MemberInfo memberInfo in memberInfoList) {
                    GUIContent content = new GUIContent(component.GetType().Name + "/" + ReflectionMemberUtility.FormatMemberName(memberInfo));
                    menu.AddItem(content, false, () => {
                        SerializedProperty property = serializedObject.FindProperty(propertyPath);
                        SerializedProperty componentProperty = serializedObject.FindProperty(componentPath);
                        property.stringValue = memberInfo.Name;
                        componentProperty.objectReferenceValue = component;
                        serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            menu.DropDown(rect);
        }
    }
}