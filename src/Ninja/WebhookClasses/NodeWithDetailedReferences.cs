/*
 * NinjaRMM Webhook (API 2.0)
 *
 * Ninja RMM Public Webhook PSA documentation.
 *
 * The version of the OpenAPI document: 2.0.5-draft
 * Contact: api@ninjarmm.com
 * Generated by: https://openapi-generator.tech
 */

using net.hempux.ninjawebhook.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace net.hempux.ninjawebhook.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class NodeWithDetailedReferences : IEquatable<NodeWithDetailedReferences>
    {
        /// <summary>
        /// Node (Device) identifier
        /// </summary>
        /// <value>Node (Device) identifier</value>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        public int Id { get; set; }

        /// <summary>
        /// Parent Node identifier
        /// </summary>
        /// <value>Parent Node identifier</value>
        [DataMember(Name = "parentDeviceId", EmitDefaultValue = false)]
        public int ParentDeviceId { get; set; }

        /// <summary>
        /// Organization identifier
        /// </summary>
        /// <value>Organization identifier</value>
        [DataMember(Name = "organizationId", EmitDefaultValue = false)]
        public int OrganizationId { get; set; }

        /// <summary>
        /// Location identifier
        /// </summary>
        /// <value>Location identifier</value>
        [DataMember(Name = "locationId", EmitDefaultValue = false)]
        public int LocationId { get; set; }


        /// <summary>
        /// Node Class
        /// </summary>
        /// <value>Node Class</value>
        [TypeConverter(typeof(CustomEnumConverter<NodeClassEnum>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum NodeClassEnum
        {

            /// <summary>
            /// Enum WINDOWSSERVEREnum for WINDOWS_SERVER
            /// </summary>
            [EnumMember(Value = "WINDOWS_SERVER")]
            WINDOWSSERVEREnum = 1,

            /// <summary>
            /// Enum WINDOWSWORKSTATIONEnum for WINDOWS_WORKSTATION
            /// </summary>
            [EnumMember(Value = "WINDOWS_WORKSTATION")]
            WINDOWSWORKSTATIONEnum = 2,

            /// <summary>
            /// Enum LINUXWORKSTATIONEnum for LINUX_WORKSTATION
            /// </summary>
            [EnumMember(Value = "LINUX_WORKSTATION")]
            LINUXWORKSTATIONEnum = 3,

            /// <summary>
            /// Enum MACEnum for MAC
            /// </summary>
            [EnumMember(Value = "MAC")]
            MACEnum = 4,

            /// <summary>
            /// Enum VMWAREVMHOSTEnum for VMWARE_VM_HOST
            /// </summary>
            [EnumMember(Value = "VMWARE_VM_HOST")]
            VMWAREVMHOSTEnum = 5,

            /// <summary>
            /// Enum VMWAREVMGUESTEnum for VMWARE_VM_GUEST
            /// </summary>
            [EnumMember(Value = "VMWARE_VM_GUEST")]
            VMWAREVMGUESTEnum = 6,

            /// <summary>
            /// Enum LINUXSERVEREnum for LINUX_SERVER
            /// </summary>
            [EnumMember(Value = "LINUX_SERVER")]
            LINUXSERVEREnum = 7,

            /// <summary>
            /// Enum MACSERVEREnum for MAC_SERVER
            /// </summary>
            [EnumMember(Value = "MAC_SERVER")]
            MACSERVEREnum = 8,

            /// <summary>
            /// Enum CLOUDMONITORTARGETEnum for CLOUD_MONITOR_TARGET
            /// </summary>
            [EnumMember(Value = "CLOUD_MONITOR_TARGET")]
            CLOUDMONITORTARGETEnum = 9,

            /// <summary>
            /// Enum NMSSWITCHEnum for NMS_SWITCH
            /// </summary>
            [EnumMember(Value = "NMS_SWITCH")]
            NMSSWITCHEnum = 10,

            /// <summary>
            /// Enum NMSROUTEREnum for NMS_ROUTER
            /// </summary>
            [EnumMember(Value = "NMS_ROUTER")]
            NMSROUTEREnum = 11,

            /// <summary>
            /// Enum NMSFIREWALLEnum for NMS_FIREWALL
            /// </summary>
            [EnumMember(Value = "NMS_FIREWALL")]
            NMSFIREWALLEnum = 12,

            /// <summary>
            /// Enum NMSPRIVATENETWORKGATEWAYEnum for NMS_PRIVATE_NETWORK_GATEWAY
            /// </summary>
            [EnumMember(Value = "NMS_PRIVATE_NETWORK_GATEWAY")]
            NMSPRIVATENETWORKGATEWAYEnum = 13,

            /// <summary>
            /// Enum NMSPRINTEREnum for NMS_PRINTER
            /// </summary>
            [EnumMember(Value = "NMS_PRINTER")]
            NMSPRINTEREnum = 14,

            /// <summary>
            /// Enum NMSSCANNEREnum for NMS_SCANNER
            /// </summary>
            [EnumMember(Value = "NMS_SCANNER")]
            NMSSCANNEREnum = 15,

            /// <summary>
            /// Enum NMSDIALMANAGEREnum for NMS_DIAL_MANAGER
            /// </summary>
            [EnumMember(Value = "NMS_DIAL_MANAGER")]
            NMSDIALMANAGEREnum = 16,

            /// <summary>
            /// Enum NMSWAPEnum for NMS_WAP
            /// </summary>
            [EnumMember(Value = "NMS_WAP")]
            NMSWAPEnum = 17,

            /// <summary>
            /// Enum NMSIPSLAEnum for NMS_IPSLA
            /// </summary>
            [EnumMember(Value = "NMS_IPSLA")]
            NMSIPSLAEnum = 18,

            /// <summary>
            /// Enum NMSCOMPUTEREnum for NMS_COMPUTER
            /// </summary>
            [EnumMember(Value = "NMS_COMPUTER")]
            NMSCOMPUTEREnum = 19,

            /// <summary>
            /// Enum NMSVMHOSTEnum for NMS_VM_HOST
            /// </summary>
            [EnumMember(Value = "NMS_VM_HOST")]
            NMSVMHOSTEnum = 20,

            /// <summary>
            /// Enum NMSAPPLIANCEEnum for NMS_APPLIANCE
            /// </summary>
            [EnumMember(Value = "NMS_APPLIANCE")]
            NMSAPPLIANCEEnum = 21,

            /// <summary>
            /// Enum NMSOTHEREnum for NMS_OTHER
            /// </summary>
            [EnumMember(Value = "NMS_OTHER")]
            NMSOTHEREnum = 22,

            /// <summary>
            /// Enum NMSSERVEREnum for NMS_SERVER
            /// </summary>
            [EnumMember(Value = "NMS_SERVER")]
            NMSSERVEREnum = 23,

            /// <summary>
            /// Enum NMSPHONEEnum for NMS_PHONE
            /// </summary>
            [EnumMember(Value = "NMS_PHONE")]
            NMSPHONEEnum = 24,

            /// <summary>
            /// Enum NMSVIRTUALMACHINEEnum for NMS_VIRTUAL_MACHINE
            /// </summary>
            [EnumMember(Value = "NMS_VIRTUAL_MACHINE")]
            NMSVIRTUALMACHINEEnum = 25,

            /// <summary>
            /// Enum NMSNETWORKMANAGEMENTAGENTEnum for NMS_NETWORK_MANAGEMENT_AGENT
            /// </summary>
            [EnumMember(Value = "NMS_NETWORK_MANAGEMENT_AGENT")]
            NMSNETWORKMANAGEMENTAGENTEnum = 26
        }

        /// <summary>
        /// Node Class
        /// </summary>
        /// <value>Node Class</value>
        [DataMember(Name = "nodeClass", EmitDefaultValue = false)]
        public NodeClassEnum NodeClass { get; set; }

        /// <summary>
        /// Node Role identifier
        /// </summary>
        /// <value>Node Role identifier</value>
        [DataMember(Name = "nodeRoleId", EmitDefaultValue = false)]
        public int NodeRoleId { get; set; }

        /// <summary>
        /// Node Role policy ID based on organization Policy Mapping
        /// </summary>
        /// <value>Node Role policy ID based on organization Policy Mapping</value>
        [DataMember(Name = "rolePolicyId", EmitDefaultValue = false)]
        public int RolePolicyId { get; set; }

        /// <summary>
        /// Assigned policy ID (overrides organization policy mapping)
        /// </summary>
        /// <value>Assigned policy ID (overrides organization policy mapping)</value>
        [DataMember(Name = "policyId", EmitDefaultValue = false)]
        public int PolicyId { get; set; }


        /// <summary>
        /// Approval Status
        /// </summary>
        /// <value>Approval Status</value>
        [TypeConverter(typeof(CustomEnumConverter<ApprovalStatusEnum>))]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum ApprovalStatusEnum
        {

            /// <summary>
            /// Enum PENDINGEnum for PENDING
            /// </summary>
            [EnumMember(Value = "PENDING")]
            PENDINGEnum = 1,

            /// <summary>
            /// Enum APPROVEDEnum for APPROVED
            /// </summary>
            [EnumMember(Value = "APPROVED")]
            APPROVEDEnum = 2
        }

        /// <summary>
        /// Approval Status
        /// </summary>
        /// <value>Approval Status</value>
        [DataMember(Name = "approvalStatus", EmitDefaultValue = false)]
        public ApprovalStatusEnum ApprovalStatus { get; set; }

        /// <summary>
        /// Is Offline?
        /// </summary>
        /// <value>Is Offline?</value>
        [DataMember(Name = "offline", EmitDefaultValue = false)]
        public bool Offline { get; set; }

        /// <summary>
        /// Display Name
        /// </summary>
        /// <value>Display Name</value>
        [DataMember(Name = "displayName", EmitDefaultValue = false)]
        public string DisplayName { get; set; }

        /// <summary>
        /// System Name
        /// </summary>
        /// <value>System Name</value>
        [DataMember(Name = "systemName", EmitDefaultValue = false)]
        public string SystemName { get; set; }

        /// <summary>
        /// DNS Name
        /// </summary>
        /// <value>DNS Name</value>
        [DataMember(Name = "dnsName", EmitDefaultValue = false)]
        public string DnsName { get; set; }

        /// <summary>
        /// NETBIOS Name
        /// </summary>
        /// <value>NETBIOS Name</value>
        [DataMember(Name = "netbiosName", EmitDefaultValue = false)]
        public string NetbiosName { get; set; }

        /// <summary>
        /// Created
        /// </summary>
        /// <value>Created</value>
        [DataMember(Name = "created", EmitDefaultValue = false)]
        public long Created { get; set; }

        /// <summary>
        /// Last Contact
        /// </summary>
        /// <value>Last Contact</value>
        [DataMember(Name = "lastContact", EmitDefaultValue = false)]
        public long LastContact { get; set; }

        /// <summary>
        /// Last data submission timestamp
        /// </summary>
        /// <value>Last data submission timestamp</value>
        [DataMember(Name = "lastUpdate", EmitDefaultValue = false)]
        public long LastUpdate { get; set; }

        /// <summary>
        /// Gets or Sets UserData
        /// </summary>
        [DataMember(Name = "userData", EmitDefaultValue = false)]
        public Dictionary<string, Object> UserData { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        /// <value>Tags</value>
        [DataMember(Name = "tags", EmitDefaultValue = false)]
        public List<string> Tags { get; set; }

        /// <summary>
        /// Custom Fields
        /// </summary>
        /// <value>Custom Fields</value>
        [DataMember(Name = "fields", EmitDefaultValue = false)]
        public Dictionary<string, Object> Fields { get; set; }

        /// <summary>
        /// Gets or Sets Maintenance
        /// </summary>
        [DataMember(Name = "maintenance", EmitDefaultValue = false)]
        public Maintenance Maintenance { get; set; }

        /// <summary>
        /// Gets or Sets References
        /// </summary>
        [DataMember(Name = "references", EmitDefaultValue = false)]
        public NodeReferences References { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NodeWithDetailedReferences {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  ParentDeviceId: ").Append(ParentDeviceId).Append("\n");
            sb.Append("  OrganizationId: ").Append(OrganizationId).Append("\n");
            sb.Append("  LocationId: ").Append(LocationId).Append("\n");
            sb.Append("  NodeClass: ").Append(NodeClass).Append("\n");
            sb.Append("  NodeRoleId: ").Append(NodeRoleId).Append("\n");
            sb.Append("  RolePolicyId: ").Append(RolePolicyId).Append("\n");
            sb.Append("  PolicyId: ").Append(PolicyId).Append("\n");
            sb.Append("  ApprovalStatus: ").Append(ApprovalStatus).Append("\n");
            sb.Append("  Offline: ").Append(Offline).Append("\n");
            sb.Append("  DisplayName: ").Append(DisplayName).Append("\n");
            sb.Append("  SystemName: ").Append(SystemName).Append("\n");
            sb.Append("  DnsName: ").Append(DnsName).Append("\n");
            sb.Append("  NetbiosName: ").Append(NetbiosName).Append("\n");
            sb.Append("  Created: ").Append(Created).Append("\n");
            sb.Append("  LastContact: ").Append(LastContact).Append("\n");
            sb.Append("  LastUpdate: ").Append(LastUpdate).Append("\n");
            sb.Append("  UserData: ").Append(UserData).Append("\n");
            sb.Append("  Tags: ").Append(Tags).Append("\n");
            sb.Append("  Fields: ").Append(Fields).Append("\n");
            sb.Append("  Maintenance: ").Append(Maintenance).Append("\n");
            sb.Append("  References: ").Append(References).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(this, Newtonsoft.Json.Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((NodeWithDetailedReferences)obj);
        }

        /// <summary>
        /// Returns true if NodeWithDetailedReferences instances are equal
        /// </summary>
        /// <param name="other">Instance of NodeWithDetailedReferences to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(NodeWithDetailedReferences other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Id == other.Id ||

                    Id.Equals(other.Id)
                ) &&
                (
                    ParentDeviceId == other.ParentDeviceId ||

                    ParentDeviceId.Equals(other.ParentDeviceId)
                ) &&
                (
                    OrganizationId == other.OrganizationId ||

                    OrganizationId.Equals(other.OrganizationId)
                ) &&
                (
                    LocationId == other.LocationId ||

                    LocationId.Equals(other.LocationId)
                ) &&
                (
                    NodeClass == other.NodeClass ||

                    NodeClass.Equals(other.NodeClass)
                ) &&
                (
                    NodeRoleId == other.NodeRoleId ||

                    NodeRoleId.Equals(other.NodeRoleId)
                ) &&
                (
                    RolePolicyId == other.RolePolicyId ||

                    RolePolicyId.Equals(other.RolePolicyId)
                ) &&
                (
                    PolicyId == other.PolicyId ||

                    PolicyId.Equals(other.PolicyId)
                ) &&
                (
                    ApprovalStatus == other.ApprovalStatus ||

                    ApprovalStatus.Equals(other.ApprovalStatus)
                ) &&
                (
                    Offline == other.Offline ||

                    Offline.Equals(other.Offline)
                ) &&
                (
                    DisplayName == other.DisplayName ||
                    DisplayName != null &&
                    DisplayName.Equals(other.DisplayName)
                ) &&
                (
                    SystemName == other.SystemName ||
                    SystemName != null &&
                    SystemName.Equals(other.SystemName)
                ) &&
                (
                    DnsName == other.DnsName ||
                    DnsName != null &&
                    DnsName.Equals(other.DnsName)
                ) &&
                (
                    NetbiosName == other.NetbiosName ||
                    NetbiosName != null &&
                    NetbiosName.Equals(other.NetbiosName)
                ) &&
                (
                    Created == other.Created ||

                    Created.Equals(other.Created)
                ) &&
                (
                    LastContact == other.LastContact ||

                    LastContact.Equals(other.LastContact)
                ) &&
                (
                    LastUpdate == other.LastUpdate ||

                    LastUpdate.Equals(other.LastUpdate)
                ) &&
                (
                    UserData == other.UserData ||
                    UserData != null &&
                    other.UserData != null &&
                    UserData.SequenceEqual(other.UserData)
                ) &&
                (
                    Tags == other.Tags ||
                    Tags != null &&
                    other.Tags != null &&
                    Tags.SequenceEqual(other.Tags)
                ) &&
                (
                    Fields == other.Fields ||
                    Fields != null &&
                    other.Fields != null &&
                    Fields.SequenceEqual(other.Fields)
                ) &&
                (
                    Maintenance == other.Maintenance ||
                    Maintenance != null &&
                    Maintenance.Equals(other.Maintenance)
                ) &&
                (
                    References == other.References ||
                    References != null &&
                    References.Equals(other.References)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)

                hashCode = hashCode * 59 + Id.GetHashCode();

                hashCode = hashCode * 59 + ParentDeviceId.GetHashCode();

                hashCode = hashCode * 59 + OrganizationId.GetHashCode();

                hashCode = hashCode * 59 + LocationId.GetHashCode();

                hashCode = hashCode * 59 + NodeClass.GetHashCode();

                hashCode = hashCode * 59 + NodeRoleId.GetHashCode();

                hashCode = hashCode * 59 + RolePolicyId.GetHashCode();

                hashCode = hashCode * 59 + PolicyId.GetHashCode();

                hashCode = hashCode * 59 + ApprovalStatus.GetHashCode();

                hashCode = hashCode * 59 + Offline.GetHashCode();
                if (DisplayName != null)
                    hashCode = hashCode * 59 + DisplayName.GetHashCode();
                if (SystemName != null)
                    hashCode = hashCode * 59 + SystemName.GetHashCode();
                if (DnsName != null)
                    hashCode = hashCode * 59 + DnsName.GetHashCode();
                if (NetbiosName != null)
                    hashCode = hashCode * 59 + NetbiosName.GetHashCode();

                hashCode = hashCode * 59 + Created.GetHashCode();

                hashCode = hashCode * 59 + LastContact.GetHashCode();

                hashCode = hashCode * 59 + LastUpdate.GetHashCode();
                if (UserData != null)
                    hashCode = hashCode * 59 + UserData.GetHashCode();
                if (Tags != null)
                    hashCode = hashCode * 59 + Tags.GetHashCode();
                if (Fields != null)
                    hashCode = hashCode * 59 + Fields.GetHashCode();
                if (Maintenance != null)
                    hashCode = hashCode * 59 + Maintenance.GetHashCode();
                if (References != null)
                    hashCode = hashCode * 59 + References.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(NodeWithDetailedReferences left, NodeWithDetailedReferences right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(NodeWithDetailedReferences left, NodeWithDetailedReferences right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
