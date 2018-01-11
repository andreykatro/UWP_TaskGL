using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace ZipLib
{
    public class Serialize
    {
        public static async Task<byte[]> ConvertFileToByteAsync(Stream streamForRead)
        {
            byte[] bytes = null;
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
            catch (Exception ex)
            {
                Debug.WriteLine("Message ZipLib. 'ConvertFileToByteAsync' " + ex.Message);
                throw;
            }
            finally
            {
                streamForRead.Dispose();
            }

            return bytes;
        }

        public static void SerializeToBinaryFile(Stream streamForWrite, object objectToSerialize)
        {
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(streamForWrite, objectToSerialize);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Message ZipLib. 'SerializeToBinaryFile' " + ex.Message);
            }
            finally
            {
                streamForWrite.Dispose();
            }
        }
    }
}
