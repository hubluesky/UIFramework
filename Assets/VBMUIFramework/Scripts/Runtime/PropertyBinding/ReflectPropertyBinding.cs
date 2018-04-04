using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace VBM {
    [System.Serializable]
    public class ReflectPropertyBinding : PropertyBinding {
        public Component component;
        public string memberName;

        public override void OnPropertyChange(object value) {
            ReflectionMemberUtility.CallMemberFunction(component, memberName, value);
        }
    }
}