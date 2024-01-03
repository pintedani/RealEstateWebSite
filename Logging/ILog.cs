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
        void Debug(string message, string memberName = "", string filePath = "", int lineNumber = 0);
        void Error(string error);
        void Info(string v);
        void Warn(string message);
    }
}
