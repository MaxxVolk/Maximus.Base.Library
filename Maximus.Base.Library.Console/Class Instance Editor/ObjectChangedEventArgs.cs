using System;

namespace Maximus.Library.SCOM.Editors
{
  public enum InstanceCommitStatus { New, Committed, Modified } // New, Committed, Modified

  public class ObjectChangedEventArgs : EventArgs
  {
    public ObjectChangedEventArgs(InstanceCommitStatus CommitStatus) : base() => this.CommitStatus = CommitStatus;

    public InstanceCommitStatus CommitStatus { get; set; }
  }
}