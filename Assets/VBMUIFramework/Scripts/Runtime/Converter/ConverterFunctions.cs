using System;
using UnityEngine;

namespace VBM {
    public static class ConverterFunctions {
        [PropertyConverter]
        public static string StringFormat1(string format, object param1) {
            return string.Format(format, param1);
        }

        [PropertyConverter]
        public static string StringFormat2(string format, object param1, object param2) {
            return string.Format(format, param1, param2);
        }

        [PropertyConverter]
        public static string StringFormat3(string format, object param1, object param2, object param3) {
            return string.Format(format, param1, param2, param3);
        }

        [PropertyConverter]
        public static float NormalizeInt(int min, int max) {
            return max == 0 ? 0 : (float)min / max;
        }

        [PropertyConverter]
        public static float NormalizeFloat(float min, float max) {
            return max == 0 ? 0 : min / max;
        }

        [PropertyConverter]
        public static float CeilString(float value) {
            return UnityEngine.Mathf.Ceil(value);
        }

        [PropertyConverter]
        public static bool IsNull(object value) {
            return value == null;
        }

        [PropertyConverter]
        public static bool IsNotNull(object value) {
            return value != null;
        }

        [PropertyConverter]
        public static float FloatAdd(float a, float b) {
            return a + b;
        }

        [PropertyConverter]
        public static int IntAdd(int a, int b) {
            return a + b;
        }

        [PropertyConverter]
        public static bool IsGreaterThan(IComparable value, IComparable other) {
            return value.CompareTo(other) > 0;
        }

        [PropertyConverter]
        public static bool IsGreaterEqualThan(IComparable value, IComparable other) {
            return value.CompareTo(other) >= 0;
        }

        [PropertyConverter]
        public static bool IsEqualThan(IComparable value, IComparable other) {
            return value.CompareTo(other) == 0;
        }

        [PropertyConverter]
        public static bool IsNotEqualThan(IComparable value, IComparable other) {
            return value.CompareTo(other) != 0;
        }

        [PropertyConverter]
        public static bool IsLessThan(IComparable value, IComparable other) {
            return value.CompareTo(other) < 0;
        }

        [PropertyConverter]
        public static bool IsLessEqualThan(IComparable value, IComparable other) {
            return value.CompareTo(other) <= 0;
        }

        [PropertyConverter]
        public static bool IsEqual(object a, object b) {
            return a == b;
        }

        [PropertyConverter]
        public static bool IsNotEqual(object a, object b) {
            return a != b;
        }

        [PropertyConverter]
        public static bool FipBool(bool value) {
            return !value;
        }
    }
}