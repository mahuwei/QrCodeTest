using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using SkiaSharp;
using ZXing;
using ZXing.QrCode;
using ZXing.QrCode.Internal;
using ZXing.SkiaSharp;
using BarcodeReader = ZXing.BarcodeReader;

namespace QrCodeTest {
    internal class Program {
        private static void Main(string[] args) {
            Console.WriteLine("QrCode Test\n");
            Console.WriteLine("input key \"C 数据 存储二维码文件目录\" 生成二维码.");
            Console.WriteLine("input key \"D 二维码文件目录\" 二维码解码.");
            Console.WriteLine("input key \"Q 头部分 识别信息 \" 生成授权码.");
            Console.WriteLine("input key \"A 验证码 文件目录 \" 生成验证码图片.");
            Console.WriteLine("input key \"R 种子数 \" 生成随机数.");
            Console.WriteLine("input key \"Quit\" to quit app.");
            OTP otp = null;

            do {
                var input = Console.ReadLine();
                if (string.IsNullOrEmpty(input)) continue;
                if (input.ToUpper() == "QUIT") break;
                var arr = input.Split(' ');
                if (arr.Length == 1 && arr[0].ToUpper() != "QUIT") continue;

                if (arr[0].ToUpper() == "D") {
                    try {
                        using (var stream = File.OpenRead(arr[1])) {
                            using (var inputStream = new SKManagedStream(stream)) {
                                using (var original = SKBitmap.Decode(inputStream)) {
                                    var reader = new BarcodeReader();
                                    var result = reader.Decode(original);
                                    if (result != null)
                                        Console.WriteLine(
                                            $"Decode Type:{result.BarcodeFormat.ToString()} Content:{result.Text}");
                                }
                            }
                        }
                    }
                    catch (Exception e) {
                        Console.WriteLine($"load image error:{e.Message}");
                    }
                    continue;
                }

                if (arr[0].ToUpper() == "C") {
                    var writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };
                    var encOptions = new QrCodeEncodingOptions {
                        Width = 300,
                        Height = 300,
                        Margin = 1,
                        PureBarcode = false,
                        CharacterSet = "utf-8",
                        QrVersion = 9
                    };
                    
                    encOptions.Hints.Add(EncodeHintType.ERROR_CORRECTION, ErrorCorrectionLevel.H);
                    writer.Options = encOptions;

                    var result = writer.Write(arr[1]);
                    using (var image = SKImage.FromBitmap(result)) {
                        using (var output = File.OpenWrite(arr[2])) {
                            image.Encode(SKEncodedImageFormat.Jpeg, 75).SaveTo(output);
                        }
                    }
                    continue;
                }

                if (arr[0].ToUpper() == "Q") {
                    //var code = OtpCode.Create(arr[1], arr[2]);
                    string code;
                    if (otp == null) {
                        otp = new OTP();
                        code = otp.GetCurrentOTP();
                    }
                    else {
                        code = otp.GetNextOTP();
                    }

                    Console.WriteLine($"授权码：{code}");
                    continue;
                }

                if (arr[0].ToUpper() == "R") {
                    var seed = 1000;
                    if (arr.Length <= 1) Console.WriteLine("随机数最大数不能为空。");
                    seed = Convert.ToInt32(arr[1]);
                    var r = new Random();
                    Console.WriteLine($"随机数:{r.Next((int)Math.Pow(10, arr[1].Length - 1), seed)}");
                    Console.WriteLine($"随机数(2-5):{r.Next(2, 5)}");
                    continue;
                }

                if (arr[0].ToUpper() == "A") {
                    var code = string.IsNullOrEmpty(arr[1]) ? "Hello world" : arr[1];
                    Captcha.GetCaptcha(code, arr[2]);
                }
            } while (true);
        }
    }
}