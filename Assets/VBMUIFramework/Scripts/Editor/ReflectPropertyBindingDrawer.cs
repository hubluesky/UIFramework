using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(ReflectPropertyBinding.ReflectMember), true)]
    public class ReflectPropertyBindingDrawer : PropertyDrawer {

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 2;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            SerializedProperty componentProperty = property.FindPropertyRelative("component");
            SerializedProperty memberNameProperty = property.FindPropertyRelative("memberName");
            rect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(rect, componentProperty);
            rect = new Rect(rect.x, rect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing, rect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.BeginDisabledGroup(componentProperty.objectReferenceValue == null);
            rect = EditorGUI.PrefixLabel(rect, new GUIContent(memberNameProperty.displayName));
            string propertyPath = property.propertyPath;
            SerializedObject serializedObject = property.serializedObject;
            string displayName = componentProperty.objectReferenceValue == null ? string.Empty : componentProperty.objectReferenceValue.GetType().Name + "." + memberNameProperty.stringValue;
            if (GUI.Button(rect, displayName)) {
                ShowAddMemberMenu(rect, componentProperty, memberNameProperty);
            }
            EditorGUI.EndDisabledGroup();
        }

        void ShowAddMemberMenu(Rect rect, SerializedProperty componentProperty, SerializedProperty memberNameProperty) {
            GameObject gameObject = null;
            if (componentProperty.objectReferenceValue is GameObject)
                gameObject = componentProperty.objectReferenceValue as GameObject;
            else if (componentProperty.objectReferenceValue is Component)
                gameObject = (componentProperty.objectReferenceValue as Component).gameObject;
            GenericMenu menu = new GenericMenu();
            foreach (Component component in gameObject.GetComponents(typeof(Component))) {
                List<MemberInfo> memberInfoList = ReflectionMemberUtility.GetMembers(component.GetType());
                foreach (MemberInfo memberInfo in memberInfoList) {
                    GUIContent content = new GUIContent(component.GetType().Name + "/" + ReflectionMemberUtility.FormatMemberName(memberInfo));
                    menu.AddItem(content, false, () => {
                        memberNameProperty.stringValue = memberInfo.Name;
                        componentProperty.objectReferenceValue = component;
                        memberNameProperty.serializedObject.ApplyModifiedProperties();
                    });
                }
            }
            menu.DropDown(rect);
        }
    }
}