using Autofac;

namespace WateryTart.Core;

public interface IHaveSettings
{

}
public interface IPlatformSpecificRegistration
{
    public void Register(ContainerBuilder builder);
}