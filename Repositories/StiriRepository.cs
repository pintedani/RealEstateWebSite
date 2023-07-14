using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Caching;
using Imobiliare.Entities;
using Imobiliare.Repositories.Interfaces;

namespace Imobiliare.Repositories
{
    public class StiriRepository : Repository<Stire>, IStiriRepository
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(StiriRepository));

        public StiriRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Stire.DateCreated), SortDirection.Descending))
        {
        }

        public List<Stire> GetLastAddedStiri()
        {
            if (StiriCaching.LastAddedStiri.Any())
            {
                return StiriCaching.LastAddedStiri;
            }

            var lastAddedStiri = this.Find(x => x.Active).Take(3).ToList();
            StiriCaching.LastAddedStiri = lastAddedStiri;

            return lastAddedStiri;
        }

        public void Edit(Stire stire)
        {
            var entity = this.DbContext.Stires.First(x => x.Id == stire.Id);
            entity.Titlu = stire.Titlu;
            entity.Continut = stire.Continut;
            entity.Active = stire.Active;
            entity.AfiseazaPrimaPagina = stire.AfiseazaPrimaPagina;
            entity.Keywords = stire.Keywords;
            entity.CategorieStire = stire.CategorieStire;
            entity.DateModified = DateTime.Now;
            entity.OrasSelect = stire.OrasSelect;
            entity.LinkExtern = stire.LinkExtern;
            entity.TitluUrl = stire.TitluUrl;
            entity.MetaDescription = stire.MetaDescription;
        }

        public void Delete(int id)
        {
            Stire stire = this.DbContext.Stires.Find(id);
            this.DbContext.Stires.Remove(stire);
            DeleteAllStiriPhotos(stire.Poze);
        }

        public string AddImage(int stireId, HttpPostedFileBase[] httpPostedFileBases)
        {
            var itemToExtend = this.DbContext.Stires.First(x => x.Id == stireId);
            var lastAddedImage = AddPhotos(itemToExtend, httpPostedFileBases);
            return lastAddedImage;
        }

        public void DeleteImage(int stireId, string pozadeSters)
        {
            var itemToremove = this.DbContext.Stires.First(x => x.Id == stireId);

            var allPhotos = itemToremove.Poze.Split(';');

            string newPictureList = string.Empty;
            foreach (var pictureName in allPhotos.Where(pictureName => pictureName != string.Empty && pictureName != pozadeSters))
            {
                if (newPictureList == string.Empty)
                {
                    newPictureList = pictureName;
                }
                else
                {
                    newPictureList += ";" + pictureName;
                }
            }

            itemToremove.Poze = newPictureList != string.Empty ? newPictureList : null;

            string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Stiri"), pozadeSters);
            var fileDel = new FileInfo(path);
            if (fileDel.Exists)
            {
                fileDel.Delete();
            }
            else
            {
                log.ErrorFormat("Attempt to remove inexisting stire photo {0}", pozadeSters);
            }
        }

        public Tuple<Stire, Stire> FindBeforeAndAfterNavigationStiri(int id, StiriFilter stiriFilter)
        {
            var stireAnt = (from x in this.DbContext.Stires where x.Active == stiriFilter.Active && x.Id > id orderby x.Id ascending select x).FirstOrDefault();
            var stireUrm = (from x in this.DbContext.Stires where x.Active == stiriFilter.Active && x.Id < id orderby x.Id descending select x).FirstOrDefault();

            return new Tuple<Stire, Stire>(stireAnt, stireUrm);
        }

        public Stire TryGetCachedStire(int id)
        {
            if (StiriCaching.Stiri.Any(x => x.Id == id))
            {
                return StiriCaching.Stiri.Single(x => x.Id == id);
            }

            var stire = base.Single(x=>x.Id == id);

            StiriCaching.Stiri.Add(stire);

            return stire;
        }

        //public override Stire Single(Expression<Func<Stire, bool>> expression)
        //{
        //    if (StiriCaching.Stiri.Where(expression.Compile()).Any())
        //    {
        //        return StiriCaching.Stiri.Single(expression.Compile());
        //    }

        //    var stire = base.Single(expression);

        //    StiriCaching.Stiri.Add(stire);

        //    return stire;
        //}

        //Refactor this to not duplicate from Imobils
        private static string AddPhotos(Stire stire, HttpPostedFileBase[] files)
        {
            string pictureName = string.Empty;
            if (files != null)
            {
                foreach (var httpPostedFileBase in files)
                {
                    if (httpPostedFileBase != null)
                    {
                        pictureName = Guid.NewGuid() + ".jpg";
                        string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Stiri"), pictureName);

                        Image image = Image.FromStream(httpPostedFileBase.InputStream);
                        //var finalImage = FixedSize(image, 640, 480);

                        image.Save(path, ImageFormat.Jpeg);
                        image.Dispose();
                        image.Dispose();

                        //pictureName = InsertWaterMark(pictureName);

                        if (stire.Poze == null)
                        {
                            stire.Poze = pictureName;
                        }
                        else
                        {
                            stire.Poze += ";" + pictureName;
                        }
                    }
                }
            }
            return pictureName;
        }

        private void DeleteAllStiriPhotos(string poze)
        {
            if (poze != null)
            {
                foreach (var poza in poze.Split(';'))
                {
                    if (poza != string.Empty)
                    {
                        string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Stiri"), poza);
                        var fileDel = new FileInfo(path);
                        if (fileDel.Exists)
                        {
                            fileDel.Delete();
                            log.DebugFormat("Deleted stire photo {0}", poza);
                        }
                        else
                        {
                            log.ErrorFormat("Attempt to remove inexisting stire photo {0}", poza);
                        }
                    }
                }
            }
        }
    }
}
