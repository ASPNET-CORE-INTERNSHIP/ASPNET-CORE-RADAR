using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Models
{
    public class SessionList
    {
        public static readonly SessionList listObject = new SessionList();
        
        public int count;

        public ArrayList Pair = new ArrayList();

        public SessionList()
        {

        }

    }
}
