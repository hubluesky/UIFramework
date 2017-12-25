namespace VBM {
    [System.Serializable]
    public class Float1StringConverter : PropertyConverter {
        public override object Convert(object value) {
            return string.Format("%.1f", value);
        }
    }
}