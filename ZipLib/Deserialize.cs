using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace ZipLib
{
    public class Deserialize
    {
        public static object DeserializeFromBinary(Stream streamRead)
        {
            object obj = null;
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                obj = formatter.Deserialize(streamRead);
            }
            catch (SerializationException ex)
            {
                Debug.WriteLine("Message ZipLib. 'DeserializeFromBinary' " + ex.Message);
            }
            finally
            {
                streamRead.Dispose();
            }
            return obj;
        }
    }
}
