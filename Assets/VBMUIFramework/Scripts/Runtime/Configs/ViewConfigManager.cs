using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public sealed class ViewConfigManager : Singleton<ViewConfigManager>, IEnumerable {
        private Dictionary<string, ViewConfig> configMap = new Dictionary<string, ViewConfig>();

        public void AddConfig(ViewConfig config) {
            if (configMap.ContainsKey(config.viewName)) {
                Debug.LogWarningFormat("Add config falied! has same key {0} in view config manager.", config.viewName);
                return;
            }
            configMap.Add(config.viewName, config);
        }

        public void AddConfigs(ViewConfig[] configs) {
            foreach (ViewConfig config in configs) {
                AddConfig(config);
            }
        }

        public void RemoveConfig(string name) {
            configMap.Remove(name);
        }

        public ViewConfig GetViewConfig<T>() where T : IModel {
            return GetViewConfig(typeof(T).Name);
        }

        public ViewConfig GetViewConfig(string name) {
            ViewConfig config;
            if (configMap.TryGetValue(name, out config))
                return config;
            return null;
        }

        public IEnumerator GetEnumerator() {
            return configMap.Values.GetEnumerator();
        }
    }
}