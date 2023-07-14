using System;

namespace Imobiliare.Entities
{
    public class BlockedIp : Entity
    {
        public string Address { get; set; }

        public DateTime DataAdaugare { get; set; }

        public int BlockCount { get; set; }

        public string Descriere { get; set; }

        public bool TraceBlocking { get; set; }

        public bool UpdateBlockCount { get; set; }

        public bool IsRegex { get; set; }

        public bool Enabled { get; set; }
    }
}
