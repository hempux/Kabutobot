using Microsoft.EntityFrameworkCore;
using net.hempux.kabuto.Ninja;
using net.hempux.kabuto.Options;
using net.hempux.ninjawebhook.Models;
using Serilog;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace net.hempux.kabuto.database
{

    public class SqliteEngine
    {


        private static NinjaApiv2 ninjaApi;
        private static TeamsBotDbContext db;
        public SqliteEngine()
        {
            if (db == null)
                db = new TeamsBotDbContext();
        }
        public void setApi(NinjaApiv2 api)
        {
            if (ninjaApi == null)
                ninjaApi = api;
        }
        public void InitDb()
        {
            if (!(File.Exists(AppOptions.SqliteDatabase)))
            {
                Log.Information("Creating sqlite file {filename}", AppOptions.SqliteDatabase);
                db.Database.Migrate();
            }
            else
            {

                if (db.Database.GetPendingMigrations().Count() > 0)
                    Log.Information("Updating current file {filename}", AppOptions.SqliteDatabase);
                else
                    Log.Information("Using sqlite file {filename}", AppOptions.SqliteDatabase);

                db.Database.Migrate();
            }



            ninjaApi = new NinjaApiv2();
        }
        public static string GetPersistentdata(string keyname)
        {
            var data = db.Persistentdata.SingleOrDefault<PersistentdataModel>(x => x.Key == keyname) ?? null;
            return data?.Value ?? string.Empty;
        }
        public static void SetPersistentdata(string name, string value)
        {
            PersistentdataModel persistentdata = new PersistentdataModel
            {
                Key = name,
                Value = value
            };

            var key = db.Persistentdata.FirstOrDefault(x => x.Key == name);

            if (key != null)
            {
                key.Value = value;
                db.Update(key);
                db.SaveChanges();
            }
            else
            {
                db.Add(persistentdata);
                db.SaveChanges();
            }


        }

        internal void Insert(DeviceModel device)
        {

            db.Update(device);

            db.SaveChanges();
        }

        internal void Insert(NodeWithDetailedReferences device)
        {

            DeviceModel deviceModel = (DeviceModel)device;
            db.Update(deviceModel);
            db.SaveChanges();
        }

        public void Insert(OauthModel oauth)
        {

            db.Update(oauth);
            db.SaveChanges();
        }
        public void Insert(OrganizationModel organization)
        {
            db.Add(organization);
            db.SaveChanges();
        }

        public void Savetoken(OauthModel token)
        {
            var key = db.Oauth.Find(1);

            if (key == null)
                db.Oauth.Add(token);
            else
                db.Oauth.Update(token);

            db.SaveChanges();
        }
        public OauthModel Storedtoken()
        {

            var token = db.Oauth.Find(1);

            return token;
        }
        public DeviceModel[] GetDevicesByOrgId(int OrgId)
        {


            var devices = db.Devices.Where(device => device.OrganizationId == OrgId);
            return devices.ToArray();



        }
        public async Task<OrganizationModel> GetOrganizationById(int organizationId)
        {


            OrganizationModel organization;
            organization = db.Organizations.SingleOrDefault(m => m.Id == organizationId);
            if (organization == null)
            {
                try
                {
                    organization = await ninjaApi.NinjaFetchAsync<Organization>(NinjaApiEndpoint.getOrganization, organizationId);


                    var orgDevices = db.Devices.Where(m => m.DeviceModelId == organizationId);


                    organization.Devices = orgDevices.ToList();

                    db.Organizations.Add(organization);
                    db.SaveChanges();

                }
                catch (System.Exception)
                {
                    throw;
                }


            }
            else
            {

                organization.Devices = GetDevicesByOrgId(organization.Id).ToList();
                db.Organizations.Update(organization);
                db.SaveChanges();
            }
            return organization;


        }
        public async Task<DeviceModel> GetDeviceById(int id)
        {
            DeviceModel devicedetails = db.Devices.SingleOrDefault(m => m.DeviceModelId == id);
            if (devicedetails == null)
            {

                try
                {
                    devicedetails = await ninjaApi.NinjaFetchAsync<NodeWithDetailedReferences>(NinjaApiEndpoint.getDevice, id);
                }
                catch (System.Exception ex)
                {
                    Log.Error(ex.Message);
                    throw;
                }


                if (devicedetails == null)
                    return null;


                OrganizationModel organization = await GetOrganizationById(devicedetails.OrganizationId);

                devicedetails.Organization = organization;


                db.Devices.Add(devicedetails);
                db.SaveChanges();

                return devicedetails;

            }
            if (devicedetails.Organization == null)
            {
                OrganizationModel organization = await GetOrganizationById(devicedetails.OrganizationId);


                devicedetails.Organization = organization;
                db.Devices.Update(devicedetails);
                db.SaveChanges();
            }
            return devicedetails;
        }



        internal async Task<OrganizationModel> GetOrganizationByDeviceIdAsync(int deviceId)
        {
            var device = await GetDeviceById(deviceId);
            return await GetOrganizationById(device.OrganizationId);

        }
    }
}