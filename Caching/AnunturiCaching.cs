using System.Collections.Generic;

using Imobiliare.Entities;
using Logging;

namespace Caching
{
    public static class AnunturiCaching
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(AnunturiCaching));
        private static Dictionary<string, int> _numarAnunturiPerJudete;
        private static List<Imobil> _lastAddedAnunturi;
        private static List<Imobil> _lastAddedAnunturiCautare;

        //Home page harta
        public static Dictionary<string, int> NumarAnunturiPerJudete
        {
            get
            {
                return _numarAnunturiPerJudete;
            }
            set
            {
                log.Debug("Reinitializing AnunturiCaching.NumarAnunturiPerJudete");
                _numarAnunturiPerJudete = value;
            }
        }

        //Last added 4 anunturi, for home page and stire
        public static List<Imobil> LastAddedAnunturi
        {
            get
            {
                return _lastAddedAnunturi;
            }
            set
            {
                log.Debug("Reinitializing AnunturiCaching.LastAddedAnunturi");
                _lastAddedAnunturi = value; 
            }
        }

        //Home page
        public static List<Imobil> LastAddedAnunturiCautare
        {
            get
            {
                return _lastAddedAnunturiCautare;
            }
            set
            {
                log.Debug("Reinitializing AnunturiCaching.LastAddedAnunturiCautare");
                _lastAddedAnunturiCautare = value;
            }
        }

        public static void Reset()
        {
            log.Debug("Reset AnunturiCaching");
            NumarAnunturiPerJudete = null;
            LastAddedAnunturi = null;
            LastAddedAnunturiCautare = null;
        }
    }
}
