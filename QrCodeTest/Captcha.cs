using System;
using System.IO;
using SkiaSharp;

namespace QrCodeTest {
    internal class Captcha {
        public static SKPaint CreatePaint() {
            var font = @"Arial";
            font = @"Liberation Serif";
            font = @"Segoe Script";
            font = @"Consolas";
            //font = @"Comic Sans MS";
            //font = @"SimSun";
            //font = @"Impact";

            return CreatePaint(SKColors.White, font, 40, SKTypefaceStyle.BoldItalic);
        }

        public static SKPaint CreatePaint(SKColor color, string fontName, float fontSize, SKTypefaceStyle fontStyle) {
            var font = SKTypeface.FromFamilyName(fontName, fontStyle);

            var paint = new SKPaint {
                IsAntialias = true,
                Color = color,
                Typeface = font,
                TextSize = fontSize
            };

            // paint.StrokeCap = SKStrokeCap.Round;

            return paint;
        }


        public static SKPaint CreateLinePaint() {
            var paint = new SKPaint {
                IsAntialias = true,
                Color = SKColors.Blue,
                StrokeCap = SKStrokeCap.Square,
                StrokeWidth = 1
            };
            return paint;
        }

        internal static byte[] GetCaptcha(string captchaText,string file = null) {
            byte[] imageBytes;

            int image2d_x;
            int image2d_y;

            int compensateDeepCharacters;

            using (var drawStyle = CreatePaint()) {
                compensateDeepCharacters = (int) drawStyle.TextSize / 5;
                if (StringComparer.Ordinal.Equals(captchaText, captchaText.ToUpperInvariant()))
                    compensateDeepCharacters = 0;

                var size = SkiaHelpers.MeasureText(captchaText, drawStyle);
                image2d_x = (int) size.Width + 10;
                image2d_y = (int) size.Height + 10 + compensateDeepCharacters;
            }

            using (var image2d = new SKBitmap(image2d_x, image2d_y, SKColorType.Bgra8888, SKAlphaType.Premul)) {
                using (var canvas = new SKCanvas(image2d)) {
                    canvas.DrawColor(SKColors.Black); // Clear 

                    using (var drawStyle = CreatePaint()) {
                        canvas.DrawText(captchaText, 0 + 5, image2d_y - 5 - compensateDeepCharacters, drawStyle);
                    }

                    using (var img = SKImage.FromBitmap(image2d)) {
                        if (file != null) {
                            using (var output = File.OpenWrite(file)) {
                                img.Encode(SKEncodedImageFormat.Jpeg, 75).SaveTo(output);
                            }
                        }

                        using (var p = img.Encode(SKEncodedImageFormat.Png, 100)) {
                            imageBytes = p.ToArray();
                        }
                    }
                }
            }

            return imageBytes;
        }
    }
}