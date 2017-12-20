using System.Collections;

namespace GeneralEditor {
    public class SerializedElementProperty : SerializedProperty {

        public SerializedElementProperty(object property, string displayName, System.Type type, SerializedArrayProperty parent, int index):
            base(property, displayName, type, parent.FieldInfo, parent) {
                Index = index;
        }

        public override object PropertyValue {
            get {
                IList list = (parent as SerializedArrayProperty).ArrayProperty;
                return list[Index];
            }
            set {
                IList list = (parent as SerializedArrayProperty).ArrayProperty;
                list[Index] = value;
            }
        }

        public int Index { get; set; }
    }
}