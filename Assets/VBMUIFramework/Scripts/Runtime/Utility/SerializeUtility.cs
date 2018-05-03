using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace VBM {
    public static class SerializeUtility {
        public static byte[] SerializeObject(object obj) {
            using(MemoryStream stream = new MemoryStream()) {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                return stream.ToArray();
            }
        }

        public static object DeserializeObject(byte[] bytes) {
            using(MemoryStream stream = new MemoryStream(bytes)) {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
    }
}