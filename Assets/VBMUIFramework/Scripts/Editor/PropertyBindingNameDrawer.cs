using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using VBM;

namespace VBMEditor {
    [CustomPropertyDrawer(typeof(PropertyBindingNameAttribute), true)]
    public class PropertyBindingNameDrawer : PropertyDrawer {

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {
            DrawSwitchPropertyBindingName(rect, property, label, null);
        }

        public static void DrawSwitchPropertyBindingName(Rect rect, SerializedProperty property, GUIContent label, System.Type propertyType) {
            float switchButtonWidth = rect.height;
            Rect rectContent = new Rect(rect.x, rect.y, rect.width - switchButtonWidth, rect.height);
            Rect rectButton = new Rect(rect.x + rectContent.width, rect.y, switchButtonWidth, rect.height);
            if (property.isExpanded) {
                EditorGUI.PropertyField(rectContent, property);
            } else {
                DrawPropertyBindingName(rectContent, property, label, propertyType);
            }
            property.isExpanded = EditorGUI.Toggle(rectButton, property.isExpanded, EditorStyles.radioButton);
        }

        public static void DrawMethodParameterPassing(Rect rect, SerializedProperty property, GUIContent label, System.Type parameterType) {
            float switchButtonWidth = rect.height;
            Rect rectContent = new Rect(rect.x, rect.y, rect.width - switchButtonWidth, rect.height);
            Rect rectButton = new Rect(rect.x + rectContent.width, rect.y, switchButtonWidth, rect.height);
            SerializedProperty passingNameProperty = property.FindPropertyRelative("passingName");
            if (property.isExpanded) {
                passingNameProperty.stringValue = null;
                const float typeWidth = 60.0f;
                rectContent.width -= typeWidth;
                Rect rectType = new Rect(rectContent.x + rectContent.width - typeWidth * 0.5f, rectContent.y, typeWidth + typeWidth * 0.5f, rectContent.height);
                SerializedProperty parameterTypeProperty = property.FindPropertyRelative("parameterType");

                MethodParameterType parameterEnumType = (MethodParameterType) parameterTypeProperty.enumValueIndex;
                SerializedProperty parameterProperty = property.FindPropertyRelative("parameter");
                if (parameterEnumType == MethodParameterType.Bool && parameterType.IsAssignableFrom(MethodParameter.BoolType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("boolParam");
                    EditorGUI.PropertyField(rectContent, paramChildProperty, label);
                    } else if (parameterEnumType == MethodParameterType.Int && parameterType.IsAssignableFrom(MethodParameter.IntType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("intParam");
                    EditorGUI.PropertyField(rectContent, paramChildProperty, label);
                    } else if (parameterEnumType == MethodParameterType.Float && parameterType.IsAssignableFrom(MethodParameter.FloatType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("floatParam");
                    EditorGUI.PropertyField(rectContent, paramChildProperty, label);
                    } else if (parameterEnumType == MethodParameterType.String && parameterType.IsAssignableFrom(MethodParameter.StringType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("stringParam");
                    EditorGUI.PropertyField(rectContent, paramChildProperty, label);
                    } else if (parameterEnumType == MethodParameterType.Enum && parameterType.IsAssignableFrom(MethodParameter.EnumType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("enumParam");
                    EditorGUI.PropertyField(rectContent, paramChildProperty, label);
                    } else if (parameterEnumType == MethodParameterType.UnityObject && MethodParameter.UnityObjectType.IsAssignableFrom(parameterType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("objectParam");
                    EditorGUI.ObjectField(rectContent, paramChildProperty, parameterType, label);
                    } else if (parameterEnumType == MethodParameterType.Color && parameterType.IsAssignableFrom(MethodParameter.ColorType)) {
                    SerializedProperty paramChildProperty = parameterProperty.FindPropertyRelative("colorParam");
                    EditorGUI.PropertyField(rectContent, paramChildProperty, label);
                    } else {
                    rectContent = EditorGUI.PrefixLabel(rectContent, label);
                    EditorGUI.HelpBox(rectContent, "Unkonwn Parameter Type", MessageType.Warning);
                }

                EditorGUI.PropertyField(rectType, parameterTypeProperty, GUIContent.none);
            } else {
                DrawPropertyBindingName(rectContent, passingNameProperty, label, parameterType);
            }
            property.isExpanded = EditorGUI.Toggle(rectButton, property.isExpanded, EditorStyles.radioButton);
        }

        public static void DrawPropertyBindingName(Rect rect, SerializedProperty property, GUIContent label, System.Type propertyType) {
            SerializedProperty parentBinding = property.serializedObject.FindProperty("parentBinding");
            ViewModelBinding viewModelBindig = parentBinding.objectReferenceValue as ViewModelBinding;
            SerializedProperty modelUniqueId = property.serializedObject.FindProperty("modelUniqueId");

            string parentName = viewModelBindig != null ? viewModelBindig.modelId : null;
            bool checkedModel = ModelReflection.instance.CheckModelProperty(parentName, modelUniqueId.stringValue);
            if (!checkedModel) {
                Color oldColor = GUI.color;
                GUI.color = Color.red;
                EditorGUI.LabelField(rect, label, new GUIContent("Not Propertys"), EditorStyles.popup);
                GUI.color = oldColor;
            } else {
                rect = EditorGUI.PrefixLabel(rect, label);
                if (GUI.Button(rect, property.stringValue, EditorStyles.popup)) {
                    GenericMenu menu = new GenericMenu();
                    ModelReflection.instance.ForeachProperty(parentName, modelUniqueId.stringValue, (propertyName, type) => {
                        if (propertyType == null || propertyType.IsAssignableFrom(type)) {
                            menu.AddItem(new GUIContent(propertyName), propertyName == property.stringValue, () => {
                                property.stringValue = propertyName;
                                property.serializedObject.ApplyModifiedProperties();
                            });
                        } else {
                            menu.AddDisabledItem(new GUIContent(propertyName));
                        }
                    });
                    menu.DropDown(rect);
                }
            }
        }
    }
}