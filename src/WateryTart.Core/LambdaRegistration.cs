using System;
using Autofac;

namespace WateryTart.Core;

public class LambdaRegistration<T>(Func<IComponentContext, T> factory) : IPlatformSpecificRegistration where T : class
{
    private readonly Func<IComponentContext, T> _factory = factory;

    public void Register(ContainerBuilder builder)
    {
        builder.Register((c,p) => _factory.Invoke(c)).AsSelf().SingleInstance();
    }
}