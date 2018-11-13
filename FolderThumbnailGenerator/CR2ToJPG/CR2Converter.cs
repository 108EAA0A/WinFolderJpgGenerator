using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace CR2ToJPG
{
    public class ConverterOptions
    {
        public string[] Files { get; set; }

        public string OutputDirectory { get; set; }
    }

    public static class CR2Converter
    {
        private static byte[] _buffer = new byte[_bufferSize];
        private static int _bufferSize = 512 * 1024;
        private static ImageCodecInfo _jpgImageCodec = GetJpegCodec();

        public static void ConvertImage(string fileName, string outputName, long quality = 100L, bool resize = false)
        {
            using (FileStream fi = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read, _bufferSize, FileOptions.None))
            {
                // Start address is at offset 0x62, file size at 0x7A, orientation at 0x6E
                fi.Seek(0x62, SeekOrigin.Begin);
                BinaryReader br = new BinaryReader(fi);
                UInt32 jpgStartPosition = br.ReadUInt32();  // 62
                br.ReadUInt32();  // 66
                br.ReadUInt32();  // 6A
                UInt32 orientation = br.ReadUInt32() & 0x000000FF; // 6E
                br.ReadUInt32();  // 72
                br.ReadUInt32();  // 76
                Int32 fileSize = br.ReadInt32();  // 7A

                fi.Seek(jpgStartPosition, SeekOrigin.Begin);

                Bitmap bitmap = new Bitmap(new PartialStream(fi, jpgStartPosition, fileSize));

                try
                {
                    if (_jpgImageCodec != null && (orientation == 8 || orientation == 6))
                    {
                        if (orientation == 8)
                            bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);
                        else
                            bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    }
                }
                catch (Exception ex)
                {
                    // Image Skipped
                }

                if (!resize)
                {
                    var ep = new EncoderParameters(1);
                    ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                    bitmap.Save(outputName, _jpgImageCodec, ep);
                }
                else
                {
                    using (var dest = new Bitmap(bitmap, FolderThumbnailGenerator.MainForm.getResizeSize(bitmap.Size)))
                    {
                        var ep = new EncoderParameters(1);
                        ep.Param[0] = new EncoderParameter(Encoder.Quality, quality);
                        dest.Save(outputName, _jpgImageCodec, ep);
                    }
                }
            }
        }

        private static ImageCodecInfo GetJpegCodec()
        {
            foreach (var c in ImageCodecInfo.GetImageEncoders())
            {
                if (c.CodecName.ToLower().Contains("jpeg")
                    || c.FilenameExtension.ToLower().Contains("*.jpg")
                    || c.FormatDescription.ToLower().Contains("jpeg")
                    || c.MimeType.ToLower().Contains("image/jpeg"))
                    return c;
            }

            return null;
        }
    }
}
