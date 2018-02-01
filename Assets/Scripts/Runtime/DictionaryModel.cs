using System;
using System.Collections.Generic;

namespace VBM {
    public class DictionaryModel : Model {
        protected Dictionary<string, object> variableMap = new Dictionary<string, object>();
        protected Dictionary<string, Delegate> functionMap = new Dictionary<string, Delegate>();

        public override object GetProperty(string propertyName) {
            if (variableMap.ContainsKey(propertyName))
                return variableMap[propertyName];
            return null;
        }

        public override Action GetFunction(string funcName) {
            Delegate function;
            if (!functionMap.TryGetValue(funcName, out function)) {
                function = ReflectionMemberUtility.GetMemberFunction(this, funcName);
                if (function != null)
                    functionMap.Add(funcName, function);
            }
            return function as Action;
        }

        public override Action<T> GetFunctionParam1<T>(string funcName) {
            Delegate function;
            if (!functionMap.TryGetValue(funcName, out function)) {
                function = ReflectionMemberUtility.GetMemberFunction<T>(this, funcName);
                if (function != null)
                    functionMap.Add(funcName, function);
            }
            return function as Action<T>;
        }

        public void SetProperty(string key, object value) {
            if (variableMap.ContainsKey(key)) {
                if (variableMap[key] == value || (variableMap[key] != null && variableMap[key].Equals(value)))
                    return;
                variableMap[key] = value;
            } else {
                variableMap.Add(key, value);
            }
            NotifyPropertyChanged(key, value);
        }
    }
}