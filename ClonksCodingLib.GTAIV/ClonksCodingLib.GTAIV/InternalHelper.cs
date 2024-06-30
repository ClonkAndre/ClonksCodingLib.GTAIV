using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace CCL.GTAIV
{
    internal static class InternalHelper
    {

        public static string CompressString(string uncompressedString)
        {
            try
            {
                byte[] compressedBytes;

                using (var uncompressedStream = new MemoryStream(Encoding.UTF8.GetBytes(uncompressedString)))
                {
                    using (var compressedStream = new MemoryStream())
                    {
                        using (var compressorStream = new DeflateStream(compressedStream, CompressionLevel.Optimal, true))
                            uncompressedStream.CopyTo(compressorStream);

                        compressedBytes = compressedStream.ToArray();
                    }
                }

                return Convert.ToBase64String(compressedBytes);
            }
            catch (Exception){}

            return string.Empty;
        }
        public static string DecompressString(string compressedString)
        {
            try
            {
                byte[] decompressedBytes;

                var compressedStream = new MemoryStream(Convert.FromBase64String(compressedString));

                using (var decompressorStream = new DeflateStream(compressedStream, CompressionMode.Decompress))
                {
                    using (var decompressedStream = new MemoryStream())
                    {
                        decompressorStream.CopyTo(decompressedStream);

                        decompressedBytes = decompressedStream.ToArray();
                    }
                }

                return Encoding.UTF8.GetString(decompressedBytes);
            }
            catch (Exception){}

            return string.Empty;
        }

    }
}
