using System;
using System.Collections.Generic;

namespace VBM {
    public abstract class Model : IModel {
        protected Dictionary<string, Delegate> functionMap = new Dictionary<string, Delegate>();
        public event System.Action<string, object> propertyChanged;

        public virtual System.Action GetFunction(string funcName) {
            Delegate function;
            if (!functionMap.TryGetValue(funcName, out function)) {
                function = ReflectionMemberUtility.GetMemberFunction(this, funcName);
                if (function != null)
                    functionMap.Add(funcName, function);
            }
            return function as System.Action;
        }

        public virtual System.Delegate GetFunctionParam1(string funcName, System.Type paramType) {
            Delegate function;
            if (!functionMap.TryGetValue(funcName, out function)) {
                function = ReflectionMemberUtility.GetMemberFunction(this, funcName, paramType);
                if (function != null)
                    functionMap.Add(funcName, function);
            }
            return function;
        }

        public virtual Action<T> GetFunctionParam1<T>(string funcName) {
            Delegate function;
            if (!functionMap.TryGetValue(funcName, out function)) {
                function = ReflectionMemberUtility.GetMemberFunction<T>(this, funcName);
                if (function != null)
                    functionMap.Add(funcName, function);
            }
            return function as Action<T>;
        }

        public virtual bool CheckElementModel(ViewModelBinding binding) {
            return true;
        }

        public virtual T GetProperty<T>(string propertyName) {
            return (T) GetProperty(propertyName);
        }

        public abstract object GetProperty(string propertyName);

        public abstract void SetProperty(string propertyName, object value);

        public void NotifyPropertyChanged(string propertyName, object value) {
            if (propertyChanged != null)
                propertyChanged(propertyName, value);
        }
    }
}