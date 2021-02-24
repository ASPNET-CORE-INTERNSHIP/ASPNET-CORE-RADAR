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
            if (Models.CurrentUser.currentUser.CurrentUserInfo[0] == null) throw new UserNotLoggedInException();
        }
    }

    public class UserNotLoggedInException : Exception
    {
        public UserNotLoggedInException() {}

        public UserNotLoggedInException(String message) : base(message) {}
    }
}
