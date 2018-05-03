using System.Reflection;
using UnityEngine;

namespace VBM {
    public abstract class ActionEvent : MonoBehaviour {
        public ViewModelBinding viewModelBinding;
        public ActionEventMethod[] methodArray = new ActionEventMethod[0];
        public MethodParameter parameter;
        public abstract System.Type ParameterType { get; }

        public bool CheckViewModelBinding() {
            if (viewModelBinding.model == null) {
                Debug.LogWarning("Slider event binding failed! the viewModelBinding model is null" + name);
                return false;
            }
            return true;
        }

        public void CallMemberFunctions() {
            foreach (ActionEventMethod memberName in methodArray) {
                if (memberName.parameterType != null) {
                    System.Delegate function = viewModelBinding.model.GetFunctionParam1(memberName.methodName, memberName.parameterType);
                    function.DynamicInvoke(memberName.parameterValue);
                } else {
                    System.Action function = viewModelBinding.model.GetFunction(memberName.methodName);
                    if (function != null)
                        function();
                }
            }
        }

        public void CallMemberFunctions<T>(T value) {
            foreach (ActionEventMethod memberName in methodArray) {
                if (memberName.parameterType != null) {
                    System.Delegate function = viewModelBinding.model.GetFunctionParam1(memberName.methodName, memberName.parameterType);
                    function.DynamicInvoke(memberName.parameterValue);
                } else {
                    System.Action<T> function = viewModelBinding.model.GetFunctionParam1<T>(memberName.methodName);
                    if (function != null)
                        function(value);
                }
            }
        }
    }
}