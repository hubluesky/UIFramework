using System;
using System.Collections.Generic;
using System.Reflection;
using VBM.Reflection;

namespace VBM {
    public class DictionaryModel : Model {
        protected Dictionary<string, object> variableMap = new Dictionary<string, object>();

        public override object GetProperty(string propertyName) {
            if (variableMap.ContainsKey(propertyName)) {
                return variableMap[propertyName];
            } else {
                PropertyInfo propertyInfo = this.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
                if (propertyInfo != null && propertyInfo.CanRead) {
                    variableMap.Add(propertyName, ReflectionUtility.GetDefault(propertyInfo.PropertyType)); // Must be add first. Otherwise the call will be called.
                    object result = propertyInfo.GetValue(this, null);
                    if (propertyInfo.CanWrite) {
                        variableMap[propertyName] = result;
                    } else {
                        variableMap.Remove(propertyName);
                    }
                    return result;
                }
            }
            return null;
        }

        public override void SetProperty(string key, object value) {
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