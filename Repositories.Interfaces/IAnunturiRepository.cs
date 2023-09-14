﻿namespace Imobiliare.Repositories.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Web;

    using Imobiliare.Entities;

    /// <summary>
    /// Gets information about Imobils and provide update methods
    /// </summary>
    public interface IAnunturiRepository : IRepository<Imobil>
    {
        List<Imobil> GetAllImobil(ImobilFilter imobilFilter, out int totalNumberOfPages);

        List<Imobil> GetLastAddedImobils(int number);

        List<Imobil> GetLastAddedAnunturiCautare(int number);

        List<Imobil> GetLastAddedImobilsOnlyWithPictures(int number);

        List<Imobil> GetPromovatedImobils(ImobilFilter imobilFilter, PromovatLevel promovatedLevel);

        Imobil GetAnuntByTitle(string title);

        //Get 3 similar random anunturi
        List<Imobil> GetSimilarAnunturi(int imobilId);

        bool UpdateImobil(Imobil imobil, bool isAdmin);

        bool UpdateImobilNew(Imobil imobilDto, bool isAdmin);

        bool DeleteImobil(int imobilId);

        void DeactivareAnunt(int imobilId);

        void ReactivareAnunt(int imobilId);

        void ChangeImobilState(int imobilId, StareAprobare stareAprobare);

        void ReActualizareAnunt(int imobilId);

        Imobil AddImobil(Imobil imobil, string userName);

        void DeleteImage(int imobilId, string imageName);

        void MovePhotoUp(int imobilId, string movePozaUp);

        void MovePhotoDown(int imobilId, string movePozaDown);

        void RotatePhoto(int imobilId, string rotatePoza);

        //Returns last added image
        //string AddImages(int imobilId, IFormFile[] files);

        void RemoveGoogleMarkerCoordinates(int imobilId);

        Dictionary<string, int> GetTotalNumarAnunturiPerJudete();

        List<Imobil> CheckForExpiredAnunturi();

        void AddCustomNoteToImobil(int id, string note);

        void ClearPhotosExceptFirst(int anuntId);

        Imobil GetImobilRelatedToExternalLink(string externalLink);

        Dictionary<TipProprietate, int> GetSituationForTipOferta(ImobilFilter filter);

        Dictionary<TipOfertaGen, int> GetSituationForTipProprietate(ImobilFilter imobilFilter);

        string GetTelefonForAnunt(string anuntId);

        void IncrementNumarAccesari(int imobilId);

        Tuple<int, int> DeletePozeForExpiredAnunturiOlderThanDate(DateTime dateTime);
        Tuple<int, int> GetNumberDeletePozeForAnunturiOlderThanDate(DateTime dateTime);

        int GetDbSize();
        int DeleteAnunturiVechiBulk(DateTime dateTime);
        int GetNumberDeleteAnunturiVechiBulk(DateTime dateTime);
    }
}
