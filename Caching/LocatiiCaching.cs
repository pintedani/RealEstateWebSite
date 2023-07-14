using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Logging;
using Imobiliare.Entities;

namespace Caching
{
    public static class LocatiiCaching
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(LocatiiCaching));
        private static List<Judet> _judete;
        private static List<Oras> _localitati;
        private static List<Cartier> _cartiere;

        public static List<Judet> Judete
        {
            get
            {
                return _judete;
            }
            set
            {
                log.Debug("Reinitializing LocatiiCaching.Judete");
                _judete = value; 
            }
        }

        public static List<Oras> Localitati
        {
            get
            {
                return _localitati;
            }
            set
            {
                log.Debug("Reinitializing LocatiiCaching.Localitati");
                _localitati = value;
            }
        }

        public static List<Cartier> Cartiere
        {
            get
            {
                return _cartiere; 

            }
            set
            {
                log.Debug("Reinitializing LocatiiCaching.Cartiere");
                _cartiere = value; 
            }
        }

        public static void Reset()
        {
            log.Debug("Reset LocatiiCaching");
            Judete = null;
            Localitati = null;
            Cartiere = null;
        }
    }
}
