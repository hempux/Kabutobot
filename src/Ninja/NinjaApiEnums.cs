namespace net.hempux.kabuto.Ninja
{
    public static class NinjaApiEndpoint
    {
        public const string oauthCode = "/ws/oauth/authorize";
        public const string oauthToken = "/ws/oauth/token";

        public const string getOrganizations = "/v2/organizations";
        public const string getOrganization = "/v2/organization/id";

        public const string getDevice = "/v2/device/id";

        public const string pendingDevices = "/v2/devices?df=status%3DPENDING";
        public const string getPolicies = "/v2/policies";

        public const string deviceApprovalUrl = "/v2/devices/approval/APPROVE";
        public const string deviceRejectionlUrl = "/v2/devices/approval/REJECT";

    }
}
