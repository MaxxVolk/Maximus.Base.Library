using System;

namespace Maximus.Library.Helpers
{
  public delegate T RemediationAction<T>();

  public delegate Exception ExceptionAction();

  public delegate void UsingAction<T>(T input);

  public static class AssertationHelper
  {
    public static T AssertNotNull<T>(T input)
    {
      if (input != null)
        return input;
      throw new ArgumentNullException();
    }

    public static T AssertNotNull<T>(T input, Action action)
    {
      if (input != null)
        return input;
      action?.Invoke();
      return default(T);
    }

    public static T AssertNotNull<T>(T input, ExceptionAction action)
    {
      if (input != null)
        return input;
      throw action?.Invoke();
    }

    public static T AssertNotNull<T>(T input, RemediationAction<T> action)
    {
      if (input != null)
        return input;
      return action();
    }

    public static void UseIfNotNull<T>(T input, UsingAction<T> usingAction)
    {
      if (input != null)
        usingAction?.Invoke(input);
    }
  }
}