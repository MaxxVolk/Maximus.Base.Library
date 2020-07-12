using System;

namespace Maximus.Library.SCOMId
{
  public class WindowsId
  {
    #region Microsoft.Windows.Computer
    /// <summary>
    /// Windows Computer (Microsoft.Windows.Computer)
    /// </summary>
    public static Guid ComputerClassId { get; } = new Guid("ea99500d-8d52-fc52-b5a5-10dcd1e9d2bd");
    public static class ComputerClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.Computer/PrincipalName
      /// </summary>
      public static Guid PrincipalNamePropertyId { get; } = new Guid("5c324096-d928-76db-e9e7-e629dcc261b1");
      /// <summary>
      /// Microsoft.Windows.Computer/DNSName
      /// </summary>
      public static Guid DNSNamePropertyId { get; } = new Guid("d3b003ee-7bb7-6456-2d45-c2e764df37a6");
      /// <summary>
      /// Microsoft.Windows.Computer/NetbiosComputerName
      /// </summary>
      public static Guid NetbiosComputerNamePropertyId { get; } = new Guid("879a4dee-444f-186b-cc6f-2e9cb89d87c3");
      /// <summary>
      /// Microsoft.Windows.Computer/NetbiosDomainName
      /// </summary>
      public static Guid NetbiosDomainNamePropertyId { get; } = new Guid("756493d6-f2a2-4f35-8970-953d7c828cd0");
      /// <summary>
      /// Microsoft.Windows.Computer/IPAddress
      /// </summary>
      public static Guid IPAddressPropertyId { get; } = new Guid("f97e7c31-f713-e2a6-0450-239a62363651");
      /// <summary>
      /// Microsoft.Windows.Computer/NetworkName
      /// </summary>
      public static Guid NetworkNamePropertyId { get; } = new Guid("8f538d63-86da-c149-3c84-1f1aace2f930");
      /// <summary>
      /// Microsoft.Windows.Computer/ActiveDirectoryObjectSid
      /// </summary>
      public static Guid ActiveDirectoryObjectSidPropertyId { get; } = new Guid("488183e9-d2bb-2dd8-b872-18a20a1c9fa6");
      /// <summary>
      /// Microsoft.Windows.Computer/IsVirtualMachine
      /// </summary>
      public static Guid IsVirtualMachinePropertyId { get; } = new Guid("6832a546-b39d-aff8-94e1-660bbd336687");
      /// <summary>
      /// Microsoft.Windows.Computer/DomainDnsName
      /// </summary>
      public static Guid DomainDnsNamePropertyId { get; } = new Guid("9f2cafc8-7a18-1c76-b709-d51b01d3b3d3");
      /// <summary>
      /// Microsoft.Windows.Computer/OrganizationalUnit
      /// </summary>
      public static Guid OrganizationalUnitPropertyId { get; } = new Guid("bfdf9a0d-3393-a720-f24a-17ea701eb77a");
      /// <summary>
      /// Microsoft.Windows.Computer/ForestDnsName
      /// </summary>
      public static Guid ForestDnsNamePropertyId { get; } = new Guid("1704a09d-8063-021a-9b80-2c47622ecf0f");
      /// <summary>
      /// Microsoft.Windows.Computer/ActiveDirectorySite
      /// </summary>
      public static Guid ActiveDirectorySitePropertyId { get; } = new Guid("4a1e1c69-c659-af91-e7a8-d886008032f3");
      /// <summary>
      /// Microsoft.Windows.Computer/LogicalProcessors
      /// </summary>
      public static Guid LogicalProcessorsPropertyId { get; } = new Guid("02563965-6649-f495-aeb7-bd8a6eab33a7");
      /// <summary>
      /// Microsoft.Windows.Computer/PhysicalProcessors
      /// </summary>
      public static Guid PhysicalProcessorsPropertyId { get; } = new Guid("acf577c1-7ada-b105-ca80-1a6347a2ccb7");
      /// <summary>
      /// Microsoft.Windows.Computer/HostServerName
      /// </summary>
      public static Guid HostServerNamePropertyId { get; } = new Guid("8613d413-99ca-ef86-7636-46ac10db6478");
      /// <summary>
      /// Microsoft.Windows.Computer/VirtualMachineName
      /// </summary>
      public static Guid VirtualMachineNamePropertyId { get; } = new Guid("6664d8ac-53c8-4e5c-72a9-349051837330");
      /// <summary>
      /// Microsoft.Windows.Computer/OffsetInMinuteFromGreenwichTime
      /// </summary>
      public static Guid OffsetInMinuteFromGreenwichTimePropertyId { get; } = new Guid("68f597b6-aada-7baa-b544-fe67c737be32");
      /// <summary>
      /// Microsoft.Windows.Computer/LastInventoryDate
      /// </summary>
      public static Guid LastInventoryDatePropertyId { get; } = new Guid("6223ef72-50de-b1fb-d666-397cefd264ea");
    }
    #endregion

    #region Microsoft.Windows.Server.Computer
    /// <summary>
    /// Windows Server (Microsoft.Windows.Server.Computer)
    /// </summary>
    public static Guid ServerComputerClassId { get; } = new Guid("e817d034-02e8-294c-3509-01ca25481689");
    public static class ServerComputerClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.Server.Computer/IsVirtualNode
      /// </summary>
      public static Guid IsVirtualNodePropertyId { get; } = new Guid("d1d39d5c-e934-7d7f-4f38-58d5ef4ae1e2");
    }
    #endregion

    #region Microsoft.Windows.Client.Computer
    /// <summary>
    /// Windows Client (Microsoft.Windows.Client.Computer)
    /// </summary>
    public static Guid ClientComputerClassId { get; } = new Guid("5918e198-2dbe-5f7d-135e-6e43a94ccb5b");
    #endregion

    #region Microsoft.Windows.LogicalDevice
    /// <summary>
    /// Windows Logical Hardware Component (Microsoft.Windows.LogicalDevice)
    /// </summary>
    public static Guid LogicalDeviceClassId { get; } = new Guid("3361982a-7dfd-288d-f48c-e04e85149750");
    public static class LogicalDeviceClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.LogicalDevice/DeviceID
      /// </summary>
      public static Guid DeviceIDPropertyId { get; } = new Guid("af13c36e-9197-95f7-393c-84aa6638fec9");
      /// <summary>
      /// Microsoft.Windows.LogicalDevice/Name
      /// </summary>
      public static Guid NamePropertyId { get; } = new Guid("e0b1c8be-c004-f892-3528-d9321d185910");
      /// <summary>
      /// Microsoft.Windows.LogicalDevice/Description
      /// </summary>
      public static Guid DescriptionPropertyId { get; } = new Guid("5d0624b0-2899-8948-924d-3e4cb24c21f5");
    }
    #endregion

    #region Microsoft.Windows.LogicalDisk
    /// <summary>
    /// Logical Disk (abstract) (Microsoft.Windows.LogicalDisk)
    /// </summary>
    public static Guid LogicalDiskClassId { get; } = new Guid("d0718456-aa7e-7b92-8a46-6a2d762d32a7");
    public static class LogicalDiskClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.LogicalDisk/VolumeName
      /// </summary>
      public static Guid VolumeNamePropertyId { get; } = new Guid("654302a3-8566-e4e9-e856-f8cae47741b2");
    }
    #endregion

    #region Microsoft.Windows.PhysicalDisk
    /// <summary>
    /// Physical Disk (abstract) (Microsoft.Windows.PhysicalDisk)
    /// </summary>
    public static Guid PhysicalDiskClassId { get; } = new Guid("26d62cff-bd8f-bc5a-74df-cbf19e0367d3");
    public static class PhysicalDiskClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.PhysicalDisk/MediaType
      /// </summary>
      public static Guid MediaTypePropertyId { get; } = new Guid("9147b06e-e3c3-12f8-0c8b-ef1f2c04cd15");
      /// <summary>
      /// Microsoft.Windows.PhysicalDisk/PNPDeviceId
      /// </summary>
      public static Guid PNPDeviceIdPropertyId { get; } = new Guid("b25559f9-16e9-551c-ebc7-efd0b8c2446f");
    }
    #endregion

    #region Microsoft.Windows.DiskPartition
    /// <summary>
    /// Disk Partition (Microsoft.Windows.DiskPartition)
    /// </summary>
    public static Guid DiskPartitionClassId { get; } = new Guid("83a0ee32-c6cd-3daf-ebc8-958ea15fd764");
    public static class DiskPartitionClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.DiskPartition/Type
      /// </summary>
      public static Guid TypePropertyId { get; } = new Guid("69fabfcd-d579-2be1-e786-c80b219c4be5");
    }
    #endregion

    #region Microsoft.Windows.LocalApplication
    /// <summary>
    /// Windows Local Application (Microsoft.Windows.LocalApplication)
    /// </summary>
    public static Guid LocalApplicationClassId { get; } = new Guid("0d026768-86be-77f5-b58e-7f07d3178cf8");
    #endregion

    #region Microsoft.Windows.ApplicationComponent
    /// <summary>
    /// Windows Application Component (Microsoft.Windows.ApplicationComponent)
    /// </summary>
    public static Guid ApplicationComponentClassId { get; } = new Guid("fb00bb78-d29c-5508-5707-bf8bafb2127b");
    #endregion

    #region Microsoft.Windows.OperatingSystem
    /// <summary>
    /// Windows Operating System (Microsoft.Windows.OperatingSystem)
    /// </summary>
    public static Guid OperatingSystemClassId { get; } = new Guid("66dd9b43-3dc1-3831-95d4-1b03b0a6ea13");
    public static class OperatingSystemClassProperties
    {
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/OSVersion
      /// </summary>
      public static Guid OSVersionPropertyId { get; } = new Guid("a90b3983-32ea-70d8-d41c-0f5a3957639a");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/OSVersionDisplayName
      /// </summary>
      public static Guid OSVersionDisplayNamePropertyId { get; } = new Guid("0d53d0cb-b62a-7177-e9ef-3543f94784fd");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/ProductType
      /// </summary>
      public static Guid ProductTypePropertyId { get; } = new Guid("cbba965a-3e95-6c6e-6f40-bdea5d061df9");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/BuildNumber
      /// </summary>
      public static Guid BuildNumberPropertyId { get; } = new Guid("1919c45d-ce1e-2f56-da19-c1f19fd70f19");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/CSDVersion
      /// </summary>
      public static Guid CSDVersionPropertyId { get; } = new Guid("bf125622-640e-901c-7c53-a32391fba2a5");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/ServicePackVersion
      /// </summary>
      public static Guid ServicePackVersionPropertyId { get; } = new Guid("38242aed-43b8-1ae2-8aec-7dbb5a6ebab0");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/SerialNumber
      /// </summary>
      public static Guid SerialNumberPropertyId { get; } = new Guid("b67a5742-50a8-05d8-cc3d-260dcac27988");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/InstallDate
      /// </summary>
      public static Guid InstallDatePropertyId { get; } = new Guid("4b8f45d0-bffc-c96a-1811-6071aacfedee");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/SystemDrive
      /// </summary>
      public static Guid SystemDrivePropertyId { get; } = new Guid("ee4b8224-d7d5-dc1e-2413-415181a26822");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/WindowsDirectory
      /// </summary>
      public static Guid WindowsDirectoryPropertyId { get; } = new Guid("58cb8748-e8ef-44d7-7355-7b3da7fd686b");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/PhysicalMemory
      /// </summary>
      public static Guid PhysicalMemoryPropertyId { get; } = new Guid("d2012547-6eef-ffcf-ef83-316aeff4f079");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/LogicalProcessors
      /// </summary>
      public static Guid LogicalProcessorsPropertyId { get; } = new Guid("6521ce39-faec-1489-b49c-3d7cc441b717");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/CountryCode
      /// </summary>
      public static Guid CountryCodePropertyId { get; } = new Guid("a690ee44-53b5-7969-3e18-a5dd6a582d8b");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/Locale
      /// </summary>
      public static Guid LocalePropertyId { get; } = new Guid("79226c29-d54f-bfb3-779f-dbdf6aaf7a0c");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/Description
      /// </summary>
      public static Guid DescriptionPropertyId { get; } = new Guid("c6ec39a6-fa96-9679-ece5-f5323a7c0810");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/Manufacturer
      /// </summary>
      public static Guid ManufacturerPropertyId { get; } = new Guid("d13263fb-404a-45ad-e470-8fd7d05155bd");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/OSLanguage
      /// </summary>
      public static Guid OSLanguagePropertyId { get; } = new Guid("eac39b2c-e610-d134-3764-d052857bc8f3");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/MinorVersion
      /// </summary>
      public static Guid MinorVersionPropertyId { get; } = new Guid("a8148d6f-cdd6-9b69-377a-db35f76c82b3");
      /// <summary>
      /// Microsoft.Windows.OperatingSystem/MajorVersion
      /// </summary>
      public static Guid MajorVersionPropertyId { get; } = new Guid("764da59e-134b-b1d4-3d98-72ac5763c1fc");
    }
    #endregion

    #region Microsoft.Windows.Server.OperatingSystem
    /// <summary>
    /// Windows Server Operating System (Microsoft.Windows.Server.OperatingSystem)
    /// </summary>
    public static Guid ServerOperatingSystemClassId { get; } = new Guid("ab468fc4-467d-841e-0f5d-61c41fa54735");
    #endregion

    #region Microsoft.Windows.Client.OperatingSystem
    /// <summary>
    /// Windows Client Operating System (Microsoft.Windows.Client.OperatingSystem)
    /// </summary>
    public static Guid ClientOperatingSystemClassId { get; } = new Guid("e9377b27-f64b-b2a3-b01d-5de35b1304ac");
    #endregion

    #region Microsoft.Windows.ComputerRole
    /// <summary>
    /// Windows Computer Role (Microsoft.Windows.ComputerRole)
    /// </summary>
    public static Guid ComputerRoleClassId { get; } = new Guid("273c8793-d1c3-aaf0-cbcb-4962f532fbe5");
    #endregion
  }
}