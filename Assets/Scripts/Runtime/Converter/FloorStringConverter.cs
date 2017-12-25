namespace VBM {
    [System.Serializable]
    public class FloorStringConverter : PropertyConverter {
        public override object Convert(object value) {
            float v = System.Convert.ToSingle(value);
            return ((int) UnityEngine.Mathf.Floor(v)).ToString();
        }
    }
}