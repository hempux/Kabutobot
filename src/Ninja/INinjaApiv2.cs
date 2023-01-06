using net.hempux.kabuto.database;
using System.Threading.Tasks;

namespace net.hempux.kabuto.Ninja
{
    public interface INinjaApiv2
    {
        Task<string> ApproveDevice(NinjaDeviceApproval devices);
        Task<NinjaOauthToken> CompleteOauthFlow(string code);
        Task<bool> EnsureTokenExistsAndIsValid();
        string GetAuthorizationCodeUrl();
        Task<NinjaOauthToken> GetOauthToken(string code);
        Task<T> NinjaFetchAsync<T>(string item);
        Task<T> NinjaFetchAsync<T>(string item, int id);
        void setSqliteEngine(SqliteEngine Sqliteengine);
    }
}