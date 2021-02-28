using ASPNETAOP.Models;
using ASPNETAOP.Session;
using PostSharp.Aspects;
using PostSharp.Serialization;
using System;

namespace ASPNETAOP.Aspect
{
 
    [PSerializable]
    public sealed class IsAuthenticatedAttribute : OnMethodBoundaryAspect
    {
        public override void OnEntry(MethodExecutionArgs args)
        {
            String sessionID = AppHttpContext.Current.Session.Id;
            Boolean userFound = false;

            //Check if there is a session for the active sessionID
            foreach (Pair pair in SessionList.listObject.Pair)
            {
                if (sessionID.Equals(pair.getSessionID())) userFound = true;
            }

            if (!userFound) throw new UserNotLoggedInException();
        }
    }

    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() {}

        public UserNotLoggedInException(String message) : base(message) {}
    }
}
