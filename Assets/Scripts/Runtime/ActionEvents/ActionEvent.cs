using System.Reflection;
using UnityEngine;

namespace VBM {
    public abstract class ActionEvent : MonoBehaviour {
        public ViewModelBinding viewModelBinding;
        public string[] memberNameArray = new string[0];

        public abstract System.Type ParameterType { get; }

        public bool CheckViewModelBinding() {
            if (viewModelBinding.model == null) {
                Debug.LogWarning("Slider event binding failed! the viewModelBinding model is null" + name);
                return false;
            }
            return true;
        }

        public void CallMemberFunctions() {
            foreach (string memberName in memberNameArray) {
                System.Action function = viewModelBinding.model.GetFunction(memberName);
                if (function != null)
                    function();
            }
        }

        public void CallMemberFunctions<T>(T value) {
            foreach (string memberName in memberNameArray) {
                System.Action<T> function = viewModelBinding.model.GetFunctionParam1<T>(memberName);
                if (function != null)
                    function(value);
            }
        }
    }
}