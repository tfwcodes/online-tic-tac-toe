using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineTicTacToe
{
    public static class Protocol
    {
        public static void ReadAll(this Stream stream, Span<byte> buffer)
        {
            if (buffer.Length == 0)
            {
                Console.WriteLine("The length of the buffer is 0");
                return;
            }

            while (buffer.Length > 0)
            {
                var read = stream.Read(buffer);

                buffer = buffer.Slice(read);
            }
        }

        public static int ReadSize(this Stream stream)
        {
            var size = new byte[4];

            stream.ReadAll(size);

            return BitConverter.ToInt32(size);
        }

        public static byte[] ReadBytes(this Stream stream)
        {
            var size = ReadSize(stream);

            var buffer = new byte[size];

            stream.ReadAll(buffer);
            return buffer;
        }

        public static void WriteBytes(this Stream stream, Span<byte> buf)
        {
            var lengthBuf = BitConverter.GetBytes(buf.Length);

            stream.Write(lengthBuf);
            stream.Write(buf);
        }

        public static string ReadString(this Stream stream)
        {
            return Encoding.UTF8.GetString(ReadBytes(stream));
        }

        public static void WriteString(this Stream stream, string message)
        {
            stream.WriteBytes(Encoding.UTF8.GetBytes(message));
        }
    }
}
