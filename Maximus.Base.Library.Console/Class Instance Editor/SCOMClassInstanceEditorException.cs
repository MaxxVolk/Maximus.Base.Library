using System;
using System.Runtime.Serialization;

namespace Maximus.Library.SCOM.Editors
{
  [Serializable]
  public class SCOMClassInstanceEditorException : Exception
  {
    public SCOMClassInstanceEditorException() : base() { }

    public SCOMClassInstanceEditorException(string Message) : base(Message) { }

    public SCOMClassInstanceEditorException(String message, Exception innerException) : base(message, innerException) { }

    protected SCOMClassInstanceEditorException(SerializationInfo info, StreamingContext context) : base(info, context) { }
  }
}