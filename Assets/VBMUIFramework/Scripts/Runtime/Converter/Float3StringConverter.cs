namespace VBM {
    [System.Serializable]
    public class Float3StringConverter : PropertyConverter {
        public override object Convert(object value) {
            return string.Format("%.3f", value);
        }
    }
}