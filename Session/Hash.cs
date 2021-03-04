using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Session
{
    public static class Hash
    {
        //Function to convert given String to long format and choose a spesific part of the final version
        //Used to make the SessionId smaller
        public static long CurrentHashed(String normalSession)
        {
            normalSession = normalSession.Substring(0, normalSession.IndexOf('-'));

            String hashedSession = "";

            foreach (char c in normalSession)
            {
                int unicode = c;
                hashedSession += unicode;
            }

            hashedSession = hashedSession.Substring(3, 7);

            long hashedSessionLong = long.Parse(hashedSession);

            return hashedSessionLong;
        }
    }
}
