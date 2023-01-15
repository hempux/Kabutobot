using Microsoft.EntityFrameworkCore;
using net.hempux.kabuto.Options;
using net.hempux.ninjawebhook.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using static net.hempux.ninjawebhook.Models.NodeWithDetailedReferences;
using static net.hempux.ninjawebhook.Models.Organization;

namespace net.hempux.kabuto.database
{
    public class TeamsBotDbContext : DbContext
    {


        public DbSet<OrganizationModel> Organizations { get; set; }
        public DbSet<DeviceModel> Devices { get; set; }
        public DbSet<OauthModel> Oauth { get; set; }
        public DbSet<PersistentdataModel> Persistentdata { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={AppOptions.SqliteDatabase}");

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<PersistentdataModel>()
                .HasIndex(u => u.Key)
                .IsUnique();

            builder.Entity<OrganizationModel>()
                .HasMany(o => o.Devices)
                .WithOne(d => d.Organization)
                .HasForeignKey("DeviceForeignKey");

            builder.Entity<DeviceModel>()
                .Property<int>("DeviceForeignKey");


            //builder.Entity<NinjaDev>();

        }




    }



    [Table("Persistentdata")]
    public class PersistentdataModel
    {
        [ForeignKey("OrganizationId")]
        public int Id { get; set; }

        public string Key { get; set; }
        public string Value { get; set; }
    }

    [Table("Oauth")]
    public class OauthModel
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string Refresh_token { get; set; }
        public int Expires_at { get; set; }
    }

    [Table("Organizations")]
    [DataContract]
    [JsonObject(IsReference = true)]
    public class OrganizationModel
    {
        public OrganizationModel() { }

        [DataMember]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [DataMember]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [DataMember]
        [JsonPropertyName("approvalMode")]
        public NodeApprovalModeEnum ApprovalMode { get; set; }
        [DataMember]
        public List<DeviceModel> Devices { get; set; }

        public static implicit operator OrganizationModel(Organization v) => new OrganizationModel()
        {
            Id = v.Id,
            Name = v.Name,
            ApprovalMode = v.NodeApprovalMode
        };

    }

    [Table("Devices")]
    [DataContract]
    [JsonObject(IsReference = true)]
    public class DeviceModel
    {
        public DeviceModel() { }
        [JsonPropertyName("id")]
        [DataMember]
        public int DeviceModelId { get; set; }

        [JsonPropertyName("organizationId")]
        [DataMember]
        public int OrganizationId { get; set; }

        [JsonPropertyName("systemName")]
        [DataMember]
        public string SystemName { get; set; }

        [JsonPropertyName("dnsName")]
        [DataMember]
        public string dnsName { get; set; }

        [JsonPropertyName("approvalStatus")]
        [DataMember]
        public ApprovalStatusEnum approvalStatus { get; set; }

        [DataMember]
        public OrganizationModel Organization { get; set; }

        public static implicit operator DeviceModel(NodeWithDetailedReferences v) => new DeviceModel()
        {
            DeviceModelId = v.Id,
            OrganizationId = v.OrganizationId,
            SystemName = v.SystemName,
            dnsName = v.DnsName,
            approvalStatus = v.ApprovalStatus,
        };
    }


}