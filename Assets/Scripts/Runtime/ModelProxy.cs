namespace VBM {
    public class ModelProxy : Model {
        public System.Func<string, object> GetPropertyFunc;

        public override object GetProperty(string propertyName) {
            return GetPropertyFunc(propertyName);
        }
    }
}