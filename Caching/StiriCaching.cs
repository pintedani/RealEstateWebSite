using Imobiliare.Entities;
using System.Collections.Generic;
using Common.Logging;

namespace Caching
{
    public static class StiriCaching
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(StiriCaching));
        private static List<Stire> _stiri = new List<Stire>();
        private static List<Stire> _lastAddedStiri = new List<Stire>();

        public static List<Stire> Stiri
        {
            get 
            {
                //Although should not be null, it happen once on live that it threw exception probably because stiri was null somehow
                if (_stiri == null)
                {
                    _stiri = new List<Stire>();
                }

                return _stiri;
            }
            set
            {
                log.Debug("Reinitializing StiriCaching.Stiri");
                _stiri = value; 
            }
        }

        public static List<Stire> LastAddedStiri
        {
            get => _lastAddedStiri;
            set
            {
                log.Debug("Reinitializing StiriCaching.LastAddedStiri");
                _lastAddedStiri = value;
            }
        }

        public static void Reset()
        {
            log.Debug("Reset StiriCaching");
            Stiri = new List<Stire>();
            LastAddedStiri = new List<Stire>();
        }
    }
}
