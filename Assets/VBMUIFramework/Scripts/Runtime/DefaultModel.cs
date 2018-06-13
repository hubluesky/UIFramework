using System.Collections.Generic;

namespace VBM {

    public class DefaultModel : Model {
        protected Dictionary<string, GetterPropertyWrapper> variableMap = new Dictionary<string, GetterPropertyWrapper>();

        public override object GetProperty(string propertyName) {
            GetterPropertyWrapper wrapper;
            if (!variableMap.TryGetValue(propertyName, out wrapper)) {
                wrapper = GetterPropertyWrapper.CreatePropertyWrapper(this, propertyName);
                variableMap.Add(propertyName, wrapper);
            }
            return wrapper.value;
        }

        public override void SetProperty(string propertyName, object value) {
            throw new System.NotImplementedException();
        }
    }
}