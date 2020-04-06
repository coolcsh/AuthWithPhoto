using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;

namespace DbImageAPI {

    public class Imaging {

        public static string GetContentTypeByImageFormat(ImageFormat format) {
            string ctype = "image/x-unknown";

            if (format.Equals(ImageFormat.Gif)) {
                ctype = "image/gif";
            } else if (format.Equals(ImageFormat.Jpeg)) {
                ctype = "image/jpeg";
            } else if (format.Equals(ImageFormat.Png)) {
                ctype = "image/png";
            } else if (format.Equals(ImageFormat.Bmp) || format.Equals(ImageFormat.MemoryBmp)) {
                ctype = "image/bmp";
            } else if (format.Equals(ImageFormat.Icon)) {
                ctype = "image/x-icon";
            } else if (format.Equals(ImageFormat.Tiff)) {
                ctype = "image/tiff";
            }

            return ctype;
        }

        public static ImageFormat GetImageFormatByContentType(string contentType) {
            ImageFormat format = null;

            if (contentType != null) {
                if (contentType.Equals("image/gif")) {
                    format = ImageFormat.Gif;
                } else if (contentType.Equals("image/jpeg") || contentType.Equals("image/pjpeg")) {
                    format = ImageFormat.Jpeg;
                } else if (contentType.Equals("image/png")) {
                    format = ImageFormat.Png;
                } else if (contentType.Equals("image/bmp")) {
                    format = ImageFormat.Bmp;
                } else if (contentType.Equals("image/x-icon")) {
                    format = ImageFormat.Icon;
                } else if (contentType.Equals("image/tiff")) {
                    format = ImageFormat.Tiff;
                }
            }

            return format;
        }

        public static string GetFileExtensionByContentType(string contentType) {
            string ext = "bin";

            if (contentType.Equals("image/gif")) {
                ext = "gif";
            } else if (contentType.Equals("image/jpeg") || contentType.Equals("image/pjpeg")) {
                ext = "jpg";
            } else if (contentType.Equals("image/png")) {
                ext = "png";
            } else if (contentType.Equals("image/bmp")) {
                ext = "bmp";
            } else if (contentType.Equals("image/x-icon")) {
                ext = "ico";
            } else if (contentType.Equals("image/tiff")) {
                ext = "tif";
            }

            return ext;
        }

        public static byte[] BytesFromImage(Image image) {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, ImageFormat.Jpeg);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] imagebytes = new byte[ms.Length];
            ms.Read(imagebytes, 0, (int)ms.Length);
            return imagebytes;
        }

        public static Image ImageFromBytes(byte[] bytes) {
            if (bytes == null || bytes.Length == 0) {
                return null;
            } else {
                MemoryStream ms = new MemoryStream(bytes);
                return Image.FromStream(ms);
            }
        }

        public static byte[] ScaleImage(byte[] bytes, int maxwidth, int maxheight) {
            return ScaleImage(bytes, maxwidth, maxheight, System.Drawing.Imaging.ImageFormat.Jpeg);
        }

        public static byte[] ScaleImage(byte[] bytes, int maxwidth, int maxheight, System.Drawing.Imaging.ImageFormat format) {
            try {
                using (System.Drawing.Image img = System.Drawing.Image.FromStream(new MemoryStream(bytes))) {
                    if (format == null) {
                        format = img.RawFormat;
                    }
                    if (img.Size.Width > maxwidth || img.Size.Height > maxheight) {
                        //resize the image to fit our website's required size
                        int newwidth = img.Size.Width;
                        int newheight = img.Size.Height;
                        if (newwidth > maxwidth) {
                            newwidth = maxwidth;
                            newheight = (int)(newheight * ((float)newwidth / img.Size.Width));
                        }
                        if (newheight > maxheight) {
                            newheight = maxheight;
                            newwidth = img.Size.Width;
                            newwidth = (int)(newwidth * ((float)newheight / img.Size.Height));
                        }

                        //resize the image to fit in the allowed image size
                        bool indexed;
                        Bitmap newimage;
                        if (img.PixelFormat == PixelFormat.Format1bppIndexed || img.PixelFormat == PixelFormat.Format4bppIndexed || img.PixelFormat == PixelFormat.Format8bppIndexed || img.PixelFormat == PixelFormat.Indexed) {
                            indexed = true;
                            newimage = new Bitmap(newwidth, newheight);
                        } else {
                            indexed = false;
                            newimage = new Bitmap(newwidth, newheight, img.PixelFormat);
                        }
                        using (newimage) {
                            using (Graphics g = Graphics.FromImage(newimage)) {
                                if (indexed) {
                                    g.FillRectangle(Brushes.White, 0, 0, newwidth, newheight);
                                }
                                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                                g.DrawImage(img, new Rectangle(0, 0, newwidth, newheight));
                            }
                            using (MemoryStream ms = new MemoryStream()) {
                                newimage.Save(ms, format);
                                bytes = ms.ToArray();
                            }
                        }
                    } else if (img.RawFormat != format) {
                        using (MemoryStream ms = new MemoryStream()) {
                            img.Save(ms, format);
                            bytes = ms.ToArray();
                        }
                    }
                    return bytes;
                }
            } catch (ArgumentException) {
                return null;
            }
        }

    }

}