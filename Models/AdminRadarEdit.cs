using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ASPNETAOP.Models
{
    public class AdminRadarEdit
    {

        public  String name { get; set; }

        public String newName { get; set; }
        public virtual String system { get; set; }

        public virtual String configuration { get; set; }


        public virtual Guid transmitter_id { get; set; }
        public virtual Guid receiver_id { get; set; }

        public virtual Guid location_id { get; set; }

    }
}
