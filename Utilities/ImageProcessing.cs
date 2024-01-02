using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Imobiliare.Entities;
using Imobiliare.Managers.ExtensionMethods;
using Logging;
using Microsoft.AspNetCore.Http;

namespace Imobiliare.Utilities
{
    public class ImageProcessing
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ImageProcessing));

        public async static Task<string> AddPhotos(Imobil imobil, IFormFile[] files, string webRootPath)
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

                            using (Stream sourceStream = file.OpenReadStream())
                            {
                                using (Image image = await Image.LoadAsync(sourceStream))
                                {
                                    image.Mutate(x => x.Resize(new ResizeOptions
                                    {
                                        Mode = ResizeMode.Max,
                                        Size = new Size(640, 480)
                                    }));

                                    using (FileStream output = File.Create(filePath))
                                    {
                                        await image.SaveAsync(output, new SixLabors.ImageSharp.Formats.Jpeg.JpegEncoder());
                                    }
                                }
                            }

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

        public void RotateImage(Imobil imobil, string pictureName, string webRootPath)
        {
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

            string path = Path.Combine(webRootPath + "\\Images\\uploadedPhotos", pictureName);
            var newName = GetFormattedImageName(imobil);
            string rotatedImagePath = Path.Combine(webRootPath + "\\Images\\uploadedPhotos", newName);

            try
            {
                using (FileStream stream = File.OpenRead(path))
                {
                    using (Image image = Image.Load(stream))
                    {
                        //TODO: Possibility to rotate back?
                        image.Mutate(x => x.Rotate(90));

                        using (FileStream output = File.Create(rotatedImagePath))
                        {
                            image.SaveAsJpeg(output);
                        }
                    }
                }

                File.Delete(path);

                //var img = Image.FromFile(path);
                //img.RotateFlip(RotateFlipType.Rotate90FlipNone);

                //var newName = GetFormattedImageName(imobil);
                //string newPath = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), newName);
                //img.Save(newPath);

                //img.Dispose();

                //File.Delete(path);

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

        public void DeleteAllImobilPhotos(string poze, string webRootPath)
        {
            if (poze != null)
            {
                foreach (var poza in poze.Split(';'))
                {
                    if (poza != string.Empty)
                    {
                        string path = Path.Combine(webRootPath + "\\Images\\uploadedPhotos", poza);
                        var fileDel = new FileInfo(path);
                        if (fileDel.Exists)
                        {
                            log.Debug($"Remove photo {poza}");
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