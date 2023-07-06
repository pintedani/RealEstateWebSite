namespace CoreTestApp.Models
{
    public class RaportActivitate : Entity
    {
        public int ReceiversCount {get; set;}

        public int StiriAdaugateCount { get; set; }

        public int AnsambluriAdaugateCount { get; set; }

        public int UseriCreatiCount { get; set; }

        public int AnunturiByUsersCount { get; set; }

        public int AnunturiByAdminCount { get; set; }

        public int SessionStartedCount { get; set; }

        public int SessionStartedDistinctCount { get; set; }

        public int AuditTrailsCount { get; set; }

        public int LastConsideredHoursCount { get; set; }

        public string FinalEmail { get; set; }

        public DateTime CreateDateTime { get; set; }

        public string GenerareRaportTime { get; set; }

        public bool IsFakeEmailManager { get; set; }
    }
}
