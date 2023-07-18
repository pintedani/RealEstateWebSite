using Imobiliare.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class Logger : ILog
    {
        //TODO Implement inteface!

        public void DebugFormat(string v, string poza)
        {
            
        }

        public void DebugFormat(string v, int imobilId, string? name)
        {
            
        }

        public void DebugFormat(string v, int imobilId, string? name, string imageName)
        {
            
        }

        public void ErrorFormat(string error)
        {

        }

        public void ErrorFormat(string v, string poza)
        {
            
        }

        public void ErrorFormat(string v, int anuntId, string secretNumber)
        {
            
        }

        public void ErrorFormat(string v, int anuntId, StareAprobare stareAprobare, StareAprobare stare)
        {
            
        }

        public void WarnFormat(string v, string userid)
        {
            
        }
    }
}
