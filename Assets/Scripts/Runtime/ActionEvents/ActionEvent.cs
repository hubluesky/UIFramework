using System.Reflection;
using UnityEngine;

namespace VBM {
    public abstract class ActionEvent : MonoBehaviour {
        public ViewModelBinding viewModelBinding;
        public string[] memberNameArray = new string[0];

        public abstract System.Type ParameterType { get; }

        public void CallMemberFunctions() {
            foreach (string memberName in memberNameArray) {
                ReflectionMemberUtility.CallMemberFunction(viewModelBinding.model, memberName);
            }
        }

        public void CallMemberFunctions<T>(T value) {
            foreach (string memberName in memberNameArray) {
                ReflectionMemberUtility.CallMemberFunction(viewModelBinding.model, memberName, value);
            }
        }
    }
}