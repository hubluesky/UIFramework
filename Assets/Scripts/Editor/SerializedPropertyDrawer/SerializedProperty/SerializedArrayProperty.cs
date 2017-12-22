using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace GeneralEditor {
    public class SerializedArrayProperty : SerializedProperty {
        protected List<SerializedElementProperty> arrayPropertys;

        public SerializedArrayProperty(object property, string displayName, SerializedProperty parent) : base(property, displayName, parent) { }

        public SerializedArrayProperty(object property, string displayName, System.Type type, FieldInfo fieldInfo, SerializedProperty parent):
            base(property, displayName, type, fieldInfo, parent) { }

        public override bool IsArray { get { return true; } }

        public IList ArrayProperty { get { return PropertyValue as IList; } }

        public override int ArraySize { get { return ArrayProperty != null ? ArrayProperty.Count : 0; } }

        public override System.Type ArrayElementType {
            get {
                if (PropertyType.IsArray)
                    return PropertyType.GetElementType();
                System.Type[] arguments = PropertyType.GetGenericArguments();
                return arguments.Length == 1 ? arguments[0] : null;
            }
        }

        public override object CreatePropertyValue() {
            PropertyValue = System.Activator.CreateInstance(PropertyType, 0);
            return PropertyValue;
        }

        public override SerializedProperty GetArrayElementAtIndex(int index) {
            IList list = ArrayProperty;
            if (arrayPropertys == null)
                arrayPropertys = new List<SerializedElementProperty>(list.Count);
            for (int i = arrayPropertys.Count; i < list.Count; i++) {
                System.Type type = list[i] != null ? list[i].GetType() : ArrayElementType;
                string displayName = string.Format("{0}.element {1}", DisplayName, i);
                arrayPropertys.Add(new SerializedElementProperty(list[i], displayName, type, this, i));
            }
            return arrayPropertys[index];
        }

        public override void CreateArrayElementAtIndex(int index, System.Type type = null) {
            object value;
            if (type != null) {
                value = System.Activator.CreateInstance(type, true);
            } else {
                if (ArrayElementType == typeof(string))
                    value = string.Empty;
                else
                    value = System.Activator.CreateInstance(ArrayElementType, true);
            }
            InsertArrayElement(index, value);
        }

        public override void InsertArrayElementAtIndex(int index, object value = null) {
            if (value == null) {
                if (ArrayElementType == typeof(string))
                    value = string.Empty;
                else
                    value = System.Activator.CreateInstance(ArrayElementType, true);
            }
            InsertArrayElement(index, value);
        }

        protected void InsertArrayElement(int index, object value) {
            if (PropertyValue == null)
                PropertyValue = System.Activator.CreateInstance(PropertyType, 0);

            if (PropertyType.IsArray) {
                System.Array array = PropertyValue as System.Array;
                System.Array newArray = System.Array.CreateInstance(ArrayElementType, array.Length + 1);
                System.Array.Copy(array, 0, newArray, 0, index);
                if (index < array.Length)
                    System.Array.Copy(array, index, newArray, index + 1, array.Length - index);
                newArray.SetValue(value, index);
                PropertyValue = newArray;
            } else {
                ArrayProperty.Insert(index, value);
            }
            if (arrayPropertys != null) {
                string displayName = string.Format("{0}.element {1}", DisplayName, index);
                arrayPropertys.Insert(index, new SerializedElementProperty(value, displayName, value.GetType(), this, index));
                for (int i = index + 1; i < arrayPropertys.Count; i++)
                    arrayPropertys[i].Index = i;
            }
        }

        public override void DeleteArrayElementAtIndex(int index) {
            if (PropertyType.IsArray) {
                System.Array array = PropertyValue as System.Array;
                System.Array newArray = System.Array.CreateInstance(ArrayElementType, array.Length - 1);
                System.Array.Copy(array, 0, newArray, 0, index);
                if (index + 1 < array.Length)
                    System.Array.Copy(array, index + 1, newArray, index, array.Length - index);
                PropertyValue = newArray;
            } else {
                ArrayProperty.RemoveAt(index);
            }
            if (arrayPropertys != null) {
                arrayPropertys.RemoveAt(index);
                for (int i = index; i < arrayPropertys.Count; i++)
                    arrayPropertys[i].Index = i;
            }
        }

        public override void ResetArrayElementSize(int size) {
            IList list = ArrayProperty;
            for (int i = list.Count; i < size; i++)
                InsertArrayElementAtIndex(i);
            for (int i = list.Count - 1; i >= size; i--)
                DeleteArrayElementAtIndex(i);
        }

        public override void MoveArrayElement(int srcIndex, int dstIndex) {
            SwapArrayElement(ArrayProperty, srcIndex, dstIndex);
            if (arrayPropertys != null) {
                SwapArrayElement(arrayPropertys, srcIndex, dstIndex);
                arrayPropertys[srcIndex].Index = srcIndex;
                arrayPropertys[dstIndex].Index = dstIndex;
            }
        }

        public static void SwapArrayElement(IList list, int srcIndex, int dstIndex) {
            object temp = list[srcIndex];
            list[srcIndex] = list[dstIndex];
            list[dstIndex] = temp;
        }
    }
}