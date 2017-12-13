using System.Collections.Generic;
using UnityEngine;

namespace VBM {
    public class ModelManager : Singleton<ModelManager> {
        protected Dictionary<string, Model> modelMap = new Dictionary<string, Model>();

        public void RegisterModel<T>(T model) where T : Model {
            RegisterModel(typeof(T).Name, model);
        }

        public void RegisterModel(string uniqueId, Model model) {
            if (modelMap.ContainsKey(uniqueId)) {
                Debug.LogWarningFormat("Register model failed! the {0} model had ben register.", uniqueId);
                return;
            }
            modelMap.Add(uniqueId, model);
        }

        public bool UnregisterModel(Model model) {
            return UnregisterModel(model.GetType().Name);
        }

        public bool UnregisterModel(string uniqueId) {
            return modelMap.Remove(uniqueId);
        }

        public T GetModel<T>() where T : Model {
            Model model;
            modelMap.TryGetValue(typeof(T).Name, out model);
            return model as T;
        }

        public Model GetModel(string uniqueId) {
            Model model;
            modelMap.TryGetValue(uniqueId, out model);
            return model;
        }
    }
}