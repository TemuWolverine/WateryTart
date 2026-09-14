using Autofac;

namespace SillyMIDI.Core;

public interface IPlatformSpecificRegistration
{
    void Register(ContainerBuilder builder);
}