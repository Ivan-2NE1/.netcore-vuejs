using System.IO;
using System.IO.Compression;
using System.Text;

namespace VSAND.Arc.Util
{
    public static class Gzip
    {
        public static string Unzip(byte[] bytes)
        {
            using (var memoryStreamIn = new MemoryStream(bytes))
            {
                using (var memoryStreamOut = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(memoryStreamIn, CompressionMode.Decompress))
                    {
                        gzipStream.CopyTo(memoryStreamOut);
                    }

                    return Encoding.UTF8.GetString(memoryStreamOut.ToArray());
                }
            }
        }
    }
}
