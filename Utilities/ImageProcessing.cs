using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.Managers.ExtensionMethods;
using Logging;
using Microsoft.AspNetCore.Http;

namespace Imobiliare.Utilities
{
    public class ImageProcessing
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImageProcessing));

        public static string AddPhotos(Imobil imobil, IFormFile[] files, string webRootPath)
        {
            string pictureName = string.Empty;
            if (files != null)
            {
                foreach (var file in files)
                {
                    if (file != null && file.Length > 0)
                    {
                        try
                        {
                            //This name is temporary, watermark creates real name
                            pictureName = GetFormattedImageName(imobil);

                            string uploadsFolder = Path.Combine(webRootPath, "Images\\uploadedPhotos");

                            string filePath = Path.Combine(uploadsFolder, pictureName);

                            // Save the file to the specified path
                            using (var fileStream = new FileStream(filePath, FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                            }

                            //Image image = Image.FromStream(file.InputStream);

                            //var scaledImage = GetResizedImageRatio(image.Width, image.Height, 640, 480);

                            //var finalImage = FixedSize(image, scaledImage.Item1, scaledImage.Item2);
                            ////var finalImage = FixedSize(image, 640, 480);

                            //finalImage.Save(path, ImageFormat.Jpeg);
                            //finalImage.Dispose();
                            //image.Dispose();

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

        //private static Image FixedSize(Image imgPhoto, int Width, int Height)
        //{
        //    int sourceWidth = imgPhoto.Width;
        //    int sourceHeight = imgPhoto.Height;
        //    int sourceX = 0;
        //    int sourceY = 0;
        //    int destX = 0;
        //    int destY = 0;

        //    float nPercent = 0;
        //    float nPercentW = 0;
        //    float nPercentH = 0;

        //    nPercentW = ((float)Width / (float)sourceWidth);
        //    nPercentH = ((float)Height / (float)sourceHeight);
        //    if (nPercentH < nPercentW)
        //    {
        //        nPercent = nPercentH;
        //        destX = Convert.ToInt16((Width -
        //                                 (sourceWidth * nPercent)) / 2);
        //    }
        //    else
        //    {
        //        nPercent = nPercentW;
        //        destY = Convert.ToInt16((Height -
        //                                 (sourceHeight * nPercent)) / 2);
        //    }

        //    int destWidth = (int)(sourceWidth * nPercent);
        //    int destHeight = (int)(sourceHeight * nPercent);

        //    Bitmap bmPhoto = new Bitmap(Width, Height,
        //        PixelFormat.Format24bppRgb);
        //    bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
        //        imgPhoto.VerticalResolution);

        //    Graphics grPhoto = Graphics.FromImage(bmPhoto);
        //    grPhoto.Clear(Color.FromArgb(249, 249, 249));
        //    grPhoto.InterpolationMode =
        //        InterpolationMode.HighQualityBicubic;

        //    grPhoto.DrawImage(imgPhoto,
        //        new Rectangle(destX, destY, destWidth, destHeight),
        //        new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
        //        GraphicsUnit.Pixel);

        //    grPhoto.Dispose();
        //    return bmPhoto;
        //}

        public void RotateImage(Imobil imobil, string pictureName)
        {
            ////https://www.c-sharpcorner.com/forums/rotate-and-save-image-in-c-sharp
            //var allPhotos = imobil.Poze.Split(';');

            //var index = 0;
            //for (var i = 0; i < allPhotos.Count(); i++)
            //{
            //    if (allPhotos[i] == pictureName)
            //    {
            //        index = i;
            //        break;
            //    }
            //}

            //string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), pictureName);

            //try
            //{
            //    var img = Image.FromFile(path);
            //    img.RotateFlip(RotateFlipType.Rotate90FlipNone);

            //    var newName = GetFormattedImageName(imobil);
            //    string newPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), newName);
            //    img.Save(newPath);

            //    img.Dispose();

            //    File.Delete(path);

            //    allPhotos[index] = newName;
            //}
            //catch (Exception exception)
            //{
            //    log.ErrorFormat("RotateImage image {0} error {1}", pictureName, exception.Message);
            //}

            //string newPictureList = string.Empty;
            //foreach (var picture in allPhotos.Where(x => x != string.Empty))
            //{
            //    if (newPictureList == string.Empty)
            //    {
            //        newPictureList = picture;
            //    }
            //    else
            //    {
            //        newPictureList += ";" + picture;
            //    }
            //}
            //imobil.Poze = newPictureList;
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
                        //string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), poza);
                        //var fileDel = new FileInfo(path);
                        //if (fileDel.Exists)
                        //{
                        //    log.DebugFormat("Remove photo {0}", poza);
                        //    fileDel.Delete();
                        //}
                        //else
                        //{
                        //    log.ErrorFormat("Attempt to remove inexisting anunt photo {0}", poza);
                        //}
                    }
                }
            }
        }
    }
}