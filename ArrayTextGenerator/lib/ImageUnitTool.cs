using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;

namespace ArrayTextGenerator.lib
{
    internal class ImageUnitTool
    {
        public static Vector3 GetMaxFontSize(string message, Vector2 boxSize, string? fontPath = null)
        {
            SKPaint paint = new()
                            {
                                Color       = SKColors.Black,
                                IsAntialias = true,
                                TextAlign   = SKTextAlign.Center
                            };
            var typeface = SKTypeface.FromFamilyName("Arial",
                                                     SKFontStyleWeight.Normal,
                                                     SKFontStyleWidth.Normal,
                                                     SKFontStyleSlant.Upright);

            if (fontPath != null)
            {
                typeface = SKTypeface.FromFile(fontPath);
            }


            float minTextSize = 1;
            var   maxTextSize = MathF.Max(boxSize.X, boxSize.Y);
            float textWidth;
            float textHeight;
            paint.Typeface = typeface;

            while (minTextSize < maxTextSize - 0.5)
            {
                var textSize = (minTextSize + maxTextSize) / 2;
                paint.TextSize = textSize;

                textWidth  = paint.MeasureText(message);
                textHeight = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;

                if (textWidth < boxSize.X && textHeight < boxSize.Y)
                {
                    minTextSize = textSize;
                }
                else
                {
                    maxTextSize = textSize;
                }
            }

            paint.TextSize = minTextSize;
            textWidth      = paint.MeasureText(message);
            textHeight     = paint.FontMetrics.Descent - paint.FontMetrics.Ascent;
            return new Vector3(textWidth, textHeight, minTextSize);
        }

        public static Vector2 GetTextPosition(Vector2                boxSize, Vector3 textSize,
                                              TextHorizontalPosition textHorizontalPosition,
                                              TextVerticalPosition   textVerticalPosition)
        {
            var x = textHorizontalPosition switch
                    {
                        TextHorizontalPosition.Left             => 0,
                        TextHorizontalPosition.HorizontalCenter => (boxSize.X - textSize.X) / 2,
                        TextHorizontalPosition.Right            => boxSize.X - textSize.X,
                        _                                       => 0
                    };

            var y = textVerticalPosition switch
                    {
                        TextVerticalPosition.Top            => 0,
                        TextVerticalPosition.VerticalCenter => (boxSize.Y - textSize.Y) / 2,
                        TextVerticalPosition.Down           => boxSize.Y - textSize.Y,
                        _                                   => 0
                    };
            return new Vector2(x, y);
        }

        public static string DrawImage(Vector2       imageSize,
                                       List<string>  messageList,
                                       List<Vector2> textPositionList,
                                       List<int>     fontSizeList,
                                       List<string?> fontPathList)
        {
            var today         = DateTime.Now;
            var formattedDate = today.ToString("yyyy-MM-dd");
            var dirPath       = $"data/{formattedDate}";
            FileUnitTool.MakeDir(dirPath);
            var imageName = FileUnitTool.GetRandomFileName(dirPath, "jpg");
            var imagePath = $"{dirPath}/{imageName}";

            var bitmap = new SKBitmap((int)imageSize.X, (int)imageSize.Y,
                                      SKColorType.Rgba8888,
                                      SKAlphaType.Premul);
            using var canvas = new SKCanvas(bitmap);
            // Draw a white rectangle as the background
            var whitePaint = new SKPaint
                             {
                                 Color = SKColors.White
                             };
            canvas.DrawRect(new SKRect(0, 0, imageSize.X, imageSize.Y), whitePaint);

            for (var i = 0; i < messageList.Count; i++)
            {
                var typeFace = SKTypeface.FromFamilyName("Arial", SKFontStyleWeight.Normal,
                                                         SKFontStyleWidth.Normal,
                                                         SKFontStyleSlant.Upright);
                if (fontPathList[i] != null)
                {
                    typeFace = SKTypeface.FromFile(fontPathList[i]);
                }

                var blackPaint = new SKPaint
                                 {
                                     Color     = SKColors.Black,
                                     TextAlign = SKTextAlign.Left,
                                     Typeface  = typeFace,
                                     TextSize  = fontSizeList[i]
                                 };
                canvas.DrawText(messageList[i], textPositionList[i].X, textPositionList[i].Y, blackPaint);
                blackPaint.Dispose();
            }

            canvas.Dispose();

            using var image  = SKImage.FromBitmap(bitmap);
            using var data   = image.Encode(SKEncodedImageFormat.Png, 100);
            using var stream = File.OpenWrite(imagePath);
            data.SaveTo(stream);

            return imagePath;
        }
    }
}