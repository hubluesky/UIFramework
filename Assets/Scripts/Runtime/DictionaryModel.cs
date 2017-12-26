using System.Collections.Generic;

namespace VBM {
    public class DictionaryModel : Model {
        protected Dictionary<string, object> variableMap = new Dictionary<string, object>();

        public override object GetProperty(string propertyName) {
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
    }
}