using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace RegistryLibrary.Infrastructure
{
    public static class AppSettings
    {
        /// <summary>
        /// Converts an image into a byte of arrays(binary)
        /// </summary>
        /// <param name="img">The image you want to convert</param>
        /// <return>byte[] of the image </return>
        public static byte[] ImageToBinary(this Image img)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Jpeg);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Converts the byte[] of the image into image
        /// </summary>
        /// <param name="imageData">The byte[] you want to convert to the image</param>
        /// <returns>An image of the byte[]</returns>
        public static Image BinaryToImage(this byte[] imageData)
        {
            using (MemoryStream ms = new MemoryStream(imageData))
            {
                return Image.FromStream(ms);
            }
        }

    }

}
