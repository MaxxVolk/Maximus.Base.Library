using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Maximus.Library.ManagedModuleBase
{
  [Serializable]
  public class ModuleBaseException : Exception
  {
    public ModuleBaseException() : base() { }

    public ModuleBaseException(string Message) : base(Message) { }

    public ModuleBaseException(String message, Exception innerException) : base(message, innerException) { }

    protected ModuleBaseException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}
