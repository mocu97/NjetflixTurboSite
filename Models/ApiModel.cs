using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CMStest.Models
{
    public class ApiModel
    {
        public List<CatAndMovies> ApiCategory { get; set; }
        public List<Tagg> ApiTaggs { get; set; }
    }
}