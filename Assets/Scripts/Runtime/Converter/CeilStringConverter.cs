namespace VBM {
    [System.Serializable]
    public class CeilStringConverter : PropertyConverter {
        public override object Convert(object value) {
            float v = System.Convert.ToSingle(value);
            return ((int) UnityEngine.Mathf.Ceil(v)).ToString();
        }
    }
}