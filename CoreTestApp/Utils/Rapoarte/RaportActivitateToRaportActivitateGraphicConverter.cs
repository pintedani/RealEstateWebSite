using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Imobiliare.Entities;
using Imobiliare.UI.Models.Rapoarte;

namespace Imobiliare.UI.Utils.Rapoarte
{
    public static class RaportActivitateToRaportActivitateGraphicConverter
    {
        public static List<RaportActivitateGraphic> GenerateRaportActivitateGraphic(List<RaportActivitate> raportActivitate)
        {
            var result = new List<RaportActivitateGraphic>();

            var totalDisctinctSessions = raportActivitate.Sum(x => x.SessionStartedDistinctCount);
            var totalAnunturiUser = raportActivitate.Sum(x => x.AnunturiByUsersCount);
            var totalAnunturiAdmin = raportActivitate.Sum(x => x.AnunturiByAdminCount);

            foreach (var activitate in raportActivitate)
            {
                var raportActivitateGraphic = new RaportActivitateGraphic();
                raportActivitateGraphic.DateTime = activitate.CreateDateTime;
                if (totalDisctinctSessions > 0 && activitate.SessionStartedDistinctCount > 0)
                {
                    raportActivitateGraphic.SesiuniDistinctePercentage = (int)Math.Round((double)(100 * activitate.SessionStartedDistinctCount) / totalDisctinctSessions);
                    raportActivitateGraphic.SessionStartedDistinctCount = activitate.SessionStartedDistinctCount;
                }

                if (totalAnunturiUser > 0 && activitate.AnunturiByUsersCount > 0)
                {
                    raportActivitateGraphic.AnunturiByUserPercentage = (int)Math.Round((double)(100 * activitate.AnunturiByUsersCount) / totalAnunturiUser);
                    raportActivitateGraphic.AnunturiByUsersCount = activitate.AnunturiByUsersCount;
                }

                if (totalAnunturiAdmin > 0 && activitate.AnunturiByAdminCount > 0)
                {
                    raportActivitateGraphic.AnunturiByAdminPercentage = (int)Math.Round((double)(100 * activitate.AnunturiByAdminCount) / totalAnunturiAdmin);
                    raportActivitateGraphic.AnunturiByAdminCount = activitate.AnunturiByAdminCount;
                }

                result.Add(raportActivitateGraphic);
            }

            return result;
        }
    }
}