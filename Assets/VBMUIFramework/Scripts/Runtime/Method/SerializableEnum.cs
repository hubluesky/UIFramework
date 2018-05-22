using System;
using UnityEngine;

namespace VBM {
    [Serializable]
    public class SerializableEnum : ISerializationCallbackReceiver {
        public string enumType;
        public string enumValue;

        public object enumObject { get; private set; }

        public void OnBeforeSerialize() { }

        public void OnAfterDeserialize() {
            Type enumType = Type.GetType(this.enumType);
            if (enumType != null && enumType.IsEnum && !string.IsNullOrEmpty(enumValue))
                enumObject = Enum.Parse(enumType, enumValue);
        }
    }
}