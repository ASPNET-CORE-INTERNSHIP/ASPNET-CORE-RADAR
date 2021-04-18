using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class AdminLocationEdit
    {

        public String name { get; set; }
        public String newName { get; set; }

        public String country { get; set; }

        public String city { get; set; }

        public String geographic_latitude { get; set; }
        public String geographic_longitude { get; set; }

        public String airborne { get; set; }


    }
}
