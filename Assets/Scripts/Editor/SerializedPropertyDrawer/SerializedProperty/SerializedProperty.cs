using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace GeneralEditor {
    public class SerializedProperty : IEnumerable<SerializedProperty> {
        protected Dictionary<FieldInfo, SerializedProperty> childPropertys = new Dictionary<FieldInfo, SerializedProperty>();
        protected SerializedProperty parent;
        protected object property;

        public SerializedProperty(object property, string displayName, SerializedProperty parent) {
            this.parent = parent;
            this.property = property;
            DisplayName = displayName;
            PropertyType = property.GetType();
            IsExpanded = true;
        }

        public SerializedProperty(object property, string displayName, System.Type type, FieldInfo fieldInfo, SerializedProperty parent) {
            FieldInfo = fieldInfo;
            PropertyType = type;
            this.parent = parent;
            this.property = property;
            DisplayName = displayName;
            IsExpanded = true;
        }

        public bool BoolValue {
            get { return System.Convert.ToBoolean(PropertyValue); }
            set { PropertyValue = value; }
        }

        public int IntValue {
            get { return System.Convert.ToInt32(PropertyValue); }
            set { PropertyValue = value; }
        }

        public float FloatValue {
            get { return System.Convert.ToSingle(PropertyValue); }
            set { PropertyValue = value; }
        }

        public double DoubleValue {
            get { return System.Convert.ToDouble(PropertyValue); }
            set { PropertyValue = value; }
        }

        public string StringValue {
            get { return System.Convert.ToString(PropertyValue); }
            set { PropertyValue = value; }
        }

        public LayerMask LayerMaskValue {
            get { return (LayerMask) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public System.Enum EnumValue {
            get { return (System.Enum) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public Vector2 Vector2Value {
            get { return (Vector2) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public Vector3 Vector3Value {
            get { return (Vector3) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public Vector4 Vector4Value {
            get { return (Vector4) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public Color ColorValue {
            get { return (Color) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public Rect RectValue {
            get { return (Rect) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public Bounds BoundsValue {
            get { return (Bounds) (PropertyValue); }
            set { PropertyValue = value; }
        }

        public AnimationCurve AnimationCurveValue {
            get { return PropertyValue as AnimationCurve; }
            set { PropertyValue = value; }
        }

        public Object UnityObjectValue {
            get { return PropertyValue as Object; }
            set { PropertyValue = value; }
        }

        public virtual object PropertyValue {
            get {
                if (ParentProperty != null && FieldInfo != null)
                    return FieldInfo.GetValue(ParentProperty.PropertyValue);
                return property;
            }
            set {
                property = value;
                if (ParentProperty != null && FieldInfo != null)
                    FieldInfo.SetValue(ParentProperty.PropertyValue, property);
                else
                    Debug.Log("Set Property value is failed. the parent or field info is null");
            }
        }

        public System.Type PropertyType { get; protected set; }

        public virtual bool IsArray { get { return false; } }

        public virtual System.Type ArrayElementType { get { return null; } }

        public virtual int ArraySize { get { return 0; } }

        public FieldInfo FieldInfo { get; protected set; }

        public string DisplayName { get; protected set; }

        public bool IsExpanded { get; set; }

        public SerializedProperty ParentProperty { get { return parent; } }

        public virtual object CreatePropertyValue() {
            return CreatePropertyValue(PropertyType);
        }

        public object CreatePropertyValue(System.Type type) {
            PropertyValue = System.Activator.CreateInstance(type, true);
            return PropertyValue;
        }

        public SerializedProperty FindChildProperty(string childName) {
            FieldInfo fieldInfo = PropertyType.GetField(childName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (fieldInfo == null)
                return null;
            return FindChildProperty(fieldInfo);
        }

        public SerializedProperty FindChildProperty(FieldInfo fieldInfo) {
            SerializedProperty child;
            if (!childPropertys.TryGetValue(fieldInfo, out child)) {
                object childProperty = fieldInfo.GetValue(PropertyValue);
                if (fieldInfo.FieldType.IsArray || typeof(IList).IsAssignableFrom(fieldInfo.FieldType))
                    child = new SerializedArrayProperty(childProperty, fieldInfo.Name, fieldInfo.FieldType, fieldInfo, this);
                else
                    child = new SerializedProperty(childProperty, fieldInfo.Name, fieldInfo.FieldType, fieldInfo, this);
                childPropertys.Add(fieldInfo, child);
            }
            return child;
        }

        // Array operation
        public virtual SerializedProperty GetArrayElementAtIndex(int index) {
            return null;
        }

        public virtual void CreateArrayElementAtIndex(int index, System.Type type = null) { }

        public virtual void InsertArrayElementAtIndex(int index, object value = null) { }

        public virtual void DeleteArrayElementAtIndex(int index) { }

        public virtual void ResetArrayElementSize(int size) { }

        public virtual void MoveArrayElement(int srcIndex, int dstIndex) { }

        public IEnumerator<SerializedProperty> GetEnumerator() {
            return new PropertyEnumerator();
        }

        private IEnumerator GetEnumerator1() {
            return this.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator1();
        }

        public class PropertyEnumerator : IEnumerator<SerializedProperty> {
            SerializedProperty IEnumerator<SerializedProperty>.Current {
                get { return null; }
            }

            private object Current1 { get { 
                // SerializedProperty current = this.Current;
                return null; } }

            object IEnumerator.Current { get { return Current1; } }

            void System.IDisposable.Dispose() {
                // Dispose(true);
            }

            bool IEnumerator.MoveNext() {
                return false;
            }

            void IEnumerator.Reset() { }
        }
    }
}