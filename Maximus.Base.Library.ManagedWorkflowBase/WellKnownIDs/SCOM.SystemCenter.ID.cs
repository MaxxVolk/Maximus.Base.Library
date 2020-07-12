using System;

namespace Maximus.Library.SCOMId
{
  public class SystemCenterId
  {
    #region Microsoft.SystemCenter.ManagementActionPoint
    /// <summary>
    ///  (Microsoft.SystemCenter.ManagementActionPoint)
    /// </summary>
    public static Guid ManagementActionPointClassId { get; } = new Guid("414bd649-ccf2-26a7-4171-e20694c802a4");
    #endregion

    #region Microsoft.SystemCenter.ManagementService
    /// <summary>
    ///  (Microsoft.SystemCenter.ManagementService)
    /// </summary>
    public static Guid ManagementServiceClassId { get; } = new Guid("d95d497c-25ec-9213-200c-50506912dad3");

    public static class ManagementServiceClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.ManagementService/HealthServiceId
      /// </summary>
      public static Guid HealthServiceIdPropertyId { get; } = new Guid("cac04922-bb71-8a38-2a97-d4f9f51d46c5");
    }
    #endregion

    #region Microsoft.SystemCenter.HealthService
    /// <summary>
    /// Health Service (Microsoft.SystemCenter.HealthService)
    /// </summary>
    public static Guid HealthServiceClassId { get; } = new Guid("ab4c891f-3359-3fb6-0704-075fbfe36710");
    public static class HealthServiceClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/AuthenticationName
      /// </summary>
      public static Guid AuthenticationNamePropertyId { get; } = new Guid("b94cce78-5ad7-5056-8019-99e6024fcb86");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/MaximumQueueSize
      /// </summary>
      public static Guid MaximumQueueSizePropertyId { get; } = new Guid("c05be2de-7d4b-24e3-cc13-66ced5bec11d");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/MaximumSizeOfAllTransferredFiles
      /// </summary>
      public static Guid MaximumSizeOfAllTransferredFilesPropertyId { get; } = new Guid("fcf69788-9b67-f7f1-a2a1-17de6e3eed75");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/RequestCompression
      /// </summary>
      public static Guid RequestCompressionPropertyId { get; } = new Guid("f338297f-df7d-ad7b-c666-168643fdfba2");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/CreateListener
      /// </summary>
      public static Guid CreateListenerPropertyId { get; } = new Guid("e50bfb16-71a7-3d09-cc65-2e3f83f7aa32");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/Port
      /// </summary>
      public static Guid PortPropertyId { get; } = new Guid("c51b7833-da54-1653-d8fd-1a738ef8ded5");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/IsRHS
      /// </summary>
      public static Guid IsRHSPropertyId { get; } = new Guid("09be58d9-1a3d-b62b-d68f-2963589342bf");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/IsManagementServer
      /// </summary>
      public static Guid IsManagementServerPropertyId { get; } = new Guid("7cb1c2b7-9d40-0ded-4af8-f56d202c65d3");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/IsAgent
      /// </summary>
      public static Guid IsAgentPropertyId { get; } = new Guid("38ca4e6a-8c10-2da3-1ccb-927b610ad4e3");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/IsGateway
      /// </summary>
      public static Guid IsGatewayPropertyId { get; } = new Guid("9f792754-17bc-f596-1f63-c59bd6c064ac");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/IsManuallyInstalled
      /// </summary>
      public static Guid IsManuallyInstalledPropertyId { get; } = new Guid("dd9a0cc5-99c4-2ee6-9602-80c4a441324f");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/InstalledBy
      /// </summary>
      public static Guid InstalledByPropertyId { get; } = new Guid("84c3b4b2-1c46-8de4-e460-ace739b9f479");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/InstallTime
      /// </summary>
      public static Guid InstallTimePropertyId { get; } = new Guid("f3b142c9-c624-763f-1d26-29e9ae91b850");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/Version
      /// </summary>
      public static Guid VersionPropertyId { get; } = new Guid("10dca198-ca6d-e6cf-5c8b-a799b624173b");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/ActionAccountIdentity
      /// </summary>
      public static Guid ActionAccountIdentityPropertyId { get; } = new Guid("45eeedab-8d7e-efb2-21bf-b1d0b3cd106c");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/HeartbeatEnabled
      /// </summary>
      public static Guid HeartbeatEnabledPropertyId { get; } = new Guid("a70ea599-8675-ca64-d8fa-2b3a6f0b244e");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/HeartbeatInterval
      /// </summary>
      public static Guid HeartbeatIntervalPropertyId { get; } = new Guid("c0a1371f-8250-5215-f43b-bfd509cb9719");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/ActiveDirectoryManaged
      /// </summary>
      public static Guid ActiveDirectoryManagedPropertyId { get; } = new Guid("7274017a-d5ef-e641-3c0e-a4e150cabea0");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/ProxyingEnabled
      /// </summary>
      public static Guid ProxyingEnabledPropertyId { get; } = new Guid("2460b133-b9b5-e834-1302-bdea539b2f71");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/PatchList
      /// </summary>
      public static Guid PatchListPropertyId { get; } = new Guid("189f0d48-2a0c-699b-f6f1-7a62cf991848");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/Protocol
      /// </summary>
      public static Guid ProtocolPropertyId { get; } = new Guid("e0d66bdd-0e6a-ec67-60bf-e3932231d1c3");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/InitiatesConnectionToParent
      /// </summary>
      public static Guid InitiatesConnectionToParentPropertyId { get; } = new Guid("314db88f-7bce-f0a4-0a52-4289e41cecbe");
      /// <summary>
      /// Microsoft.SystemCenter.HealthService/ThirdPartyAuthenticationUri
      /// </summary>
      public static Guid ThirdPartyAuthenticationUriPropertyId { get; } = new Guid("efc39f1f-9a3b-44a0-0ec9-1d16acd5bac3");
    }
    #endregion

    #region Microsoft.SystemCenter.Agent
    /// <summary>
    /// Agent (Microsoft.SystemCenter.Agent)
    /// </summary>
    public static Guid AgentClassId { get; } = new Guid("118c0f18-7a70-5f57-3a9b-eeaf1705b3fc");
    #endregion

    #region Microsoft.SystemCenter.HealthServiceWatcher
    /// <summary>
    /// Health Service Watcher (Microsoft.SystemCenter.HealthServiceWatcher)
    /// </summary>
    public static Guid HealthServiceWatcherClassId { get; } = new Guid("a4899740-ef2f-1541-6c1d-51d34b739491");
    public static class HealthServiceWatcherClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceWatcher/HealthServiceId
      /// </summary>
      public static Guid HealthServiceIdPropertyId { get; } = new Guid("e356df80-5d09-cd62-f29d-e9106f730db6");
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceWatcher/HealthServiceName
      /// </summary>
      public static Guid HealthServiceNamePropertyId { get; } = new Guid("50c2ad1b-cd40-d992-baa5-312c02dc5782");
    }
    #endregion

    #region Microsoft.SystemCenter.HealthServiceWatchersGroup
    /// <summary>
    /// Health Service Watcher Group (Microsoft.SystemCenter.HealthServiceWatchersGroup)
    /// </summary>
    public static Guid HealthServiceWatchersGroupClassId { get; } = new Guid("a4c8b023-4cfc-52cc-30b7-6c1b959c4723");
    public static class HealthServiceWatchersGroupClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceWatchersGroup/WatcherGroupName
      /// </summary>
      public static Guid WatcherGroupNamePropertyId { get; } = new Guid("87177203-9053-76a1-a322-1226f815507a");
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceWatchersGroup/RMSPrincipalName
      /// </summary>
      public static Guid RMSPrincipalNamePropertyId { get; } = new Guid("89ef0a95-2e5b-ee5f-96c2-71cce8490758");
    }
    #endregion

    #region Microsoft.SystemCenter.CollectionManagementServer
    /// <summary>
    /// Collection Server (Microsoft.SystemCenter.CollectionManagementServer)
    /// </summary>
    public static Guid CollectionManagementServerClassId { get; } = new Guid("7b11b1b9-3462-c54f-9f10-943cabb9d26e");
    #endregion

    #region Microsoft.SystemCenter.Connector
    /// <summary>
    /// Connector (Microsoft.SystemCenter.Connector)
    /// </summary>
    public static Guid ConnectorClassId { get; } = new Guid("2d63134b-491f-ae0b-d71b-81ff238422bf");
    public static class ConnectorClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.Connector/Id
      /// </summary>
      public static Guid IdPropertyId { get; } = new Guid("29906075-e9fc-7bc4-487b-f964f91d6532");
      /// <summary>
      /// Microsoft.SystemCenter.Connector/Name
      /// </summary>
      public static Guid NamePropertyId { get; } = new Guid("fd39fd0e-a9df-9509-33b9-ba0f2d9bd215");
      /// <summary>
      /// Microsoft.SystemCenter.Connector/Description
      /// </summary>
      public static Guid DescriptionPropertyId { get; } = new Guid("0a6e2ad0-d84a-9641-58e3-abba4511bb79");
      /// <summary>
      /// Microsoft.SystemCenter.Connector/DiscoveryDataIsManaged
      /// </summary>
      public static Guid DiscoveryDataIsManagedPropertyId { get; } = new Guid("89b6c2dd-4f07-0f86-a18d-72401ea57f32");
      /// <summary>
      /// Microsoft.SystemCenter.Connector/DiscoveryDataIsShared
      /// </summary>
      public static Guid DiscoveryDataIsSharedPropertyId { get; } = new Guid("802345ee-2e7b-af9e-469f-612945ce3b81");
      /// <summary>
      /// Microsoft.SystemCenter.Connector/Incoming
      /// </summary>
      public static Guid IncomingPropertyId { get; } = new Guid("d3e1b258-3d8a-b7f4-a7a3-d4499da44734");
      /// <summary>
      /// Microsoft.SystemCenter.Connector/Outgoing
      /// </summary>
      public static Guid OutgoingPropertyId { get; } = new Guid("02e47678-7056-068b-244e-126788f70b7b");
    }
    #endregion

    #region Microsoft.SystemCenter.GatewayManagementServer
    /// <summary>
    /// Gateway (Microsoft.SystemCenter.GatewayManagementServer)
    /// </summary>
    public static Guid GatewayManagementServerClassId { get; } = new Guid("c1721bcc-35f7-5a49-5d5f-6880687c3d48");
    #endregion

    #region Microsoft.SystemCenter.ManagementGroup
    /// <summary>
    /// Operations Manager Management Group (Microsoft.SystemCenter.ManagementGroup)
    /// </summary>
    public static Guid ManagementGroupClassId { get; } = new Guid("6b1d1be8-ebb4-b425-08dc-2385c5930b04");
    public static class ManagementGroupClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/Name
      /// </summary>
      public static Guid NamePropertyId { get; } = new Guid("46b79996-9e04-464d-474d-9a9fb8c2cd5d");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/Id
      /// </summary>
      public static Guid IdPropertyId { get; } = new Guid("df0ca9f1-7ca7-e177-716a-4bc404919411");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/SDKServiceSCP
      /// </summary>
      public static Guid SDKServiceSCPPropertyId { get; } = new Guid("9fcb7696-eaac-cf0d-1714-33788290e437");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/RootHealthServiceSCP
      /// </summary>
      public static Guid RootHealthServiceSCPPropertyId { get; } = new Guid("2e759952-23f9-3838-b3b1-639902c39469");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/CEIPEnabled
      /// </summary>
      public static Guid CEIPEnabledPropertyId { get; } = new Guid("b7222ae9-3490-015a-db7e-aebb490aca03");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/CommunicationPort
      /// </summary>
      public static Guid CommunicationPortPropertyId { get; } = new Guid("a09f8605-8ead-9dd3-5e6a-7560ae7b41b1");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/ErrorReportingEnabled
      /// </summary>
      public static Guid ErrorReportingEnabledPropertyId { get; } = new Guid("d5243a87-9016-38ec-02d7-1a39a3f792eb");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/ErrorReportingQueuingEnabled
      /// </summary>
      public static Guid ErrorReportingQueuingEnabledPropertyId { get; } = new Guid("4be130e0-6273-97c9-43bd-87e75eded95c");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/OnlineProductKnowledgeUrl
      /// </summary>
      public static Guid OnlineProductKnowledgeUrlPropertyId { get; } = new Guid("c7d5ca84-abe2-f8b3-d6cb-29ac7c3111fc");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/SendOperationalDataReports
      /// </summary>
      public static Guid SendOperationalDataReportsPropertyId { get; } = new Guid("a3fcb97d-3e4a-752a-3112-43f2eaf5c2a1");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/SDKServerName
      /// </summary>
      public static Guid SDKServerNamePropertyId { get; } = new Guid("94642b9c-a7f1-b192-273b-5c0f695e9f1e");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/OperationalDatabaseName
      /// </summary>
      public static Guid OperationalDatabaseNamePropertyId { get; } = new Guid("1e47d32a-c6b5-1caf-41e3-c7c8cabcb60d");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/SQLServerName
      /// </summary>
      public static Guid SQLServerNamePropertyId { get; } = new Guid("4f6bf68f-122a-c3fc-1fec-67dbf20b4747");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/AlertAutoResolvePeriod
      /// </summary>
      public static Guid AlertAutoResolvePeriodPropertyId { get; } = new Guid("6a98a953-74ea-7182-a386-e2ef4f9e7ce1");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/HealthyAlertAutoResolvePeriod
      /// </summary>
      public static Guid HealthyAlertAutoResolvePeriodPropertyId { get; } = new Guid("28581f22-e563-8222-ed64-e3b9309c833b");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementGroup/TierTimeDifferenceThreshold
      /// </summary>
      public static Guid TierTimeDifferenceThresholdPropertyId { get; } = new Guid("23674d44-17dd-48bf-b137-dd00f76ef3d7");
    }
    #endregion

    #region Microsoft.SystemCenter.ManagementServer
    /// <summary>
    /// Management Server (Microsoft.SystemCenter.ManagementServer)
    /// </summary>
    public static Guid ManagementServerClassId { get; } = new Guid("9189a49e-b2de-cab0-2e4f-4925b68e335d");
    public static class ManagementServerClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/ManagementServerSCP
      /// </summary>
      public static Guid ManagementServerSCPPropertyId { get; } = new Guid("fabc7a50-1a47-610b-ad63-2c1059e5b4f5");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/AutoApproveManuallyInstalledAgents
      /// </summary>
      public static Guid AutoApproveManuallyInstalledAgentsPropertyId { get; } = new Guid("49edf37d-6dde-0106-9d7d-bf9d43bf85a9");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/NumberOfMissingHeartBeatsToMarkMachineDown
      /// </summary>
      public static Guid NumberOfMissingHeartBeatsToMarkMachineDownPropertyId { get; } = new Guid("954ee5f1-2a6e-e2ca-ff3c-28059efaff32");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/ProxyAddress
      /// </summary>
      public static Guid ProxyAddressPropertyId { get; } = new Guid("6faaee0b-0050-6c15-4279-c738915a91ad");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/ProxyPort
      /// </summary>
      public static Guid ProxyPortPropertyId { get; } = new Guid("805a8adf-eb82-55cb-9afb-051661f94861");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/RejectManuallyInstalledAgents
      /// </summary>
      public static Guid RejectManuallyInstalledAgentsPropertyId { get; } = new Guid("3b584cb1-b84f-e3ee-784b-b285d8094527");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServer/UseProxyServer
      /// </summary>
      public static Guid UseProxyServerPropertyId { get; } = new Guid("4d22694e-6dd6-13ac-bb7b-0d0934f949ee");
    }
    #endregion

    #region Microsoft.SystemCenter.AllManagementServersPool
    /// <summary>
    /// All Management Servers Resource Pool (Microsoft.SystemCenter.AllManagementServersPool)
    /// </summary>
    public static Guid AllManagementServersPoolClassId { get; } = new Guid("4932d8f0-c8e2-2f4b-288e-3ed98a340b9f");
    #endregion

    #region Microsoft.SystemCenter.DataWarehouse
    /// <summary>
    /// Microsoft System Center Data Warehouse (Microsoft.SystemCenter.DataWarehouse)
    /// </summary>
    public static Guid DataWarehouseClassId { get; } = new Guid("16781f33-f72d-033c-1df4-65a2aff32ca3");
    public static class DataWarehouseClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.DataWarehouse/ReportingServerUrl
      /// </summary>
      public static Guid ReportingServerUrlPropertyId { get; } = new Guid("8033f6cd-5c04-8771-beda-ee46d3fa660c");
      /// <summary>
      /// Microsoft.SystemCenter.DataWarehouse/ReportRootFolderName
      /// </summary>
      public static Guid ReportRootFolderNamePropertyId { get; } = new Guid("6d266847-f09a-a3e3-e7a4-9413d7967ad2");
      /// <summary>
      /// Microsoft.SystemCenter.DataWarehouse/MyReportsFolderName
      /// </summary>
      public static Guid MyReportsFolderNamePropertyId { get; } = new Guid("dabe508d-eba5-75f5-872c-9f151a1e1258");
      /// <summary>
      /// Microsoft.SystemCenter.DataWarehouse/MainDatabaseServerName
      /// </summary>
      public static Guid MainDatabaseServerNamePropertyId { get; } = new Guid("ddf89d02-f634-eed5-bb5d-28d05d96b098");
      /// <summary>
      /// Microsoft.SystemCenter.DataWarehouse/MainDatabaseName
      /// </summary>
      public static Guid MainDatabaseNamePropertyId { get; } = new Guid("dea1260e-5e07-656c-33fe-ce976ec9c8af");
    }
    #endregion

    #region Microsoft.SystemCenter.ManagementServicePool
    /// <summary>
    /// Management Service Pool (Microsoft.SystemCenter.ManagementServicePool)
    /// </summary>
    public static Guid ManagementServicePoolClassId { get; } = new Guid("f5d4c6af-f7ff-57d6-011f-82713e64100d");
    public static class ManagementServicePoolClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServicePool/IsDynamic
      /// </summary>
      public static Guid IsDynamicPropertyId { get; } = new Guid("c73a06fe-7c0e-1991-e600-e849326e6e15");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServicePool/MembershipVersion
      /// </summary>
      public static Guid MembershipVersionPropertyId { get; } = new Guid("7d048a6a-fe81-f26d-ce4c-c174b0fe846a");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServicePool/UseDefaultObserver
      /// </summary>
      public static Guid UseDefaultObserverPropertyId { get; } = new Guid("f0724ef3-0044-b4a4-ea69-418a555fb207");
      /// <summary>
      /// Microsoft.SystemCenter.ManagementServicePool/Description
      /// </summary>
      public static Guid DescriptionPropertyId { get; } = new Guid("e2c4086b-2480-d4eb-db5c-7a1aafadf33e");
    }
    #endregion

    // === Relationships

    #region Microsoft.SystemCenter.HealthServiceShouldManageEntity
    /// <summary>
    /// Health Service should manage Entity (Microsoft.SystemCenter.HealthServiceShouldManageEntity)
    /// </summary>
    public static Guid HealthServiceShouldManageEntityRelationshipId { get; } = new Guid("2f71c644-e092-b80a-040b-5c81ba1ec353");
    #endregion

    #region Microsoft.SystemCenter.ManagementActionPointShouldManageEntity
    /// <summary>
    ///  (Microsoft.SystemCenter.ManagementActionPointShouldManageEntity)
    /// </summary>
    public static Guid ManagementActionPointShouldManageEntityRelationshipId { get; } = new Guid("cdb09107-2411-d9e2-d718-e574983d304d");
    #endregion

    #region Microsoft.SystemCenter.ManagementActionPointManagesEntity
    /// <summary>
    ///  (Microsoft.SystemCenter.ManagementActionPointManagesEntity)
    /// </summary>
    public static Guid ManagementActionPointManagesEntityRelationshipId { get; } = new Guid("cb72a458-d56e-3be8-950b-955b16f2f6a2");
    #endregion

    #region Microsoft.SystemCenter.HealthServiceCommunication
    /// <summary>
    /// Health Service Communication (Microsoft.SystemCenter.HealthServiceCommunication)
    /// </summary>
    public static Guid HealthServiceCommunicationRelationshipId { get; } = new Guid("37848e16-37a2-b81b-daaf-60a5a626be93");

    public static class HealthServiceCommunicationClassProperties
    {
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceCommunication/CanEstablishConnectionTo
      /// </summary>
      public static Guid CanEstablishConnectionToPropertyId { get; } = new Guid("f3f1e4c9-e9be-3c82-e4d8-9d3b7b641034");
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceCommunication/HeartbeatType
      /// </summary>
      public static Guid HeartbeatTypePropertyId { get; } = new Guid("1d854446-c633-968e-a220-53dc4b0770f2");
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceCommunication/MaxBytesPerSecondToSend
      /// </summary>
      public static Guid MaxBytesPerSecondToSendPropertyId { get; } = new Guid("7b9b1d6d-cdd3-b742-fe1e-ef0971c40761");
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceCommunication/NetworkTimeout
      /// </summary>
      public static Guid NetworkTimeoutPropertyId { get; } = new Guid("869a3a95-f21c-6556-65f0-07b0fc9d3a74");
      /// <summary>
      /// Microsoft.SystemCenter.HealthServiceCommunication/OrderOfPreference
      /// </summary>
      public static Guid OrderOfPreferencePropertyId { get; } = new Guid("18a3814f-959f-75a7-e3ae-6dc579bfe8b6");
    }
    #endregion
  }
}