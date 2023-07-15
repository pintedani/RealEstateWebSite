using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.Managers.ExtensionMethods;
using log4net;

namespace Imobiliare.Utilities
{
    public class ImageProcessing
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImageProcessing));

        public static string AddPhotos(Imobil imobil, HttpPostedFileBase[] files)
        {
            string pictureName = string.Empty;
            if (files != null)
            {
                foreach (var httpPostedFileBase in files)
                {
                    if (httpPostedFileBase != null)
                    {
                        try
                        {
                            //This name is temporary, watermark creates real name
                            pictureName = GetFormattedImageName(imobil);
                            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"),
                                pictureName);

                            Image image = Image.FromStream(httpPostedFileBase.InputStream);

                            var scaledImage = GetResizedImageRatio(image.Width, image.Height, 640, 480);

                            var finalImage = FixedSize(image, scaledImage.Item1, scaledImage.Item2);
                            //var finalImage = FixedSize(image, 640, 480);

                            finalImage.Save(path, ImageFormat.Jpeg);
                            finalImage.Dispose();
                            image.Dispose();

                            //pictureName = InsertWaterMark(pictureName, imobil);

                            if (imobil.Poze == null)
                            {
                                imobil.Poze = pictureName;
                            }
                            else
                            {
                                imobil.Poze += ";" + pictureName;
                            }
                        }
                        catch (Exception e)
                        {
                            pictureName = null;
                            log.ErrorFormat($"Eroare la incarcarea imaginii pentru anunt, verificati sa fie de tipul imagine: {e.Message}, Stacktrace: {e.StackTrace}");
                        }
                    }
                }
            }
            return pictureName;
        }

        public void RotateImage(Imobil imobil, string pictureName)
        {
            //https://www.c-sharpcorner.com/forums/rotate-and-save-image-in-c-sharp
            var allPhotos = imobil.Poze.Split(';');

            var index = 0;
            for (var i = 0; i < allPhotos.Count(); i++)
            {
                if (allPhotos[i] == pictureName)
                {
                    index = i;
                    break;
                }
            }

            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), pictureName);

            try
            {
                var img = Image.FromFile(path);
                img.RotateFlip(RotateFlipType.Rotate90FlipNone);

                var newName = GetFormattedImageName(imobil);
                string newPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), newName);
                img.Save(newPath);

                img.Dispose();

                File.Delete(path);

                allPhotos[index] = newName;
            }
            catch (Exception exception)
            {
                log.ErrorFormat("RotateImage image {0} error {1}", pictureName, exception.Message);
            }

            string newPictureList = string.Empty;
            foreach (var picture in allPhotos.Where(x => x != string.Empty))
            {
                if (newPictureList == string.Empty)
                {
                    newPictureList = picture;
                }
                else
                {
                    newPictureList += ";" + picture;
                }
            }
            imobil.Poze = newPictureList;
        }

        public static Tuple<int, int> GetResizedImageRatio(int imgWidth, int imgHeight, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / imgWidth;
            var ratioY = (double)maxHeight / imgHeight;
            var ratio = Math.Min(ratioX, ratioY);

            var width = (int)(imgWidth * ratio);
            var height = (int)(imgHeight * ratio);
            Tuple<int, int> result = new Tuple<int, int>(width, height);
            return result;
        }

        private static Image FixedSize(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = Convert.ToInt16((Width -
                                         (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = Convert.ToInt16((Height -
                                         (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height,
                PixelFormat.Format24bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.FromArgb(249, 249, 249));
            grPhoto.InterpolationMode =
                InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }

        //Not used currently
        //private static string InsertWaterMark(string pictureName, Imobil imobil)
        //{
        //    var waterMarkedImage = GetFormattedImageName(imobil);
        //    string initialPictureLocation = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), pictureName);
        //    try
        //    {
        //        float horizontalResolution;
        //        float verticalResolution;
        //        int imgPhotoWidth;
        //        using (var imgPhoto = Image.FromFile(initialPictureLocation)) { imgPhotoWidth = imgPhoto.Width; horizontalResolution = imgPhoto.HorizontalResolution; verticalResolution = imgPhoto.VerticalResolution; }

        //        using (Image imgWatermark = new Bitmap(Path.Combine(HttpContext.Current.Server.MapPath("~/Images/DecorationImages"), "watermark.png")))
        //        using (var bmWatermark = new Bitmap(initialPictureLocation))
        //        {
        //            bmWatermark.SetResolution(horizontalResolution, verticalResolution);
        //            using (Graphics grWatermark = Graphics.FromImage(bmWatermark))
        //            {
        //                var imageAttributes = new ImageAttributes();

        //                //The first step in manipulating the watermark image is to replace 
        //                //the background color with one that is trasparent (Alpha=0, R=0, G=0, B=0)
        //                //to do this we will use a Colormap and use this to define a RemapTable
        //                ColorMap colorMap = new ColorMap();

        //                //My watermark was defined with a background of 100% Green this will
        //                //be the color we search for and replace with transparency
        //                colorMap.OldColor = Color.FromArgb(255, 0, 255, 0);
        //                colorMap.NewColor = Color.FromArgb(0, 0, 0, 0);

        //                ColorMap[] remapTable = { colorMap };

        //                imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);

        //                //The second color manipulation is used to change the opacity of the 
        //                //watermark.  This is done by applying a 5x5 matrix that contains the 
        //                //coordinates for the RGBA space.  By setting the 3rd row and 3rd column 
        //                //to 0.3f we achive a level of opacity
        //                float[][] colorMatrixElements =
        //                {
        //                    new float[] { 1.0f, 0.0f, 0.0f, 0.0f, 0.0f },
        //                    new float[] { 0.0f, 1.0f, 0.0f, 0.0f, 0.0f },
        //                    new float[] { 0.0f, 0.0f, 1.0f, 0.0f, 0.0f },
        //                    new float[] { 0.0f, 0.0f, 0.0f, 1.0f, 0.0f },
        //                    new float[] { 0.0f, 0.0f, 0.0f, 0.0f, 1.0f }
        //                };
        //                var wmColorMatrix = new ColorMatrix(colorMatrixElements);

        //                imageAttributes.SetColorMatrix(wmColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

        //                //For this example we will place the watermark in the upper right
        //                //hand corner of the photograph. offset down 10 pixels and to the 
        //                //left 10 pixles

        //                int phWidth = imgPhotoWidth;
        //                int wmWidth = imgWatermark.Width;
        //                int wmHeight = imgWatermark.Height;
        //                int xPosOfWm = ((phWidth - wmWidth) - 10);
        //                int yPosOfWm = 10;

        //                grWatermark.DrawImage(
        //                    imgWatermark,
        //                    new Rectangle(xPosOfWm, yPosOfWm, wmWidth, wmHeight),
        //                    //Set the detination Position
        //                    0,
        //                    // x-coordinate of the portion of the source image to draw. 
        //                    0,
        //                    // y-coordinate of the portion of the source image to draw. 
        //                    wmWidth,
        //                    // Watermark Width
        //                    wmHeight,
        //                    // Watermark Height
        //                    GraphicsUnit.Pixel,
        //                    // Unit of measurment
        //                    imageAttributes); //ImageAttributes Object
        //            }

        //            //replace new image to file syste
        //            bmWatermark.Save(Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), waterMarkedImage), ImageFormat.Jpeg);
        //        }
        //    }
        //    catch (Exception exception)
        //    {
        //        log.ErrorFormat("Error applying watermark to image {0}, error {1}", pictureName, exception.Message);

        //        //Use non watermarkedImage instead if not created
        //        return pictureName;
        //    }

        //    //Delete initial image
        //    try
        //    {
        //        if (File.Exists(initialPictureLocation))
        //            File.Delete(initialPictureLocation);
        //    }
        //    catch (Exception exception)
        //    {
        //        log.ErrorFormat("Error deleting initial image {0} after applying watermark, error {1}", initialPictureLocation, exception.Message);
        //    }

        //    return waterMarkedImage;
        //}

        private static string GetFormattedImageName(Imobil imobil)
        {
            string waterMarkedImage = imobil.Oras.Nume.RemoveSpecialCharacters('_', true) + "_" + imobil.TipOfertaGen + "_" +
                                      imobil.TipProprietate + "_";
            if (imobil.NumarCamere == 1)
            {
                waterMarkedImage += "1_camera_";
            }
            else if (imobil.NumarCamere > 1)
            {
                waterMarkedImage += imobil.NumarCamere + "_camere_";
            }

            if (imobil.Cartier != null)
            {
                waterMarkedImage += "cartier_" + imobil.Cartier.Nume.RemoveSpecialCharacters('_', true) + "_";
            }

            waterMarkedImage += Guid.NewGuid() + ".jpg";
            return waterMarkedImage;
        }

        public void DeleteAllImobilPhotos(string poze)
        {
            if (poze != null)
            {
                foreach (var poza in poze.Split(';'))
                {
                    if (poza != string.Empty)
                    {
                        string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), poza);
                        var fileDel = new FileInfo(path);
                        if (fileDel.Exists)
                        {
                            log.DebugFormat("Remove photo {0}", poza);
                            fileDel.Delete();
                        }
                        else
                        {
                            log.ErrorFormat("Attempt to remove inexisting anunt photo {0}", poza);
                        }
                    }
                }
            }
        }
    }
}