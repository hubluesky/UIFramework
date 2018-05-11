using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using VBM;
using VBM.Reflection;

namespace VBMEditor {
    public class ModelReflection {
        public const BindingFlags modelPropertyFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        public static ModelReflection instance { get; private set; }

        static ModelReflection() {
            List<System.Type> typeList = ReflectionUtility.GetClassTypeFromAssembly(typeof(ModelReflection));
            System.Type type = FindSubClassType<ModelReflection>(typeList);
            instance = System.Activator.CreateInstance(type) as ModelReflection;
        }

        static System.Type FindSubClassType<T>(List<System.Type> typeList) {
            if (typeList.Count == 0) return typeof(T);
            foreach (System.Type type in typeList) {
                if (type.IsSubclassOf(typeof(T)))
                    return type;
            }
            return typeList[0];
        }

        protected List<System.Type> modelTypeList;

        public ModelReflection() {
            modelTypeList = ReflectionUtility.GetModelTypeList();
        }

        public virtual int IndexOfModel(string name) {
            return modelTypeList.FindIndex((element) => { return element.FullName == name; });
        }

        public virtual void ForeachModelName(System.Action<string> OnModelName) {
            foreach (System.Type type in modelTypeList) {
                OnModelName(type.FullName);
            }
        }

        public virtual void ForeachProperty(int index, System.Action<string, System.Type> OnProperty) {
            ReflectionUtility.ForeachGetClassProperty(modelTypeList[index], (propertyInfo) => {
                if (propertyInfo.CanRead)
                    OnProperty(propertyInfo.Name, propertyInfo.PropertyType);
            });
        }

        public virtual bool CheckModelProperty(string parentName, string modelName) {
            int index = IndexOfModel(string.IsNullOrEmpty(parentName) ? modelName : parentName);
            if (index == -1) return false;

            if (string.IsNullOrEmpty(parentName)) {
                return true;
            } else {
                PropertyInfo modelPropertyInfo = modelTypeList[index].GetProperty(modelName, modelPropertyFlags);
                if (modelPropertyInfo == null)
                    return false;
                return true;
            }
        }

        public virtual void ForeachProperty(string parentName, string modelName, System.Action<string, System.Type> OnProperty) {
            int index = IndexOfModel(string.IsNullOrEmpty(parentName) ? modelName : parentName);
            if (index == -1) return;

            System.Type modelType;
            if (string.IsNullOrEmpty(parentName)) {
                modelType = modelTypeList[index];
            } else {
                PropertyInfo modelPropertyInfo = modelTypeList[index].GetProperty(modelName, modelPropertyFlags);
                if (modelPropertyInfo == null)
                    return;
                modelType = modelPropertyInfo.PropertyType;
            }

            ReflectionUtility.ForeachGetClassProperty(modelType, (propertyInfo) => {
                if (propertyInfo.CanRead)
                    OnProperty(propertyInfo.Name, propertyInfo.PropertyType);
            });
        }
    }
}