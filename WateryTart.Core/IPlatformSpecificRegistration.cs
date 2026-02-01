using Autofac;

namespace WateryTart.Core;

public interface IHaveSettings
{
    public string Icon { get; }

}
public interface IPlatformSpecificRegistration
{
    public void Register(ContainerBuilder builder);
}