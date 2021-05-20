﻿using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
#if STANDALONE
[assembly: AssemblyTitle("Maximus.Standalone.Library.ManagedWorkflowBase")]
#else
[assembly: AssemblyTitle("Maximus.Base.Library.ManagedWorkflowBase")]
#endif
[assembly: AssemblyDescription("Base managed module classes to create SCOM actions.")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
#if STANDALONE
[assembly: AssemblyProduct("Maximus.Standalone.Library.ManagedWorkflowBase")]
#else
[assembly: AssemblyProduct("Maximus.Base.Library.ManagedWorkflowBase")]
#endif
[assembly: AssemblyCopyright("Copyright © Maximus 2020")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5e0c0ba4-f155-493e-95bc-cc00dd323d8c")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
#if STANDALONE
[assembly: AssemblyVersion("1.0.2.0")]
[assembly: AssemblyFileVersion("1.0.2.0")]
#else
[assembly: AssemblyVersion("1.0.1.0")] // to reference the library
[assembly: AssemblyFileVersion("1.0.1.5")]
#endif