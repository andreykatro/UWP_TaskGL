using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace ClassLibrary2_0
{
    public class Class1
    {
        public static async Task<byte[]> ConvertFileToByteAsync(Stream streamForRead)
        {
            MyList myList = new MyList();

            byte[] bytes = null;
            using (streamForRead)
            {
                try
                {
                    bytes = new byte[streamForRead.Length];
                    const int BUFFER_SIZE = 1024;
                    byte[] buffer = new byte[BUFFER_SIZE];
                    int position = 0;
                    int bytesread = 0;
                    while ((bytesread = await streamForRead.ReadAsync(buffer, 0, BUFFER_SIZE)) > 0)
                    {
                        for (int i = 0; i < bytesread; i++, position++)
                        {
                            bytes[position] = buffer[i];
                        }
                    }
                }
                catch(Exception ex)
                {
                    Debug.WriteLine("Message Lib2_0. 'ConvertFileToByteAsync' " + ex.Message);
                }     
            }
            return bytes;
        }
        public static void SerializeToBinaryFile(Stream streamForWrite, object objectToSerialize)
        {
            using (streamForWrite)
            {
                BinaryFormatter formatter = new BinaryFormatter();
                try
                {
                    formatter.Serialize(streamForWrite, objectToSerialize);
                }
                catch (SerializationException ex)
                {
                    Debug.WriteLine("Message Lib2_0. 'Serialize' " + ex.Message);

                }
            }
        }


        //    public static void Deserialize()
        //    {
        //        // Declare the hashtable reference.
        //        Hashtable addresses = null;

        //        // Open the file containing the data that you want to deserialize.
        //        FileStream fs = new FileStream("DataFile.dat", FileMode.Open);
        //        try
        //        {
        //            BinaryFormatter formatter = new BinaryFormatter();

        //            // Deserialize the hashtable from the file and 
        //            // assign the reference to the local variable.
        //            addresses = (Hashtable)formatter.Deserialize(fs);
        //        }
        //        catch (SerializationException e)
        //        {
        //            Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
        //            throw;
        //        }
        //        finally
        //        {
        //            fs.Close();
        //        }

        //        // To prove that the table deserialized correctly, 
        //        // display the key/value pairs.
        //        foreach (DictionaryEntry de in addresses)
        //        {
        //            Console.WriteLine("{0} lives at {1}.", de.Key, de.Value);
        //        }
        //    }
    }
}

