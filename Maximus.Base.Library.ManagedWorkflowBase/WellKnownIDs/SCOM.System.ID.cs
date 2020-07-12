using System;

namespace Maximus.Library.SCOMId
{
  public class SystemId
  {
    #region System.Entity
    /// <summary>
    /// Object (System.Entity)
    /// </summary>
    public static Guid EntityClassId { get; } = new Guid("55270a70-ac47-c853-c617-236b0cff9b4c");
    public static class EntityClassProperties
    {
      /// <summary>
      /// System.Entity/DisplayName
      /// </summary>
      public static Guid DisplayNamePropertyId { get; } = new Guid("afb4f9e6-bf48-1737-76ad-c9b3ec325b97");
    }
    #endregion

    #region System.ConfigItem
    /// <summary>
    /// Configuration Item (System.ConfigItem)
    /// </summary>
    public static Guid ConfigItemClassId { get; } = new Guid("62f0be9f-ecea-e73c-f00d-3dd78a7422fc");
    public static class ConfigItemClassProperties
    {
      /// <summary>
      /// System.ConfigItem/ObjectStatus
      /// </summary>
      public static Guid ObjectStatusPropertyId { get; } = new Guid("11927069-6957-dfcf-a277-08a784221325");
      /// <summary>
      /// System.ConfigItem/AssetStatus
      /// </summary>
      public static Guid AssetStatusPropertyId { get; } = new Guid("a6b7ea8d-9423-8529-cb21-87ac1a21fdcb");
      /// <summary>
      /// System.ConfigItem/Notes
      /// </summary>
      public static Guid NotesPropertyId { get; } = new Guid("55124dce-750d-bd54-4b92-d1fddf99adec");
    }
    #endregion

    #region System.Collections
    /// <summary>
    /// Collection (System.Collections)
    /// </summary>
    public static Guid CollectionsClassId { get; } = new Guid("6833e0ae-75b0-0e28-7466-98cefe548585");
    #endregion

    #region System.LogicalEntity
    /// <summary>
    /// Logical Entity (System.LogicalEntity)
    /// </summary>
    public static Guid LogicalEntityClassId { get; } = new Guid("885bc562-ed36-c548-b938-44ce5ba7ba2b");
    #endregion

    #region System.Database
    /// <summary>
    /// Database (System.Database)
    /// </summary>
    public static Guid DatabaseClassId { get; } = new Guid("36866ed8-4de1-32b8-3f01-030fd6005839");
    #endregion

    #region System.PhysicalEntity
    /// <summary>
    /// Physical Entity (System.PhysicalEntity)
    /// </summary>
    public static Guid PhysicalEntityClassId { get; } = new Guid("4f931f46-b47a-fd34-cdd6-8aa9dbd819b4");
    #endregion

    #region System.LogicalHardware
    /// <summary>
    /// Logical Hardware Component (System.LogicalHardware)
    /// </summary>
    public static Guid LogicalHardwareClassId { get; } = new Guid("d7c78218-48b6-1ed9-c7d2-b6b832ef1652");
    #endregion

    #region System.Device
    /// <summary>
    /// Device (System.Device)
    /// </summary>
    public static Guid DeviceClassId { get; } = new Guid("7ad221e0-e4bb-39a8-6514-33b60bba46f5");
    #endregion

    #region System.Computer
    /// <summary>
    /// Computer (System.Computer)
    /// </summary>
    public static Guid ComputerClassId { get; } = new Guid("b4a14ffd-52c8-064f-c936-67616c245b35");
    #endregion

    #region System.MobileDevice
    /// <summary>
    /// Mobile device (System.MobileDevice)
    /// </summary>
    public static Guid MobileDeviceClassId { get; } = new Guid("149aef0f-c094-c06a-3824-6e0732c7a04a");
    public static class MobileDeviceClassProperties
    {
      /// <summary>
      /// System.MobileDevice/Id
      /// </summary>
      public static Guid IdPropertyId { get; } = new Guid("8e22d1b5-f99f-e3d1-d332-a0c220346302");
      /// <summary>
      /// System.MobileDevice/DeviceImei
      /// </summary>
      public static Guid DeviceImeiPropertyId { get; } = new Guid("ffe59943-d4f2-d245-0b66-d7ba22a71593");
      /// <summary>
      /// System.MobileDevice/DeviceOS
      /// </summary>
      public static Guid DeviceOSPropertyId { get; } = new Guid("b2e701f0-3939-202f-cacc-ea5502c47a7d");
      /// <summary>
      /// System.MobileDevice/DevicePhoneNumber
      /// </summary>
      public static Guid DevicePhoneNumberPropertyId { get; } = new Guid("79956129-c115-e4dd-e056-ffc0084d248c");
      /// <summary>
      /// System.MobileDevice/DeviceMobileOperator
      /// </summary>
      public static Guid DeviceMobileOperatorPropertyId { get; } = new Guid("29e02638-ab60-7920-aac5-677e3b434169");
      /// <summary>
      /// System.MobileDevice/DeviceModel
      /// </summary>
      public static Guid DeviceModelPropertyId { get; } = new Guid("1559c5cc-04df-dcdf-9b07-0a2112673c2c");
      /// <summary>
      /// System.MobileDevice/ExchangeServer
      /// </summary>
      public static Guid ExchangeServerPropertyId { get; } = new Guid("56c62eca-54fb-cc87-89ad-b256941e6b29");
      /// <summary>
      /// System.MobileDevice/DeviceManufacturer
      /// </summary>
      public static Guid DeviceManufacturerPropertyId { get; } = new Guid("08fe3348-c7db-478d-6bcc-6a9842fae742");
      /// <summary>
      /// System.MobileDevice/FirmwareVersion
      /// </summary>
      public static Guid FirmwareVersionPropertyId { get; } = new Guid("079e6f75-93e3-4f25-ca39-4df46c1fc8fe");
      /// <summary>
      /// System.MobileDevice/HardwareVersion
      /// </summary>
      public static Guid HardwareVersionPropertyId { get; } = new Guid("193a08aa-854f-0c6d-4c74-7ebfd6e5fb88");
      /// <summary>
      /// System.MobileDevice/OEM
      /// </summary>
      public static Guid OEMPropertyId { get; } = new Guid("35c574c3-2a04-f99c-4d7d-7daf82665f4d");
      /// <summary>
      /// System.MobileDevice/ProcessorType
      /// </summary>
      public static Guid ProcessorTypePropertyId { get; } = new Guid("e6472620-ee26-b980-bedd-8ed709e56b3a");
      /// <summary>
      /// System.MobileDevice/HorizontalResolution
      /// </summary>
      public static Guid HorizontalResolutionPropertyId { get; } = new Guid("56d78143-d770-ea42-a24e-94943c528fa7");
      /// <summary>
      /// System.MobileDevice/VerticalResolution
      /// </summary>
      public static Guid VerticalResolutionPropertyId { get; } = new Guid("a6522909-981e-8154-9e8c-70cd992262a4");
    }
    #endregion

    #region System.NetworkDevice
    /// <summary>
    /// Network Device (System.NetworkDevice)
    /// </summary>
    public static Guid NetworkDeviceClassId { get; } = new Guid("4fbfee21-293b-d82f-8af1-da003bebabfc");
    #endregion

    #region System.Group
    /// <summary>
    /// Group (System.Group)
    /// </summary>
    public static Guid GroupClassId { get; } = new Guid("d0b32736-5344-2fcc-74b3-f72dc64ef572");
    #endregion

    #region System.User
    /// <summary>
    /// User (System.User)
    /// </summary>
    public static Guid UserClassId { get; } = new Guid("943d298f-d79a-7a29-a335-8833e582d252");
    public static class UserClassProperties
    {
      /// <summary>
      /// System.User/FirstName
      /// </summary>
      public static Guid FirstNamePropertyId { get; } = new Guid("a1dd0787-f6b9-5131-3f43-f356ca2bb163");
      /// <summary>
      /// System.User/Initials
      /// </summary>
      public static Guid InitialsPropertyId { get; } = new Guid("7b32dd73-f649-f288-f8a1-13b16481eca6");
      /// <summary>
      /// System.User/LastName
      /// </summary>
      public static Guid LastNamePropertyId { get; } = new Guid("2cde06de-0ad2-ea7a-50b0-627d0a2337df");
      /// <summary>
      /// System.User/Company
      /// </summary>
      public static Guid CompanyPropertyId { get; } = new Guid("7b3a602c-c9e3-766e-840d-a2099f5a51ad");
      /// <summary>
      /// System.User/Department
      /// </summary>
      public static Guid DepartmentPropertyId { get; } = new Guid("22405812-2d6d-e429-110c-2d2d8f9518df");
      /// <summary>
      /// System.User/Office
      /// </summary>
      public static Guid OfficePropertyId { get; } = new Guid("6021c777-23dd-c73a-3465-b97e00d7a1ba");
      /// <summary>
      /// System.User/Title
      /// </summary>
      public static Guid TitlePropertyId { get; } = new Guid("9010dfd3-8bce-eac9-6657-d19130e07bfa");
      /// <summary>
      /// System.User/EmployeeId
      /// </summary>
      public static Guid EmployeeIdPropertyId { get; } = new Guid("d56f368d-2e48-e4be-f4eb-f82744879663");
      /// <summary>
      /// System.User/StreetAddress
      /// </summary>
      public static Guid StreetAddressPropertyId { get; } = new Guid("3c923ab8-1f93-bd52-07a9-df59fd78bd1f");
      /// <summary>
      /// System.User/City
      /// </summary>
      public static Guid CityPropertyId { get; } = new Guid("42b2a214-7ccc-b734-f5a7-ee7b0bb8b371");
      /// <summary>
      /// System.User/State
      /// </summary>
      public static Guid StatePropertyId { get; } = new Guid("ed79d5a6-19ff-6c72-7204-1b7b26677f17");
      /// <summary>
      /// System.User/Zip
      /// </summary>
      public static Guid ZipPropertyId { get; } = new Guid("584266bf-a344-f323-a15b-4a761aecd867");
      /// <summary>
      /// System.User/Country
      /// </summary>
      public static Guid CountryPropertyId { get; } = new Guid("9c5cfe85-8752-d66e-c08b-419776f9c0f0");
      /// <summary>
      /// System.User/BusinessPhone
      /// </summary>
      public static Guid BusinessPhonePropertyId { get; } = new Guid("d1aded93-3cd9-35e2-d4f4-8b4d0d37409c");
      /// <summary>
      /// System.User/BusinessPhone2
      /// </summary>
      public static Guid BusinessPhone2PropertyId { get; } = new Guid("51d9d560-1c1a-058e-4307-4e5302132612");
      /// <summary>
      /// System.User/HomePhone
      /// </summary>
      public static Guid HomePhonePropertyId { get; } = new Guid("b22c0740-cdcc-4e9d-9ad2-d97b096f8330");
      /// <summary>
      /// System.User/HomePhone2
      /// </summary>
      public static Guid HomePhone2PropertyId { get; } = new Guid("1e319ed0-84e5-98d1-b4cb-e62b524d81b1");
      /// <summary>
      /// System.User/Fax
      /// </summary>
      public static Guid FaxPropertyId { get; } = new Guid("d962f04f-1429-a910-fa7b-ac8444645aee");
      /// <summary>
      /// System.User/Mobile
      /// </summary>
      public static Guid MobilePropertyId { get; } = new Guid("79a97a95-f92d-eb97-e9e9-d4b9c51e0ab5");
      /// <summary>
      /// System.User/Pager
      /// </summary>
      public static Guid PagerPropertyId { get; } = new Guid("d72232bd-85e4-3e3d-1f33-6bebf3b7606a");
    }
    #endregion

    #region System.Printer
    /// <summary>
    /// Printer (System.Printer)
    /// </summary>
    public static Guid PrinterClassId { get; } = new Guid("319e665c-8db5-c06b-fd4b-93991942e429");
    #endregion

    #region System.Perspective
    /// <summary>
    /// Perspective (System.Perspective)
    /// </summary>
    public static Guid PerspectiveClassId { get; } = new Guid("176ced55-7556-a619-717c-b59ae14e756f");
    #endregion

    #region System.OperatingSystem
    /// <summary>
    /// Operating System (System.OperatingSystem)
    /// </summary>
    public static Guid OperatingSystemClassId { get; } = new Guid("a82191f5-fe0d-cf40-8464-39e75f95db57");
    #endregion

    #region System.ComputerRole
    /// <summary>
    /// Computer Role (System.ComputerRole)
    /// </summary>
    public static Guid ComputerRoleClassId { get; } = new Guid("fb979319-fa0c-ea0e-3ac5-d514c7bdd043");
    #endregion

    #region System.LocalApplication
    /// <summary>
    /// Local Application (System.LocalApplication)
    /// </summary>
    public static Guid LocalApplicationClassId { get; } = new Guid("1f06bba9-58a9-fd0b-d980-08d5af2e67d3");
    #endregion

    #region System.Service
    /// <summary>
    /// Service (System.Service)
    /// </summary>
    public static Guid ServiceClassId { get; } = new Guid("1d870aa6-edb4-7d13-3950-d3c73755d6bf");
    public static class ServiceClassProperties
    {
      /// <summary>
      /// System.Service/ServiceDescription
      /// </summary>
      public static Guid ServiceDescriptionPropertyId { get; } = new Guid("f75caf82-ba43-cad3-c165-88f709fc3fbe");
      /// <summary>
      /// System.Service/BusinessDetailedDescription
      /// </summary>
      public static Guid BusinessDetailedDescriptionPropertyId { get; } = new Guid("0269f25a-1f9f-0423-4d7a-b286220d05d0");
      /// <summary>
      /// System.Service/IsBusinessService
      /// </summary>
      public static Guid IsBusinessServicePropertyId { get; } = new Guid("7879f2b1-3238-7221-e427-77785556d78a");
      /// <summary>
      /// System.Service/OwnedByOrganization
      /// </summary>
      public static Guid OwnedByOrganizationPropertyId { get; } = new Guid("112beb7c-a1c5-94b9-9954-a4d8cd0e0363");
      /// <summary>
      /// System.Service/Priority
      /// </summary>
      public static Guid PriorityPropertyId { get; } = new Guid("a4b16882-4709-46e6-97e7-5cf34b69ad28");
      /// <summary>
      /// System.Service/Status
      /// </summary>
      public static Guid StatusPropertyId { get; } = new Guid("9d19da0e-e22b-9835-1ba7-fddb36832789");
      /// <summary>
      /// System.Service/Classification
      /// </summary>
      public static Guid ClassificationPropertyId { get; } = new Guid("330dcee3-8fa0-875b-c8f0-fe5f77d12fff");
      /// <summary>
      /// System.Service/AvailabilitySchedule
      /// </summary>
      public static Guid AvailabilitySchedulePropertyId { get; } = new Guid("c8b4567b-842f-ae1b-0f39-1f446d1c7919");
    }
    #endregion

    #region System.ApplicationComponent
    /// <summary>
    /// Application Component (System.ApplicationComponent)
    /// </summary>
    public static Guid ApplicationComponentClassId { get; } = new Guid("15c8d578-72b5-756d-8e11-9a644a44ad67");
    #endregion

    #region System.Environment
    /// <summary>
    /// Environment (System.Environment)
    /// </summary>
    public static Guid EnvironmentClassId { get; } = new Guid("62d48404-535d-17cc-e01b-d4f45a7998cd");
    public static class EnvironmentClassProperties
    {
      /// <summary>
      /// System.Environment/Title
      /// </summary>
      public static Guid TitlePropertyId { get; } = new Guid("f59aed76-ea57-5339-4330-27149b398c3f");
      /// <summary>
      /// System.Environment/Description
      /// </summary>
      public static Guid DescriptionPropertyId { get; } = new Guid("edc0c554-1ad9-97e1-c979-8a8d58bf35ff");
      /// <summary>
      /// System.Environment/Notes
      /// </summary>
      public static Guid NotesPropertyId { get; } = new Guid("dbabac52-9202-419f-4993-f0ddb6cfc06c");
      /// <summary>
      /// System.Environment/Category
      /// </summary>
      public static Guid CategoryPropertyId { get; } = new Guid("76342a38-a140-aa2b-35e0-7c33919d2852");
    }
    #endregion

    #region System.GeoLocation
    /// <summary>
    /// Geo Location (System.GeoLocation)
    /// </summary>
    public static Guid GeoLocationClassId { get; } = new Guid("a7f774ed-534c-08af-2fbf-54cbe1745388");
    public static class GeoLocationClassProperties
    {
      /// <summary>
      /// System.GeoLocation/Id
      /// </summary>
      public static Guid IdPropertyId { get; } = new Guid("010edd9d-e536-b62a-8aae-577e21f26e52");
      /// <summary>
      /// System.GeoLocation/Latitude
      /// </summary>
      public static Guid LatitudePropertyId { get; } = new Guid("a0a917df-7797-75e9-7bc0-c65bd8a36c3b");
      /// <summary>
      /// System.GeoLocation/Longitude
      /// </summary>
      public static Guid LongitudePropertyId { get; } = new Guid("a7074e3d-44d5-56b2-2153-50adb6b821e4");
    }
    #endregion

    // === Relationships ===

    #region System.Reference
    /// <summary>
    /// Reference (System.Reference)
    /// </summary>
    public static Guid ReferenceRelationshipId { get; } = new Guid("5996b276-2d7f-7ca5-68e7-09186d9462d5");
    #endregion

    #region System.Containment
    /// <summary>
    /// Membership (System.Containment)
    /// </summary>
    public static Guid ContainmentRelationshipId { get; } = new Guid("189f4500-7a70-db53-9566-feea4695da29");
    #endregion

    #region System.Membership
    /// <summary>
    /// Membership (System.Membership)
    /// </summary>
    public static Guid MembershipRelationshipId { get; } = new Guid("c89ea3f4-bcc3-a4ab-89c0-c58c2cd88c84");
    #endregion

    #region System.Hosting
    /// <summary>
    /// Hosting (System.Hosting)
    /// </summary>
    public static Guid HostingRelationshipId { get; } = new Guid("ae80f883-4409-9e35-03da-90ecc19a8b2c");
    #endregion

    #region System.WatchedBy
    /// <summary>
    /// Entity Watched By Perspective (System.WatchedBy)
    /// </summary>
    public static Guid WatchedByRelationshipId { get; } = new Guid("26ae25af-4307-5414-b07f-eda2bf845f9b");
    #endregion
  }
}