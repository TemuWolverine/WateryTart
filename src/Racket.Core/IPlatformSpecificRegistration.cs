using Autofac;

namespace Racket.Core;

public interface IPlatformSpecificRegistration
{
    void Register(ContainerBuilder builder);
}