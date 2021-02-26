using System;

namespace ASPNETAOP.Models
{
    public class Pair
    {
        private String sessionID;
        private int requestID;
        private int userID;

        public Pair(String sessionID, int requestID)
        {
            this.sessionID = sessionID;
            this.requestID = requestID;
        }

        public Pair(String sessionID, int requestID, int userID)
        {
            this.sessionID = sessionID;
            this.requestID = requestID;
            this.userID = userID;
        }

        public String getSessionID() { return this.sessionID; }

        public int getRequestID() { return this.requestID; }

        public int getUserID() { return this.userID; }
    }
}
