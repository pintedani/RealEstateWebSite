using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imobiliare.Entities;
using Imobiliare.Managers.ExtensionMethods;
using Imobiliare.Repositories.Interfaces;

namespace Imobiliare.Repositories
{
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Web;

    using Logging;
    using Microsoft.EntityFrameworkCore;

    public class AnsambluriRepository : Repository<Ansamblu>, IAnsambluriRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnsambluriRepository));

        public AnsambluriRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Ansamblu.Id)))
        {
            this.IncludeList.Add(nameof(Ansamblu.UserProfile));
            this.IncludeList.Add(nameof(Ansamblu.Oras));
        }

        public List<Ansamblu> GetAnsambluri(AnsambluriRezidentialeFilter filter, out int totalNumberOfPages)
        {
            var query = this.DbContext.Ansambluri.Include(nameof(Agentie.Oras));

            var allFilteredImobils =
                query.OrderByDescending(x => x.Id)
                    .Where(x => (filter.OrasId == 0 || x.OrasSelect == filter.OrasId)
                                && (filter.Active == null || x.Active == filter.Active.Value)).ToList();
                        

            totalNumberOfPages = allFilteredImobils.Count();
            return
                allFilteredImobils.Skip((filter.Page - 1) * filter.PageSize)
                    .Take(filter.PageSize)
                    .ToList();
        }

        public void Edit(Ansamblu ansamblu)
        {
            var entity = this.DbContext.Ansambluri.First(x => x.Id == ansamblu.Id);
            entity.Titlu = ansamblu.Titlu;
            entity.Continut = ansamblu.Continut;
            entity.Active = ansamblu.Active;
            entity.Keywords = ansamblu.Keywords;
            entity.DateModified = DateTime.Now;
            entity.Oras = this.DbContext.Orase.Single(x => x.Id == ansamblu.OrasSelect);

            //Inainte era doar asta dar dadea eroare "The conversion of a datetime2 data type to a datetime data type resulted in an out-of-range value"
            //db.Entry(emailTemplate).State = EntityState.Modified;
        }

        public void Delete(int id)
        {
            //Ansamblu ansamblu = this.DbContext.Ansambluri.Find(id);
            //this.DbContext.Ansambluri.Remove(ansamblu);
            //DeleteAllAnsambluriPhotos(ansamblu.Poze);
        }

        //private void DeleteAllAnsambluriPhotos(string poze)
        //{
        //    if (poze != null)
        //    {
        //        foreach (var poza in poze.Split(';'))
        //        {
        //            if (poza != string.Empty)
        //            {
        //                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/AnsambluriRezidentiale"), poza);
        //                var fileDel = new FileInfo(path);
        //                if (fileDel.Exists)
        //                {
        //                    fileDel.Delete();
        //                    log.Debug($"Deleted ansamblu rezidential photo {0}", poza);
        //                }
        //                else
        //                {
        //                    log.Error($"Attempt to remove inexisting ansamblu rezidential photo {0}", poza);
        //                }
        //            }
        //        }
        //    }
        //}

        //public string AddImage(int ansambluId, IFormFile[] IFormFiles)
        //{
        //    var itemToExtend = this.DbContext.Ansambluri.Include(nameof(Ansamblu.Oras)).First(x => x.Id == ansambluId);
        //    var lastAddedImage = AddPhotos(itemToExtend, IFormFiles);
        //    return lastAddedImage;
        //}

        public void DeleteImage(int ansambluId, string pozadeSters)
        {
        //    var itemToremove = this.DbContext.Ansambluri.First(x => x.Id == ansambluId);

        //    var allPhotos = itemToremove.Poze.Split(';');

        //    string newPictureList = string.Empty;
        //    foreach (var pictureName in allPhotos.Where(pictureName => pictureName != string.Empty && pictureName != pozadeSters))
        //    {
        //        if (newPictureList == string.Empty)
        //        {
        //            newPictureList = pictureName;
        //        }
        //        else
        //        {
        //            newPictureList += ";" + pictureName;
        //        }
        //    }

        //    itemToremove.Poze = newPictureList != string.Empty ? newPictureList : null;

        //    string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/AnsambluriRezidentiale"), pozadeSters);
        //    var fileDel = new FileInfo(path);
        //    if (fileDel.Exists)
        //    {
        //        fileDel.Delete();
        //    }
        //    else
        //    {
        //        log.Error($"Attempt to remove inexisting ansambluId photo {0}", pozadeSters);
        //    }
        }

        //Refactor this to not duplicate from Imobils
        //private static string AddPhotos(Ansamblu ansamblu, IFormFile[] files)
        //{
        //    string pictureName = string.Empty;
        //    if (files != null)
        //    {
        //        foreach (var IFormFile in files)
        //        {
        //            if (IFormFile != null)
        //            {
        //                var formattedNume = ansamblu.Titlu.RemoveSpecialCharacters('_', true);
        //                if (ansamblu.Oras != null)
        //                {
        //                    formattedNume += "_" + ansamblu.Oras.Nume.RemoveSpecialCharacters('_', true);
        //                }
        //                pictureName = formattedNume + "_" + Guid.NewGuid() + ".jpg";
        //                string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/AnsambluriRezidentiale"), pictureName);

        //                Image image = Image.FromStream(IFormFile.InputStream);
        //                //var finalImage = FixedSize(image, 640, 480);

        //                image.Save(path, ImageFormat.Jpeg);
        //                image.Dispose();
        //                image.Dispose();

        //                //pictureName = InsertWaterMark(pictureName);

        //                if (ansamblu.Poze == null)
        //                {
        //                    ansamblu.Poze = pictureName;
        //                }
        //                else
        //                {
        //                    ansamblu.Poze += ";" + pictureName;
        //                }
        //            }
        //        }
        //    }
        //    return pictureName;
        //}

        public void IncrementNumarAccesari(int ansambluIdValue)
        {
            var itemToIncrement = this.DbContext.Ansambluri.First(x => x.Id == ansambluIdValue);
            itemToIncrement.NumarVizualizari++;
        }
    }
}