using System;
using System.Security.Cryptography;
using System.Text;

namespace QrCodeTest {
    public class OtpCode {
        public static string Create(string header, string seed) {
            var dtStart = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
            var seconds = (int)(DateTime.Now - dtStart).TotalSeconds;

            var a = $"{seed}{seconds}";
            Console.WriteLine($"seed+ticks={a}");
            var d = DateTime.Now - (new DateTime(1970, 1, 1));
            Console.WriteLine($"{(int)d.TotalSeconds}");

            SHA1 sha1 = new SHA1CryptoServiceProvider();
            var bytes_in = Encoding.Default.GetBytes(a);
            var bytes_out = sha1.ComputeHash(bytes_in, 0, 14);
            Console.WriteLine($"bytes_out长度：{bytes_out.Length}");
            sha1.Dispose();
            var result = BitConverter.ToString(bytes_out);
            result = result.Replace("-", "");

            if (header == null) header = "";
            return $"{header}{result}";
        }
    }
}