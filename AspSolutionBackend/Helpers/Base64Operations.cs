using System;

namespace Helpers
{
    public static class Base64Operations
    {
        public static string RepairBase64StringToCorrectFormat(string encodedBase64)
        {
            return encodedBase64 //This does not remove the initial start of the string 'date:image' etc...
                .Replace('_', '/')
                .Replace('-', '+');
            // .TrimEnd('=');
        }

        public static byte[] ValidateBase64(string base64)
        {
            var bytes = new Span<byte>(new byte[((base64.Length*3)+3)/4]); 
            if (!Convert.TryFromBase64String(base64, bytes, out var bytesWritten))
            {
                return Array.Empty<byte>();
            }
            return bytes.ToArray();
        }
    }
}