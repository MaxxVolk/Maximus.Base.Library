using System;
using System.Runtime.Serialization;

namespace Maximus.Library.SCOM.Editors
{
  public class SCOMClassInstanceCommintStatusException : Exception
  {
    public SCOMClassInstanceCommintStatusException() : base() { }

    public SCOMClassInstanceCommintStatusException(string Message) : base(Message) { }

    public SCOMClassInstanceCommintStatusException(string message, Exception innerException) : base(message, innerException) { }

    protected SCOMClassInstanceCommintStatusException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}