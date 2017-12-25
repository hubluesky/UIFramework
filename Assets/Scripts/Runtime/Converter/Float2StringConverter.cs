namespace VBM {
    [System.Serializable]
    public class Float2StringConverter : PropertyConverter {
        public override object Convert(object value) {
            return string.Format("%.2f", value);
        }
    }
}