﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maximus.Base.Library.Console
{
  /// <summary>
  /// https://stackoverflow.com/a/17661276/6763830
  /// </summary>
  /// <typeparam name="TAbstract"></typeparam>
  /// <typeparam name="TBase"></typeparam>
  public class AbstractControlDescriptionProvider<TAbstract, TBase> : TypeDescriptionProvider
  {
    public AbstractControlDescriptionProvider()
        : base(TypeDescriptor.GetProvider(typeof(TAbstract)))
    {
    }

    public override Type GetReflectionType(Type objectType, object instance)
    {
      if (objectType == typeof(TAbstract))
        return typeof(TBase);

      return base.GetReflectionType(objectType, instance);
    }

    public override object CreateInstance(IServiceProvider provider, Type objectType, Type[] argTypes, object[] args)
    {
      if (objectType == typeof(TAbstract))
        objectType = typeof(TBase);

      return base.CreateInstance(provider, objectType, argTypes, args);
    }
  }
}
