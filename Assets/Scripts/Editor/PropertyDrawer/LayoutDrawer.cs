using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace GeneralEditor {
    public static class LayoutDrawer {
        public enum BooleanOption : byte {
            True,
            False,
        }

        public static readonly System.Type BoolType = typeof(bool);
        public static readonly System.Type IntType = typeof(int);
        public static readonly System.Type FloatType = typeof(float);
        public static readonly System.Type DoubleType = typeof(double);
        public static readonly System.Type StringType = typeof(string);
        public static readonly System.Type LayerMaskType = typeof(LayerMask);
        public static readonly System.Type EnumType = typeof(System.Enum);
        public static readonly System.Type Vector2Type = typeof(Vector2);
        public static readonly System.Type Vector3Type = typeof(Vector3);
        public static readonly System.Type Vector4Type = typeof(Vector4);
        public static readonly System.Type ColorType = typeof(Color);
        public static readonly System.Type RectType = typeof(Rect);
        public static readonly System.Type BoundsType = typeof(Bounds);
        public static readonly System.Type AnimationCurveType = typeof(AnimationCurve);
        public static readonly System.Type ObjectType = typeof(Object);

        private static List<string> layerNameList = new List<string>(32);

        public static string[] GetLayerNames() {
            layerNameList.Clear();
            for (int i = 0; i < 32; i++)
                layerNameList.Add(LayerMask.LayerToName(i));
            return layerNameList.ToArray();
        }

        public static void PropertyField(SerializedProperty value, GUIContent label, params GUILayoutOption[] options) {
            if (value.PropertyType == BoolType) {
                BooleanOption option = value.BoolValue ? BooleanOption.True : BooleanOption.False;
                value.BoolValue = (BooleanOption) EditorGUILayout.EnumPopup(label, option, EditorStyles.popup, options) == BooleanOption.True;
            } else if (value.PropertyType == IntType) {
                GUI.backgroundColor = new Color(0, 0.6f, 0, 1);
                value.IntValue = EditorGUILayout.IntField(label, value.IntValue, options);
                GUI.backgroundColor = GUI.color;
            } else if (value.PropertyType == FloatType) {
                GUI.backgroundColor = new Color(0, 0.7f, 0, 1);
                value.FloatValue = EditorGUILayout.FloatField(label, value.FloatValue, options);
                GUI.backgroundColor = GUI.color;
            } else if (value.PropertyType == DoubleType) {
                GUI.backgroundColor = new Color(0, 0.8f, 0, 1);
                value.DoubleValue = EditorGUILayout.DoubleField(label, value.DoubleValue, options);
                GUI.backgroundColor = GUI.color;
            } else if (value.PropertyType == StringType) {
                GUI.backgroundColor = Color.cyan;
                value.StringValue = EditorGUILayout.TextField(label, value.StringValue, options);
                GUI.backgroundColor = GUI.color;
            } else if (value.PropertyType == LayerMaskType)
                value.LayerMaskValue = EditorGUILayout.MaskField(label, value.LayerMaskValue, GetLayerNames(), options);
            else if (EnumType.IsAssignableFrom(value.PropertyType))
                value.EnumValue = EditorGUILayout.EnumPopup(label, value.EnumValue, options);
            else if (value.PropertyType == Vector2Type)
                value.Vector2Value = EditorGUILayout.Vector2Field(label, value.Vector2Value, options);
            else if (value.PropertyType == Vector3Type)
                value.Vector3Value = EditorGUILayout.Vector3Field(label, value.Vector3Value, options);
            else if (value.PropertyType == Vector4Type)
                value.Vector4Value = EditorGUILayout.Vector3Field(label, value.Vector4Value, options);
            else if (value.PropertyType == ColorType)
                value.ColorValue = EditorGUILayout.ColorField(label, value.ColorValue, options);
            else if (value.PropertyType == RectType)
                value.RectValue = EditorGUILayout.RectField(label, value.RectValue, options);
            else if (value.PropertyType == BoundsType)
                value.BoundsValue = EditorGUILayout.BoundsField(label, value.BoundsValue, options);
            else if (value.PropertyType == AnimationCurveType)
                value.AnimationCurveValue = EditorGUILayout.CurveField(label, value.AnimationCurveValue, options);
            else if (ObjectType.IsAssignableFrom(value.PropertyType))
                value.UnityObjectValue = EditorGUILayout.ObjectField(label, value.UnityObjectValue, value.PropertyType, false, options);
            else
                EditorGUILayout.TextField(label, "Unknown :" + value.PropertyType, options);
        }

        public static void PropertyField(SerializedProperty value, GUIContent label, GUIStyle style, params GUILayoutOption[] options) {
            if (value.PropertyType == BoolType)
                value.BoolValue = EditorGUILayout.Toggle(label, value.BoolValue, style, options);
            else if (value.PropertyType == IntType)
                value.IntValue = EditorGUILayout.IntField(label, value.IntValue, style, options);
            else if (value.PropertyType == FloatType)
                value.FloatValue = EditorGUILayout.FloatField(label, value.FloatValue, style, options);
            else if (value.PropertyType == DoubleType)
                value.DoubleValue = EditorGUILayout.DoubleField(label, value.DoubleValue, style, options);
            else if (value.PropertyType == StringType)
                value.StringValue = EditorGUILayout.TextField(label, value.StringValue, style, options);
            else if (value.PropertyType == LayerMaskType)
                value.LayerMaskValue = EditorGUILayout.MaskField(label, value.LayerMaskValue, GetLayerNames(), style, options);
            else if (value.PropertyType == EnumType)
                value.EnumValue = EditorGUILayout.EnumPopup(label, value.EnumValue, style, options);
            else if (value.PropertyType == Vector2Type)
                value.Vector2Value = EditorGUILayout.Vector2Field(label, value.Vector2Value, options);
            else if (value.PropertyType == Vector3Type)
                value.Vector3Value = EditorGUILayout.Vector3Field(label, value.Vector3Value, options);
            else if (value.PropertyType == Vector4Type)
                value.Vector4Value = EditorGUILayout.Vector4Field(label.text, value.Vector4Value, options);
            else if (value.PropertyType == ColorType)
                value.ColorValue = EditorGUILayout.ColorField(label, value.ColorValue, options);
            else if (value.PropertyType == RectType)
                value.RectValue = EditorGUILayout.RectField(label, value.RectValue, options);
            else if (value.PropertyType == BoundsType)
                value.BoundsValue = EditorGUILayout.BoundsField(label, value.BoundsValue, options);
            else if (value.PropertyType == AnimationCurveType)
                value.AnimationCurveValue = EditorGUILayout.CurveField(label, value.AnimationCurveValue, options);
            else if (value.PropertyType.IsSubclassOf(ObjectType))
                value.UnityObjectValue = EditorGUILayout.ObjectField(label, value.UnityObjectValue, value.PropertyType, false, options);
            else
                EditorGUILayout.TextField(label, "Unknown value type", options);
        }

        //public static void PropertyField(Rect rect, SerializedProperty value, GUIContent label) {
        //    if (value.PropertyType == BoolType)
        //        value.BoolValue = EditorGUI.Toggle(rect, label, value.BoolValue);
        //    else if (value.PropertyType == IntType)
        //        value.IntValue = EditorGUI.IntField(rect, label, value.IntValue);
        //    else if (value.PropertyType == FloatType)
        //        value.FloatValue = EditorGUI.FloatField(rect, label, value.FloatValue);
        //    else if (value.PropertyType == DoubleType)
        //        value.DoubleValue = EditorGUI.DoubleField(rect, label, value.DoubleValue);
        //    else if (value.PropertyType == StringType)
        //        value.StringValue = EditorGUI.TextField(rect, label, value.StringValue);
        //    else if (value.PropertyType == LayerMaskType)
        //        value.LayerMaskValue = EditorGUI.MaskField(rect, label, value.LayerMaskValue, GetLayerNames());
        //    else if (EnumType.IsAssignableFrom(value.PropertyType))
        //        value.EnumValue = EditorGUI.EnumPopup(rect, label, value.EnumValue);
        //    else if (value.PropertyType == Vector2Type)
        //        value.Vector2Value = EditorGUI.Vector2Field(rect, label, value.Vector2Value);
        //    else if (value.PropertyType == Vector3Type)
        //        value.Vector3Value = EditorGUI.Vector3Field(rect, label, value.Vector3Value);
        //    else if (value.PropertyType == Vector4Type)
        //        value.Vector4Value = EditorGUI.Vector3Field(rect, label, value.Vector4Value);
        //    else if (value.PropertyType == ColorType)
        //        value.ColorValue = EditorGUI.ColorField(rect, label, value.ColorValue);
        //    else if (value.PropertyType == RectType)
        //        value.RectValue = EditorGUI.RectField(rect, label, value.RectValue);
        //    else if (value.PropertyType == BoundsType)
        //        value.BoundsValue = EditorGUI.BoundsField(rect, label, value.BoundsValue);
        //    else if (value.PropertyType == AnimationCurveType)
        //        value.AnimationCurveValue.Curve = EditorGUI.CurveField(rect, label, value.AnimationCurveValue.Curve);
        //    else if (ObjectType.IsAssignableFrom(value.PropertyType))
        //        value.UnityObjectValue = EditorGUI.ObjectField(rect, label, value.UnityObjectValue, value.PropertyType, false);
        //    else
        //        EditorGUI.TextField(rect, label, "Unknown :" + value.PropertyType);
        //}

        //public static void PropertyField(Rect rect, SerializedProperty value, GUIContent label, GUIStyle style, params GUILayoutOption[] options) {
        //    if (value.PropertyType == BoolType)
        //        value.BoolValue = EditorGUI.Toggle(rect, label, value.BoolValue, style);
        //    else if (value.PropertyType == IntType)
        //        value.IntValue = EditorGUI.IntField(rect, label, value.IntValue, style);
        //    else if (value.PropertyType == FloatType)
        //        value.FloatValue = EditorGUI.FloatField(rect, label, value.FloatValue, style);
        //    else if (value.PropertyType == DoubleType)
        //        value.DoubleValue = EditorGUI.DoubleField(rect, label, value.DoubleValue, style);
        //    else if (value.PropertyType == StringType)
        //        value.StringValue = EditorGUI.TextField(rect, label, value.StringValue, style);
        //    else if (EnumType.IsAssignableFrom(value.PropertyType))
        //        value.EnumValue = EditorGUI.EnumPopup(rect, label, value.EnumValue, style);
        //    else if (value.PropertyType == Vector2Type)
        //        value.Vector2Value = EditorGUI.Vector2Field(rect, label, value.Vector2Value);
        //    else if (value.PropertyType == Vector3Type)
        //        value.Vector3Value = EditorGUI.Vector3Field(rect, label, value.Vector3Value);
        //    else if (value.PropertyType == Vector4Type)
        //        value.Vector4Value = EditorGUI.Vector3Field(rect, label, value.Vector4Value);
        //    else if (value.PropertyType == ColorType)
        //        value.ColorValue = EditorGUI.ColorField(rect, label, value.ColorValue);
        //    else if (value.PropertyType == RectType)
        //        value.RectValue = EditorGUI.RectField(rect, label, value.RectValue);
        //    else if (value.PropertyType == BoundsType)
        //        value.BoundsValue = EditorGUI.BoundsField(rect, label, value.BoundsValue);
        //    else if (value.PropertyType == AnimationCurveType)
        //        value.AnimationCurveValue.Curve = EditorGUI.CurveField(rect, label, value.AnimationCurveValue.Curve);
        //    else if (ObjectType.IsAssignableFrom(value.PropertyType))
        //        value.UnityObjectValue = EditorGUI.ObjectField(rect, label, value.UnityObjectValue, value.PropertyType, false);
        //    else
        //        EditorGUI.TextField(rect, label, "Unknown :" + value.PropertyType);
        //}
    }
}