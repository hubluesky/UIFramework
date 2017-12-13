namespace VBM {
    public class GeneralPropertyConverter : PropertyConverter {
        public System.Type targetType;

        public override object Conver(object value) {
            return System.Convert.ChangeType(value, targetType);
        }
    }
}