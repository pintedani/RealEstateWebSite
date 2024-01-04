using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public interface ILog
    {
        void Debug(string message);
        void Error(string error);
        void Info(string v);
        void Warn(string message);
    }
}
