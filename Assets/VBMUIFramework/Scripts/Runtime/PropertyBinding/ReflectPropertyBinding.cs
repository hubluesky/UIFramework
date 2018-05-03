using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace VBM {
    [System.Serializable]
    public class ReflectPropertyBinding : PropertyBinding {
        [System.Serializable]
        public class ReflectMember {
            public Component component;
            public string memberName;
        }

        public ReflectMember reflectMember;

        public override void OnPropertyChange(object value) {
            ReflectionMemberUtility.CallMemberFunction(reflectMember.component, reflectMember.memberName, value);
        }
    }
}