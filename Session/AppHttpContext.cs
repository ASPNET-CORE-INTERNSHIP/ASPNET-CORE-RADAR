using Microsoft.AspNetCore.Http;
using System;
using System.Threading;

namespace ASPNETAOP.Session
{
    // Used for accessing to the current HttpContext outside of controllers
    public static class AppHttpContext
    {
        static IServiceProvider services = null;

        // Provides static access to the framework's services provider
        public static IServiceProvider Services
        {
            get { return services; }
            set
            {
                if (services != null)
                {
                    throw new Exception("Can't set once a value has already been set.");
                }
                services = value;
            }
        }

        // Provides static access to the current HttpContext
        public static HttpContext Current
        {
            get
            {
                IHttpContextAccessor httpContextAccessor = services.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
                return httpContextAccessor?.HttpContext;
            }
        }

    }
}
