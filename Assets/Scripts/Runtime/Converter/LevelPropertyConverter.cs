namespace VBM {
    [System.Serializable]
    public class LevelPropertyConverter : PropertyConverter {
        public override object Convert(object value) {
            return string.Format("Lv {0}", value);
        }
    }
}