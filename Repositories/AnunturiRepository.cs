using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using Caching;
using Imobiliare.Entities;
using Imobiliare.Managers.ExtensionMethods;
using Imobiliare.Repositories.Interfaces;
using Imobiliare.Utilities;
using Logging;

namespace Imobiliare.Repositories
{
    public class AnunturiRepository : Repository<Imobil>, IAnunturiRepository
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnunturiRepository));
        private readonly ImageProcessing _imageProcessing;

        public AnunturiRepository(ApplicationDbContext dbContext) : base(dbContext, new SortSpec(nameof(Imobil.Id)))
        {
            //Needed For Find Anunturi Adaugate
            this.IncludeList.Add(nameof(Imobil.UserProfile));
            this.IncludeList.Add(nameof(Imobil.Cartier));
            this.IncludeList.Add(nameof(Imobil.Oras));
            this.IncludeList.Add(nameof(Imobil.Judet));
            _imageProcessing = new ImageProcessing();
        }

        public List<Imobil> GetAllImobil(ImobilFilter imobilFilter, out int totalNumberOfPages)
        {
            var dateLimit = DateTime.Now;
            if (imobilFilter.VechimeMaximaZile > 0)
            {
                dateLimit = DateTime.Now.AddDays(-imobilFilter.VechimeMaximaZile);
            }

            double x1 = 0, y1 = 0, x2 = 0, y2 = 0;

            if (imobilFilter.GoogleMapBounds != null)
            {
                var mapBounds = imobilFilter.GoogleMapBounds.Replace("(", "").Replace(")", "");
                x1 = double.Parse(mapBounds.Split(',')[0], CultureInfo.InvariantCulture);
                y1 = double.Parse(mapBounds.Split(',')[1], CultureInfo.InvariantCulture);
                x2 = double.Parse(mapBounds.Split(',')[2], CultureInfo.InvariantCulture);
                y2 = double.Parse(mapBounds.Split(',')[3], CultureInfo.InvariantCulture);

                //Reset initial judet/oras if google markers specified
                imobilFilter.JudetId = 0;
                imobilFilter.OrasId = 0;
            }

            //get user profile id
            UserProfile userProfile = null;
            if (imobilFilter.UserProfile != null)
            {
                userProfile = this.DbContext.Users.FirstOrDefault(x => x.UserName == imobilFilter.UserProfile.UserName);
            }
            string userProfileId = null;
            if (userProfile != null)
                userProfileId = userProfile.Id;

            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("UserProfile");
            query.Include("UserProfile.Agentie");
            query.Include("UserProfile.Constructor");
            query.Include("Judet");
            query.Include("Oras");
            var allFilteredImobils =
              query.OrderByDescending(x => x.DataAdaugare)
                .Where(x =>
                  (x.Cartier.Id == imobilFilter.CartierId || imobilFilter.CartierId == 0)
                  && (x.Oras.Id == imobilFilter.OrasId || imobilFilter.OrasId == 0)
                  && (x.Judet.Id == imobilFilter.JudetId || imobilFilter.JudetId == 0)
                  && (imobilFilter.StareAprobare == 0 || x.StareAprobare == imobilFilter.StareAprobare)
                  && (imobilFilter.CamereMin == 0 || x.NumarCamere >= imobilFilter.CamereMin)
                  && (imobilFilter.CamereMax == 0 || x.NumarCamere <= imobilFilter.CamereMax)
                  && (imobilFilter.MpMin == 0 || x.Suprafata >= imobilFilter.MpMin)
                  && (imobilFilter.MpMax == 0 || x.Suprafata <= imobilFilter.MpMax)
                  && (imobilFilter.PretMin == 0 || x.Price >= imobilFilter.PretMin)
                  && (imobilFilter.PretMax == 0 || x.Price <= imobilFilter.PretMax)
                  && (imobilFilter.VechimeMaximaZile == 0 || x.DataAdaugare >= dateLimit)
                  && (imobilFilter.PerioadaStart == DateTime.MinValue
                      || (!imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugare >= imobilFilter.PerioadaStart)
                      ||
                      (imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugareInitiala >= imobilFilter.PerioadaStart))
                  && (imobilFilter.PerioadaEnd == DateTime.MinValue
                      || (!imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugare <= imobilFilter.PerioadaEnd)
                      || (imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugareInitiala <= imobilFilter.PerioadaEnd))
                  && (!imobilFilter.OnlyUserAnunturi
                      || (imobilFilter.OnlyUserAnunturi && x.UserProfile.Role != Role.Administrator))
                  && (!imobilFilter.OnlyAdminAnunturi
                      || (imobilFilter.OnlyAdminAnunturi && x.UserProfile.Role == Role.Administrator))
                  && (x.TipProprietate == imobilFilter.TipProprietate || imobilFilter.TipProprietate == 0)
                  && (x.TipOfertaGen == imobilFilter.TipOfertaGen || imobilFilter.TipOfertaGen == 0)
                  && (x.TipVanzator == imobilFilter.TipVanzator || imobilFilter.TipVanzator == TipVanzator.TotiVanzatorii)
                  && (userProfileId == null || x.UserId == userProfileId)

                  && (imobilFilter.OnlyWithGoogleMarker == false || (x.GoogleMarkerCoordinateLat != 0
                                                                     && x1 <= x.GoogleMarkerCoordinateLat
                                                                     && x2 >= x.GoogleMarkerCoordinateLat
                                                                     && y1 <= x.GoogleMarkerCoordinateLong
                                                                     && y2 >= x.GoogleMarkerCoordinateLong)));
            totalNumberOfPages = allFilteredImobils.Count();
            return
              allFilteredImobils.Skip((imobilFilter.Page - 1) * imobilFilter.PageSize)
                .Take(imobilFilter.PageSize)
                .ToList();
        }

        /// <summary>
        /// Take 4 last added anunturi, and based on needs take 4/2
        /// </summary>
        public List<Imobil> GetLastAddedImobils(int number)
        {
            if (AnunturiCaching.LastAddedAnunturi != null)
            {
                return AnunturiCaching.LastAddedAnunturi.Take(number).ToList();
            }

            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("Oras");
            query.Include("Judet");
            //Doar anunturile de oferta, nu si de cautare
            lastAddedImobils =
              query.OrderByDescending(x => x.DataAdaugare)
                .Where(
                  x =>
                    x.StareAprobare == StareAprobare.Aprobat
                    && x.TipOfertaGen != TipOfertaGen.VreauSaCumpar && x.TipOfertaGen != TipOfertaGen.VreauSaInchiriez)
                .Take(4)
                .ToList();

            AnunturiCaching.LastAddedAnunturi = lastAddedImobils;

            return lastAddedImobils;
        }

        /// <summary>
        /// Returns all the time 3 anunturi
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        public List<Imobil> GetLastAddedAnunturiCautare(int number)
        {
            if (AnunturiCaching.LastAddedAnunturiCautare != null)
            {
                return AnunturiCaching.LastAddedAnunturiCautare;
            }

            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("Oras");
            query.Include("Judet");
            lastAddedImobils =
              query.OrderByDescending(x => x.DataAdaugare)
                .Where(
                  x =>
                    x.StareAprobare == StareAprobare.Aprobat
                    && (x.TipOfertaGen == TipOfertaGen.VreauSaCumpar || x.TipOfertaGen == TipOfertaGen.VreauSaInchiriez))
                .Take(number)
                .ToList();

            AnunturiCaching.LastAddedAnunturiCautare = lastAddedImobils;

            return lastAddedImobils;
        }

        public List<Imobil> GetLastAddedImobilsOnlyWithPictures(int number)
        {
            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("Oras");
            query.Include("Judet");
            lastAddedImobils =
              query.OrderByDescending(x => x.DataAdaugare)
                .Where(x => x.StareAprobare == StareAprobare.Aprobat && x.Poze != null)
                .Take(number)
                .ToList();

            return lastAddedImobils;
        }

        public List<Imobil> GetPromovatedImobils(ImobilFilter imobilFilter, PromovatLevel promovatedLevel)
        {
            List<Imobil> promovatedImobils;

            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("Oras");
            query.Include("Judet");
            query.Include(nameof(Imobil.UserProfile));
            promovatedImobils = query.Where(x => x.PromotedLevel >= promovatedLevel
                                                 && x.StareAprobare == StareAprobare.Aprobat
                                                 && (imobilFilter.TipOfertaGen == TipOfertaGen.Toate || x.TipOfertaGen == imobilFilter.TipOfertaGen)
                                                 && x.Judet.Id == imobilFilter.JudetId).ToList();

            if (promovatedImobils.Count > 0)
            {
                var promovatedRandomList = promovatedImobils.Randomize().Take(3).ToList();
                return promovatedRandomList;
            }
            return promovatedImobils;
        }

        public Imobil GetImobil(int imobilId)
        {
            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("Judet");
            query.Include("Oras");
            query.Include("Cartier");
            query.Include("UserProfile");
            return query.FirstOrDefault(x => x.Id == imobilId);
        }

        public List<Imobil> GetSimilarAnunturi(int imobilId)
        {
            var similarAnunturi = new List<Imobil>();
            Imobil anuntSelectat;
            var query = this.DbContext.Imobile.Include("Cartier");
            query.Include("Judet");
            query.Include("Oras");
            query.Include("Cartier");
            query.Include("UserProfile");
            anuntSelectat = query.FirstOrDefault(x => x.Id == imobilId);

            similarAnunturi = query.Where(x => x.Id != anuntSelectat.Id &&
                                               (x.TipProprietate == anuntSelectat.TipProprietate || anuntSelectat.TipProprietate == 0) &&
                                               x.StareAprobare == StareAprobare.Aprobat &&
                                               x.Judet.Id == anuntSelectat.Judet.Id && x.Oras.Id == anuntSelectat.Oras.Id)
              .ToList();

            if (similarAnunturi.Count() > 0)
            {
                return similarAnunturi.Randomize().ToList();
            }

            return similarAnunturi;
        }

        public bool UpdateImobil(Imobil imobil, bool isAdmin)
        {
            var imobilToEdit = this.DbContext.Imobile.First(x => x.Id == imobil.Id);
            imobilToEdit.Descriere = imobil.Descriere.Trim();
            imobilToEdit.AerConditionat = imobil.AerConditionat;
            imobilToEdit.CT = imobil.CT;
            if (imobil.Oras != null)
            {
                imobilToEdit.Oras = this.DbContext.Orase.FirstOrDefault(x => x.Id == imobil.Oras.Id);
                imobilToEdit.Judet = this.DbContext.Judete.FirstOrDefault(x => x.Id == imobilToEdit.Oras.JudetId);
            }
            if (imobil.Cartier != null)
            {
                imobilToEdit.Cartier = this.DbContext.Cartiere.FirstOrDefault(x => x.Id == imobil.Cartier.Id);
            }
            imobilToEdit.Etaj = imobil.Etaj;
            imobilToEdit.NumarTotalEtaje = imobil.NumarTotalEtaje;
            imobilToEdit.Garaj = imobil.Garaj;
            imobilToEdit.LocParcare = imobil.LocParcare;
            imobilToEdit.Decomandat = imobil.Decomandat;
            imobilToEdit.Finisat = imobil.Finisat;
            imobilToEdit.LocInPivnita = imobil.LocInPivnita;
            imobilToEdit.Negociabil = imobil.Negociabil;
            imobilToEdit.NrBai = imobil.NrBai;
            imobilToEdit.NrBalcoane = imobil.NrBalcoane;
            imobilToEdit.Price = imobil.Price;
            imobilToEdit.Strada = imobil.Strada;
            imobilToEdit.Suprafata = imobil.Suprafata;
            imobilToEdit.Title = imobil.Title.Trim();
            imobilToEdit.Valabilitate = imobil.Valabilitate;
            imobilToEdit.VechimeImobil = imobil.VechimeImobil;
            imobilToEdit.NumarCamere = imobil.NumarCamere;
            imobilToEdit.ContactTelefon = imobil.ContactTelefon.Trim();
            imobilToEdit.ContactEmail = imobil.ContactEmail.Trim();
            imobilToEdit.TipProprietate = imobil.TipProprietate;
            imobilToEdit.TipOfertaGen = imobil.TipOfertaGen;
            imobilToEdit.TipVanzator = imobil.TipVanzator;
            imobilToEdit.GoogleMarkerCoordinates = imobil.GoogleMarkerCoordinates;

            if (isAdmin)
            {
                imobilToEdit.PromotedLevel = imobil.PromotedLevel;
                imobilToEdit.ObservatiiAdmin = imobil.ObservatiiAdmin;
            }

            //Reenable when photo/map position does not affect rowversion!!!
            //if (!imobilToEdit.RowVersion.SequenceEqual(imobil.RowVersion))
            //  throw new DbUpdateConcurrencyException();

            return true;
        }

        public bool UpdateImobilNew(Imobil imobil, bool isAdmin)
        {
            var imobilToEdit = this.DbContext.Imobile.First(x => x.Id == imobil.Id);
            imobilToEdit.Title = imobil.Title.Trim();
            imobilToEdit.Descriere = imobil.Descriere.Trim();
            imobilToEdit.Price = imobil.Price;
            imobilToEdit.TipOfertaGen = imobil.TipOfertaGen;
            imobilToEdit.TipProprietate = imobil.TipProprietate;
            imobilToEdit.TipVanzator = imobil.TipVanzator;
            imobilToEdit.Suprafata = imobil.Suprafata;
            imobilToEdit.ContactTelefon = imobil.ContactTelefon.Trim();
            imobilToEdit.ContactEmail = imobil.ContactEmail;
            imobilToEdit.GoogleMarkerCoordinates = imobil.GoogleMarkerCoordinates;

            if (imobil.Oras != null)
            {
                imobilToEdit.Oras = this.DbContext.Orase.FirstOrDefault(x => x.Id == imobil.Oras.Id);
                imobilToEdit.Judet = this.DbContext.Judete.FirstOrDefault(x => x.Id == imobilToEdit.Oras.JudetId);
            }
            if (imobil.Cartier != null)
            {
                imobilToEdit.Cartier = this.DbContext.Cartiere.FirstOrDefault(x => x.Id == imobil.Cartier.Id);
            }

            imobilToEdit.Strada = imobil.Strada;
            imobilToEdit.VechimeImobil = imobil.VechimeImobil;
            imobilToEdit.Negociabil = imobil.Negociabil;
            imobilToEdit.AerConditionat = imobil.AerConditionat;
            imobilToEdit.CT = imobil.CT;
            imobilToEdit.Garaj = imobil.Garaj;
            imobilToEdit.LocParcare = imobil.LocParcare;
            imobilToEdit.Finisat = imobil.Finisat;
            imobilToEdit.Decomandat = imobil.Decomandat;
            imobilToEdit.LocInPivnita = imobil.LocInPivnita;
            imobilToEdit.Valabilitate = imobil.Valabilitate;
            imobilToEdit.Etaj = imobil.Etaj;
            imobilToEdit.NumarTotalEtaje = imobil.NumarTotalEtaje;
            imobilToEdit.NumarCamere = imobil.NumarCamere;
            imobilToEdit.NrBai = imobil.NrBai;
            imobilToEdit.NrBalcoane = imobil.NrBalcoane;
            imobilToEdit.PromotedLevel = imobil.PromotedLevel;

            return true;
        }

        public bool DeleteImobil(int imobilId)
        {
            var itemToremove = this.DbContext.Imobile.First(x => x.Id == imobilId);
            this.DbContext.Imobile.Remove(itemToremove);
            _imageProcessing.DeleteAllImobilPhotos(itemToremove.Poze);

            return true;
        }

        public void DeactivareAnunt(int imobilId)
        {
            var itemToremove = this.DbContext.Imobile.First(x => x.Id == imobilId);
            itemToremove.StareAprobare = StareAprobare.Dezactivat;
        }

        public void ReactivareAnunt(int imobilId)
        {
            //Done only by end user
            var itemToUpdate = this.DbContext.Imobile.First(x => x.Id == imobilId);
            //itemToUpdate.DataAdaugare = DateTime.Now;
            if (itemToUpdate.StareAprobare == StareAprobare.InAsteptare)
            {
                throw new Exception("End user can not Reactivate anunt if in stare InAsteptare state before");
            }
            itemToUpdate.StareAprobare = StareAprobare.Aprobat;
            itemToUpdate.DataAdaugare = DateTime.Now;
        }

        public void ChangeImobilState(int imobilId, StareAprobare stareAprobare)
        {
            var itemToUpdate = this.DbContext.Imobile.First(x => x.Id == imobilId);
            itemToUpdate.StareAprobare = stareAprobare;
            if (stareAprobare == StareAprobare.Aprobat)
            {
                itemToUpdate.DataAdaugare = DateTime.Now;
                itemToUpdate.DataAprobare = DateTime.Now;
            }
        }

        public void ReActualizareAnunt(int imobilId)
        {
            var itemToActualizate = this.DbContext.Imobile.First(x => x.Id == imobilId);
            itemToActualizate.DataAdaugare = DateTime.Now;
            itemToActualizate.StareAprobare = StareAprobare.Aprobat;
        }

        public Imobil AddImobil(Imobil imobil, string userName)
        {
            Task<Imobil> addedImobil;
            imobil.DataAdaugare = DateTime.Now;
            imobil.DataAdaugareInitiala = DateTime.Now;
            imobil.UserProfile = this.DbContext.Users.First(x => x.UserName == userName);
            if (imobil.UserProfile.Role == Role.Administrator || imobil.UserProfile.TrustedUser)
            {
                imobil.StareAprobare = StareAprobare.Aprobat;
            }
            else
            {
                imobil.StareAprobare = StareAprobare.InAsteptare;
            }
            if (imobil.Cartier != null)
            {
                //Cartier may be null
                imobil.Cartier = this.DbContext.Cartiere.FirstOrDefault(x => x.Id == imobil.Cartier.Id);
            }


            imobil.Oras = this.DbContext.Orase.Single(x => x.Id == imobil.Oras.Id);
            imobil.Judet = this.DbContext.Judete.Single(x => x.Id == imobil.Oras.JudetId);

            imobil.Title = imobil.Title.Trim();
            imobil.Descriere = imobil.Descriere.Trim();
            imobil.ContactTelefon = imobil.ContactTelefon.Trim();

            imobil.TipVanzator = imobil.TipVanzator;
            addedImobil = this.DbContext.AddAsync(this.DbContext, imobil);

            return addedImobil.Result;
        }

        public void DeleteImage(int imobilId, string imageName)
        {
            var itemToremove = this.DbContext.Imobile.First(x => x.Id == imobilId);
            var allPhotos = itemToremove.Poze.Split(';');

            string newPictureList = string.Empty;
            foreach (var pictureName in allPhotos.Where(pictureName => pictureName != string.Empty && pictureName != imageName))
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

            //Actual delete
            _imageProcessing.DeleteAllImobilPhotos(imageName);
        }

        public void MovePhotoUp(int imobilId, string movePozaUp)
        {
            var itemToremove = this.DbContext.Imobile.First(x => x.Id == imobilId);
            var allPhotos = itemToremove.Poze.Split(';');

            var index = 0;
            for (var i = 0; i < allPhotos.Count(); i++)
            {
                if (allPhotos[i] == movePozaUp)
                {
                    index = i;
                }
            }
            Swap(allPhotos, index, index - 1);
            string newPictureList = string.Empty;
            foreach (var pictureName in allPhotos.Where(pictureName => pictureName != string.Empty))
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
            itemToremove.Poze = newPictureList;
        }

        public void MovePhotoDown(int imobilId, string movePozaDown)
        {
            var itemToremove = this.DbContext.Imobile.First(x => x.Id == imobilId);
            var allPhotos = itemToremove.Poze.Split(';');

            var index = 0;
            for (var i = 0; i < allPhotos.Count(); i++)
            {
                if (allPhotos[i] == movePozaDown)
                {
                    index = i;
                }
            }
            Swap(allPhotos, index, index + 1);
            string newPictureList = string.Empty;
            foreach (var pictureName in allPhotos.Where(pictureName => pictureName != string.Empty))
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
            itemToremove.Poze = newPictureList;
        }

        public void RotatePhoto(int imobilId, string rotatePoza)
        {
            var imobil = this.DbContext.Imobile.Include(nameof(Imobil.Oras)).First(x => x.Id == imobilId);
            new ImageProcessing().RotateImage(imobil, rotatePoza);
        }

        static void Swap(IList<string> list, int indexA, int indexB)
        {
            string tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
        }

        //public string AddImages(int imobilId, HttpPostedFileBase[] files)
        //{
        //    string lastAddedImage = string.Empty;
        //    var itemToExtend = this.DbContext.Imobile.Include(nameof(Imobil.Oras)).Include(nameof(Imobil.Cartier)).First(x => x.Id == imobilId);
        //    lastAddedImage = ImageProcessing.AddPhotos(itemToExtend, files);
        //    return lastAddedImage;
        //}

        public void RemoveGoogleMarkerCoordinates(int imobilId)
        {
            var itemToExtend = this.DbContext.Imobile.First(x => x.Id == imobilId);
            itemToExtend.GoogleMarkerCoordinates = null;
        }

        public Dictionary<string, int> GetTotalNumarAnunturiPerJudete()
        {
            if (AnunturiCaching.NumarAnunturiPerJudete != null)
            {
                return AnunturiCaching.NumarAnunturiPerJudete;
            }
            log.Debug("Rebuilding AnunturiNumberCaching.NumarAnunturiPerJudete because of anunt state changed");

            var judeteAnunturiDict = new Dictionary<string, int>();
            var judete = this.DbContext.Judete.ToList();
            foreach (var imobil in this.DbContext.Imobile.Where(x => x.StareAprobare == StareAprobare.Aprobat))
            {
                var judet = judete.First(x => x.Id == imobil.Judet.Id);
                if (!judeteAnunturiDict.ContainsKey(judet.Nume))
                {
                    judeteAnunturiDict.Add(judet.Nume, 1);
                }
                else
                {
                    judeteAnunturiDict[judet.Nume]++;
                }
            }

            AnunturiCaching.NumarAnunturiPerJudete = judeteAnunturiDict;
            return judeteAnunturiDict;
        }

        public List<Imobil> CheckForExpiredAnunturi()
        {
            var expiredAnunturi = new List<Imobil>();
            try
            {
                {
                    foreach (var imobil in this.DbContext.Imobile.Include("UserProfile").Where(x => x.StareAprobare == StareAprobare.Aprobat))
                    {
                        if (imobil.DataAdaugare.AddDays(imobil.Valabilitate) < DateTime.Now)
                        {
                            expiredAnunturi.Add(imobil);
                            imobil.StareAprobare = StareAprobare.Expirat;
                            log.DebugFormat("Anuntul cu id {0} a expirat. Titlu {1}", imobil.Id, imobil.Title);
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                log.ErrorFormat("Error while Checking for expired anunturi: {0}", exception.Message);
            }

            return expiredAnunturi;
        }

        public void AddCustomNoteToImobil(int id, string note)
        {
            var itemToApprove = this.DbContext.Imobile.First(x => x.Id == id);
            itemToApprove.ObservatiiAdmin += " | " + note;
        }

        public void ClearPhotosExceptFirst(int anuntId)
        {
            //var itemToEdit = this.DbContext.Imobile.First(x => x.Id == anuntId);

            ////Delete all except first photo
            //foreach (var poza in itemToEdit.Poze.Split(';').Skip(1))
            //{
            //    if (poza != string.Empty)
            //    {
            //        string path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/uploadedPhotos"), poza);
            //        var fileDel = new FileInfo(path);
            //        if (fileDel.Exists)
            //        {
            //            fileDel.Delete();
            //        }
            //        else
            //        {
            //            log.ErrorFormat("Attempt to remove inexisting anunt photo {0} for anunt with id {1}", poza, anuntId);
            //        }
            //    }
            //}

            //itemToEdit.Poze = itemToEdit.Poze.Split(';')[0];
        }

        public Imobil GetImobilRelatedToExternalLink(string externalLink)
        {
            return this.DbContext.Imobile.FirstOrDefault(x => x.LinkExtern == externalLink);
        }

        public Dictionary<TipProprietate, int> GetSituationForTipOferta(ImobilFilter imobilFilter)
        {
            Dictionary<TipProprietate, int> situationTipOfertaDictionary = new Dictionary<TipProprietate, int>();

            foreach (var tipOferta in Enum.GetValues(typeof(TipProprietate)).Cast<TipProprietate>().Except(new[] { TipProprietate.Toate }))
            {
                situationTipOfertaDictionary.Add(tipOferta, 0);
            }
            Microsoft.EntityFrameworkCore.DbSet<Imobil> query = this.DbContext.Imobile;

            var allFilteredImobils = ConstructFilterQuery(imobilFilter, query);
            foreach (var imobil in allFilteredImobils)
            {
                //TODO DAPI do something with this anunturi which are still on Toate
                if (imobil.TipProprietate != TipProprietate.Toate)
                {
                    situationTipOfertaDictionary[imobil.TipProprietate]++;
                }
            }

            return situationTipOfertaDictionary;
        }

        public Dictionary<TipOfertaGen, int> GetSituationForTipProprietate(ImobilFilter imobilFilter)
        {
            Dictionary<TipOfertaGen, int> situationTipOfertaDictionary = new Dictionary<TipOfertaGen, int>();

            foreach (var tipOferta in Enum.GetValues(typeof(TipOfertaGen)).Cast<TipOfertaGen>().Except(new[] { TipOfertaGen.Toate }))
            {
                situationTipOfertaDictionary.Add(tipOferta, 0);
            }
            var query = this.DbContext.Imobile;

            var allFilteredImobils = ConstructFilterQuery(imobilFilter, query);
            foreach (var imobil in allFilteredImobils)
            {
                //TODO DAPI do something with this anunturi which are still on Toate
                if (imobil.TipOfertaGen != TipOfertaGen.Toate)
                {
                    situationTipOfertaDictionary[imobil.TipOfertaGen]++;
                }
            }

            return situationTipOfertaDictionary;
        }

        private static IQueryable<Imobil> ConstructFilterQuery(ImobilFilter imobilFilter, Microsoft.EntityFrameworkCore.DbSet<Imobil> query)
        {
            return query.Where(
              x =>
                (x.Cartier.Id == imobilFilter.CartierId || imobilFilter.CartierId == 0)
                && (x.Oras.Id == imobilFilter.OrasId || imobilFilter.OrasId == 0)
                && (x.Judet.Id == imobilFilter.JudetId || imobilFilter.JudetId == 0)
                && (imobilFilter.StareAprobare == 0 || x.StareAprobare == imobilFilter.StareAprobare)
                && (imobilFilter.CamereMin == 0 || x.NumarCamere >= imobilFilter.CamereMin)
                && (imobilFilter.CamereMax == 0 || x.NumarCamere <= imobilFilter.CamereMax)
                && (imobilFilter.MpMin == 0 || x.Suprafata >= imobilFilter.MpMin)
                && (imobilFilter.MpMax == 0 || x.Suprafata <= imobilFilter.MpMax)
                && (imobilFilter.PretMin == 0 || x.Price >= imobilFilter.PretMin)
                && (imobilFilter.PretMax == 0 || x.Price <= imobilFilter.PretMax)
                && (imobilFilter.PerioadaStart == DateTime.MinValue
                    || (!imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugare >= imobilFilter.PerioadaStart)
                    || (imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugareInitiala >= imobilFilter.PerioadaStart))
                && (imobilFilter.PerioadaEnd == DateTime.MinValue
                    || (!imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugare <= imobilFilter.PerioadaEnd)
                    || (imobilFilter.FiltareDupaDataAdaugareInitiala && x.DataAdaugareInitiala <= imobilFilter.PerioadaEnd))
                && (!imobilFilter.OnlyUserAnunturi
                    || (imobilFilter.OnlyUserAnunturi && x.UserProfile.Role != Role.Administrator))
                && (!imobilFilter.OnlyAdminAnunturi
                    || (imobilFilter.OnlyAdminAnunturi && x.UserProfile.Role == Role.Administrator))
                && (x.TipProprietate == imobilFilter.TipProprietate || imobilFilter.TipProprietate == 0)
                && (x.TipOfertaGen == imobilFilter.TipOfertaGen || imobilFilter.TipOfertaGen == 0)
                && (x.TipVanzator == imobilFilter.TipVanzator || imobilFilter.TipVanzator == TipVanzator.TotiVanzatorii));
        }

        public string GetTelefonForAnunt(string anuntId)
        {
            var parsedId = int.Parse(anuntId);
            var anunt = this.DbContext.Imobile.FirstOrDefault(x => x.Id == parsedId);
            if (anunt != null)
            {
                return anunt.ContactTelefon;
            }

            return string.Empty;
        }

        public void IncrementNumarAccesari(int imobilId)
        {
            var itemToIncrement = this.DbContext.Imobile.First(x => x.Id == imobilId);
            itemToIncrement.NumarVizualizari++;
        }

        public Imobil GetAnuntByTitle(string title)
        {
            var anunt = this.DbContext.Imobile.FirstOrDefault(x => x.Title == title.Trim());
            if (anunt != null)
            {
                return anunt;
            }

            return null;
        }

        public Tuple<int, int> DeletePozeForExpiredAnunturiOlderThanDate(DateTime dateTime)
        {
            Tuple<int, int> result;
            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile;
            lastAddedImobils = query.Where(x => x.StareAprobare != StareAprobare.Aprobat && x.Poze != null && x.DataAdaugare < dateTime).ToList();

            var anunturiWithMultiplePhotos = lastAddedImobils.Where(x => x.NumarPoze > 1).ToList();
            log.DebugFormat("Starting photo cleanup: {0} with 1 photo already, {1} with more than one photo",
                anunturiWithMultiplePhotos.Count(), (lastAddedImobils.Count() - anunturiWithMultiplePhotos.Count()).ToString());

            var photoCount = 0;
            foreach (var anunt in anunturiWithMultiplePhotos)
            {
                //First Photo will not be deleted
                photoCount += anunt.NumarPoze - 1;

                log.DebugFormat("Attempt to remove {0} secondary photos for not active anunt with id {1}, titlu {2}",
                    (anunt.NumarPoze - 1).ToString(), anunt.Id, anunt.Title);
                ClearPhotosExceptFirst(anunt.Id);
            }

            result = new Tuple<int, int>(anunturiWithMultiplePhotos.Count(), photoCount);
            return result;
        }

        public Tuple<int, int> GetNumberDeletePozeForAnunturiOlderThanDate(DateTime dateTime)
        {
            Tuple<int, int> result;
            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile;
            lastAddedImobils = query.Where(x => x.StareAprobare != StareAprobare.Aprobat && x.Poze != null && x.DataAdaugare < dateTime).ToList();
            //var lastAddedImobilsTota; = query.Where(x => x.StareAprobare == StareAprobare.Expirat && x.Poze != null).ToList();

            var anunturiWithMultiplePhotos = lastAddedImobils.Where(x => x.NumarPoze > 1).ToList();
            //var anunturiWithMultiplePhotosTotal = lastAddedImobils2.Where(x => x.NumarPoze > 1).ToList();

            var photoCount = 0;
            foreach (var anunt in anunturiWithMultiplePhotos)
            {
                //First Photo will not be deleted
                photoCount += anunt.NumarPoze - 1;
            }

            result = new Tuple<int, int>(anunturiWithMultiplePhotos.Count(), photoCount);
            return result;
        }

        public int GetDbSize()
        {
            //https://stackoverflow.com/questions/176379/determine-sql-server-database-size
            //https://stackoverflow.com/questions/12031393/how-to-know-the-physical-size-of-the-database-in-entity-framework
            //var sqlConn = this.DbContext.Database.Connection as SqlConnection;

            //var cmd = new SqlCommand("sp_helpdb")
            //{
            //    CommandType = CommandType.StoredProcedure,
            //    Connection = sqlConn
            //};

            //cmd.Parameters.Add(new SqlParameter()
            //{
            //    ParameterName = "dbname",
            //    Value = "ImobiliareDb"
            //});

            //var adp = new SqlDataAdapter(cmd);
            //var dataset = new DataSet();
            //sqlConn.Open();
            //adp.Fill(dataset);
            //sqlConn.Close();

            //var l = dataset.Tables[1].Rows[0][4].ToString();
            //var mb = int.Parse(l.Split(' ')[0]) / 1024;

            //return mb;
            return 1;
        }

        public int DeleteAnunturiVechiBulk(DateTime dateTime)
        {
            int result;
            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile;
            lastAddedImobils = query.Where(x => x.StareAprobare != StareAprobare.Aprobat && x.DataAdaugare < dateTime).ToList();
            
            foreach(var item in lastAddedImobils)
            {
                log.DebugFormat($"Maintenance... Attempt to remove expired old anunt with id {item.Id} and title {item.Title}");
                DeleteImobil(item.Id);
            }

            result = lastAddedImobils.Count;
            return result;
        }

        public int GetNumberDeleteAnunturiVechiBulk(DateTime dateTime)
        {
            int result;
            var lastAddedImobils = new List<Imobil>();

            var query = this.DbContext.Imobile;
            lastAddedImobils = query.Where(x => x.StareAprobare != StareAprobare.Aprobat && x.DataAdaugare < dateTime).ToList();

            result = lastAddedImobils.Count;
            return result;
        }
    }
}