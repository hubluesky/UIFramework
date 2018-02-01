using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public sealed class ViewConfigManager : Singleton<ViewConfigManager> {
        private Dictionary<string, ViewConfig> configMap = new Dictionary<string, ViewConfig>();

        public void AddConfig(ViewConfig config) {
            if (configMap.ContainsKey(config.name)) {
                Debug.LogWarningFormat("Add config falied! has same key {0} in view config manager.", config.name);
                return;
            }
            configMap.Add(config.name, config);
        }

        public void AddConfigs(ViewConfig[] configs) {
            foreach (ViewConfig config in configs) {
                AddConfig(config);
            }
        }

        public void RemoveConfig(string name) {
            configMap.Remove(name);
        }

        public ViewConfig GetViewConfig(System.Enum value) {
            return GetViewConfig(value.ToString());
        }

        public ViewConfig GetViewConfig(string name) {
            ViewConfig config;
            if (configMap.TryGetValue(name, out config))
                return config;
            return null;
        }
    }
}