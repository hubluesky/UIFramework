using System;
using UnityEngine;

namespace VBM {
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.ReturnValue, AllowMultiple = true)]
    public class PropertyConverterAttribute : PropertyAttribute {
    }
}