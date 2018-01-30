namespace VBM {
    [System.Serializable]
    public class FlipBoolConverter : PropertyConverter {
        public override object Convert(object value) {
            return !((bool) value);
        }
    }
}