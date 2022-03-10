using IconCobbler.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace IconCobbler
{
    public static class IconMaker
    {

        public static void MakeIconFile(string outputFilePath, IEnumerable<string> sourceImagePaths)
        {
            if (sourceImagePaths.Any() == false)
            {
                throw new InvalidOperationException("There must be one or more source files to read.");
            }
            List<ImageInfo> images = new List<ImageInfo>();

            foreach (var imagePath in sourceImagePaths)
            {
                using var fs = System.IO.File.OpenRead(imagePath);
                var decoder = new PngBitmapDecoder(fs, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.OnDemand);
                
                // check
                var width = decoder.Frames[0].PixelWidth;
                var height = decoder.Frames[0].PixelHeight;

                if (width != height)
                {
                    throw new BadAspectRatioException(imagePath);
                }
                if (width < 16)
                {
                    throw new ImageTooSmallException(imagePath);
                }
                if (width > 256)
                {
                    throw new ImageTooLargeException(imagePath);
                }

                images.Add(new ImageInfo()
                {
                    BitDepth = decoder.Frames[0].Format.BitsPerPixel,
                    PaletteLength = decoder.Frames[0].Palette is null ? 0 : decoder.Frames[0].Palette.Colors.Count,
                    Height = height,
                    Width = width,
                    Length = (int)fs.Length
                });
            }

            // create the .ico file format
            using var ico = System.IO.File.Create(outputFilePath);
            using var writer = new System.IO.BinaryWriter(ico);

            int headerSize = 6 + (16 * images.Count());
            int currImageOffset = headerSize;

            // write ICONDIR
            writer.Write((ushort)0);              // must be zero
            writer.Write((ushort)1);              // 1 for .ico (2 for .cur)
            writer.Write((ushort)images.Count()); // number of images

            // write ICONDIRENTRY
            foreach (var image in images)
            {
                // width x height
                writer.Write((byte)(image.Width == 255 ? 0 : image.Width));
                writer.Write((byte)(image.Height == 255 ? 0 : image.Height));

                // palette
                writer.Write((byte)image.PaletteLength);

                // reserved
                writer.Write((byte)0);

                // color planes (this should probably always be 1 for PNGs?)
                writer.Write((ushort)1);

                // bits per pixel
                writer.Write((ushort)image.BitDepth);

                // size of image in bytes
                writer.Write(image.Length);

                // offset of the image in the file
                writer.Write(currImageOffset);

                currImageOffset += image.Length;
            }

            // write the image data
            foreach (var file in sourceImagePaths)
            {
                // write the images
                using var fs = System.IO.File.OpenRead(file);
                fs.CopyTo(ico);
            }
            ico.Flush();
        }

    }
}
