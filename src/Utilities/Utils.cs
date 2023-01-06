using System;

namespace net.hempux.kabuto.Utilities
{
    public static class Utils
    {
        public static bool InDocker { get { return Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"; } }

    }
}
