using System;

namespace Maximus.Library.Helpers
{
  public enum FailureReason { Success, NotFound, ApplicationError, SystemError, Constraint, AccessDenied, Disconnected, Exception, AlreadyExists, Timeout, Retry }

  public class ResultWrapper<ReturnType>
  {
    public ResultWrapper(ReturnType result)
    {
      Result = result;
      IsOK = true;
      FailureReason = FailureReason.Success;
      Exception = null;
    }

    public ResultWrapper()
    {
      Result = default;
      IsOK = true;
      FailureReason = FailureReason.Success;
      Exception = null;
    }

    public ResultWrapper(ReturnType result, FailureReason failureReason)
    {
      Result = result;
      IsOK = false;
      FailureReason = failureReason;
      Exception = null;
    }

    public ResultWrapper(ReturnType result, Exception exception)
    {
      Result = result;
      IsOK = false;
      FailureReason = FailureReason.Exception;
      Exception = exception;
    }

    public ResultWrapper(ReturnType result, Exception exception, FailureReason failureReason)
    {
      Result = result;
      IsOK = false;
      FailureReason = failureReason;
      Exception = exception;
    }

    public ResultWrapper(Exception exception, FailureReason failureReason)
    {
      Result = default;
      IsOK = false;
      FailureReason = failureReason;
      Exception = exception;
    }

    public ResultWrapper(FailureReason failureReason)
    {
      Result = default;
      IsOK = false;
      FailureReason = failureReason;
      Exception = null;
    }

    public ResultWrapper(Exception exception)
    {
      Result = default;
      IsOK = false;
      FailureReason = FailureReason.Exception;
      Exception = exception;
    }

    public ReturnType Result { get; protected set; }
    public bool IsOK { get; protected set; }
    public FailureReason FailureReason { get; protected set; }
    public Exception Exception { get; protected set; }

    public override string ToString()
    {
      return (IsOK ? "OK" : ("Failure: " + FailureReason.ToString())) + " {" + (Result?.ToString() ?? "NULL") + "}";
    }
  }
}
