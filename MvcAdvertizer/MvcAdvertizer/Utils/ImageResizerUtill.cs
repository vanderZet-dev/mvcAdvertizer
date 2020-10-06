using System;
using System.Drawing;
using System.IO;

namespace MvcAdvertizer.Utils
{
    public static class ImageResizerUtill
    {
        public static byte[] ScaledByWidth(byte[] bytes, string widthParam) {

            var fullSizeImage = ByteArrayToImage(bytes);
            var width = int.Parse(widthParam);
            width = WidthLimiter(width);
            var scaledImage = ScaleImage(fullSizeImage, width);

            return ImageToByte(scaledImage);
        }

        private static Image ByteArrayToImage(byte[] byteArrayIn) {

            Image returnImage = null;
            using (MemoryStream ms = new MemoryStream(byteArrayIn))
            {
                returnImage = Image.FromStream(ms);
            }
            return returnImage;
        }

        public static byte[] ImageToByte(Image img) {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        public static Image ScaleImage(Image image, int maxWidth) {

            var maxHeight = maxWidth;

            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);

            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);

            var newImage = new Bitmap(newWidth, newHeight);

            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);

            return newImage;
        }

        private static int WidthLimiter(int width) {

            var maxWidth = 2000;
            if (width > maxWidth)
            {
                width = maxWidth;
            }

            var minWidth = 10;
            if (width < minWidth)
            {
                width = minWidth;
            }

            return width;
        }
    }
}
