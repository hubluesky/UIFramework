using System;
using System.Reflection;

namespace VBM {
    public abstract class GetterPropertyWrapper {
        public abstract object value { get; }

        public static GetterPropertyWrapper CreatePropertyWrapper(object obj, string propertyName) {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty);
            System.Type funcType = typeof(Func<>).MakeGenericType(propertyInfo.PropertyType);
            Delegate funcDelegate = Delegate.CreateDelegate(funcType, obj, propertyInfo.GetGetMethod());
#if UNITY_IPHONE
            return new GetterPropertyWrapper() { func = funcDelegate };
#else 
            System.Type wrapperType = typeof(GetterPropertyWrapper<>).MakeGenericType(propertyInfo.PropertyType);
            return Activator.CreateInstance(wrapperType, funcDelegate) as GetterPropertyWrapper;
#endif
        }
    }

    class GeneralGetterPropertyWrapper : GetterPropertyWrapper {
        private Delegate func;
        public override object value { get { return func.DynamicInvoke(); } }
    }

    class GetterPropertyWrapper<T> : GetterPropertyWrapper {
        private Func<T> Func;
        public override object value { get { return Func(); } }
        public GetterPropertyWrapper(Delegate funcDelegate) {
            Func = funcDelegate as Func<T>;
        }
    }
}