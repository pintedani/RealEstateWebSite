using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Imobiliare.UI.Models
{
    using Imobiliare.Entities;

    public class EditUserDetailsModel
    {
        public UserProfile UserProfile { get; set; }

        public HttpPostedFileBase ProfileImage { get; set; }
    }
}