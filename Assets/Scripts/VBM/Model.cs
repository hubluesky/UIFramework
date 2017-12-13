using System.Collections.Generic;

namespace VBM {
    public class Model {
        public event System.Action<string, object> propertyChanged;
        private Dictionary<string, object> variableMap = new Dictionary<string, object>();

        public T GetProperty<T>(string propertyName) {
            if (variableMap.ContainsKey(propertyName))
                return (T) variableMap[propertyName];
            return default(T);
        }

        public object GetProperty(string propertyName) {
            if (variableMap.ContainsKey(propertyName))
                return variableMap[propertyName];
            return null;
        }

        public void SetProperty(string key, object value) {
            if (variableMap.ContainsKey(key)) {
                if (variableMap[key].Equals(value))
                    return;
                variableMap[key] = value;
            } else {
                variableMap.Add(key, value);
            }
            NotifyPropertyChanged(key, value);
        }

        public void NotifyPropertyChanged(string propertyName, object value) {
            if (propertyChanged != null)
                propertyChanged(propertyName, value);
        }
    }
}