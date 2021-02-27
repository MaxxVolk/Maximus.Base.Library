// The most up to date version is available 
// on GitHub: https://github.com/meziantou/Meziantou.Framework/tree/master/src/Meziantou.Framework.Win32.CredentialManager
// NuGet package: https://www.nuget.org/packages/Meziantou.Framework.Win32.CredentialManager/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Win32.SafeHandles;

namespace Maximus.Library.CredentialManagement
{
  public static class CredentialManager
  {
    public static Credential ReadCredential(string applicationName)
    {
      bool read = CredRead(applicationName, CredentialType.Generic, 0, out IntPtr nCredPtr);
      if (read)
      {
        using (CriticalCredentialHandle critCred = new CriticalCredentialHandle(nCredPtr))
        {
          CREDENTIAL cred = critCred.GetCredential();
          return ReadCredential(cred);
        }
      }

      return null;
    }

    private static Credential ReadCredential(CREDENTIAL credential)
    {
      string applicationName = Marshal.PtrToStringUni(credential.TargetName);
      string userName = Marshal.PtrToStringUni(credential.UserName);
      string secret = null;
      if (credential.CredentialBlob != IntPtr.Zero)
      {
        secret = Marshal.PtrToStringUni(credential.CredentialBlob, (int)credential.CredentialBlobSize / 2);
      }

      return new Credential(credential.Type, applicationName, userName, secret);
    }

    public static void WriteCredential(string applicationName, string userName, string secret)
    {
      byte[] byteArray = secret == null ? null : Encoding.Unicode.GetBytes(secret);
      // XP and Vista: 512; 
      // 7 and above: 5*512
      if (Environment.OSVersion.Version < new Version(6, 1) /* Windows 7 */)
      {
        if (byteArray != null && byteArray.Length > 512)
          throw new ArgumentOutOfRangeException("secret", "The secret message has exceeded 512 bytes.");
      }
      else
      {
        if (byteArray != null && byteArray.Length > 512 * 5)
          throw new ArgumentOutOfRangeException("secret", "The secret message has exceeded 2560 bytes.");
      }

      CREDENTIAL credential = new CREDENTIAL
      {
        AttributeCount = 0,
        Attributes = IntPtr.Zero,
        Comment = IntPtr.Zero,
        TargetAlias = IntPtr.Zero,
        Type = CredentialType.Generic,
        Persist = (uint)CredentialPersistence.LocalMachine,
        CredentialBlobSize = (uint)(byteArray == null ? 0 : byteArray.Length),
        TargetName = Marshal.StringToCoTaskMemUni(applicationName),
        CredentialBlob = Marshal.StringToCoTaskMemUni(secret),
        UserName = Marshal.StringToCoTaskMemUni(userName ?? Environment.UserName)
      };

      bool written = CredWrite(ref credential, 0);
      Marshal.FreeCoTaskMem(credential.TargetName);
      Marshal.FreeCoTaskMem(credential.CredentialBlob);
      Marshal.FreeCoTaskMem(credential.UserName);

      if (!written)
      {
        int lastError = Marshal.GetLastWin32Error();
        throw new Exception(string.Format("CredWrite failed with the error code {0}.", lastError));
      }
    }

    public static ReadOnlyCollection<Credential> EnumerateCrendentials()
    {
      List<Credential> result = new List<Credential>();

      bool ret = CredEnumerate(null, 0, out int count, out IntPtr pCredentials);
      if (ret)
      {
        for (int n = 0; n < count; n++)
        {
          IntPtr credential = Marshal.ReadIntPtr(pCredentials, n * Marshal.SizeOf(typeof(IntPtr)));
          result.Add(ReadCredential((CREDENTIAL)Marshal.PtrToStructure(credential, typeof(CREDENTIAL))));
        }
      }
      else
      {
        int lastError = Marshal.GetLastWin32Error();
        throw new Win32Exception(lastError);
      }

      return result.AsReadOnly();
    }

    [DllImport("Advapi32.dll", EntryPoint = "CredReadW", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern bool CredRead(string target, CredentialType type, int reservedFlag, out IntPtr credentialPtr);

    [DllImport("Advapi32.dll", EntryPoint = "CredWriteW", CharSet = CharSet.Unicode, SetLastError = true)]
    static extern bool CredWrite([In] ref CREDENTIAL userCredential, [In] UInt32 flags);

    [DllImport("advapi32", SetLastError = true, CharSet = CharSet.Unicode)]
    static extern bool CredEnumerate(string filter, int flag, out int count, out IntPtr pCredentials);

    [DllImport("Advapi32.dll", EntryPoint = "CredFree", SetLastError = true)]
    static extern bool CredFree([In] IntPtr cred);

    private enum CredentialPersistence : uint
    {
      Session = 1,
      LocalMachine,
      Enterprise
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct CREDENTIAL
    {
      public uint Flags;
      public CredentialType Type;
      public IntPtr TargetName;
      public IntPtr Comment;
      public System.Runtime.InteropServices.ComTypes.FILETIME LastWritten;
      public uint CredentialBlobSize;
      public IntPtr CredentialBlob;
      public uint Persist;
      public uint AttributeCount;
      public IntPtr Attributes;
      public IntPtr TargetAlias;
      public IntPtr UserName;
    }

    sealed class CriticalCredentialHandle : CriticalHandleZeroOrMinusOneIsInvalid
    {
      public CriticalCredentialHandle(IntPtr preexistingHandle)
      {
        SetHandle(preexistingHandle);
      }

      public CREDENTIAL GetCredential()
      {
        if (!IsInvalid)
        {
          CREDENTIAL credential = (CREDENTIAL)Marshal.PtrToStructure(handle, typeof(CREDENTIAL));
          return credential;
        }

        throw new InvalidOperationException("Invalid CriticalHandle!");
      }

      protected override bool ReleaseHandle()
      {
        if (!IsInvalid)
        {
          CredFree(handle);
          SetHandleAsInvalid();
          return true;
        }

        return false;
      }
    }
  }

  public enum CredentialType
  {
    Generic = 1,
    DomainPassword,
    DomainCertificate,
    DomainVisiblePassword,
    GenericCertificate,
    DomainExtended,
    Maximum,
    MaximumEx = Maximum + 1000,
  }

  public class Credential
  {
    public CredentialType CredentialType { get; }

    public string ApplicationName { get; }

    public string UserName { get; }

    public string Password { get; }

    public Credential(CredentialType credentialType, string applicationName, string userName, string password)
    {
      ApplicationName = applicationName;
      UserName = userName;
      Password = password;
      CredentialType = credentialType;
    }

    public override string ToString()
    {
      return string.Format("CredentialType: {0}, ApplicationName: {1}, UserName: {2}, Password: {3}", CredentialType, ApplicationName, UserName, Password);
    }
  }
}