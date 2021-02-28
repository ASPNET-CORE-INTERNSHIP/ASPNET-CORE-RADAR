using System;

namespace ASPNETAOP.Models
{
    // Used for storing the HttpContext.SessionId and the ID of the Http request sent to ASPNETAOP-WebServer of the currently active user
    public class Pair
    {
        private String sessionID;   // Stores HttpContext.SessionId
        private int requestID;      // Stores the id of the POST Request 
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
