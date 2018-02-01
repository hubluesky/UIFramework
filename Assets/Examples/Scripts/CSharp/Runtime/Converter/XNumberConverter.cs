namespace VBM {
    [System.Serializable]
    public class XNumberConverter : PropertyConverter {
        public override object Convert(object value) {
            return "x" + value.ToString();
        }
    }
}