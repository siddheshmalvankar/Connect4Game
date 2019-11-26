using Ka4.Core.Interfaces.UseCases;
using Ka4.Core.UseCases;
using Autofac;

/// <summary>
/// Maps to the layers that hold the Use Case and Entity concerns and is also where our External Interfaces get defined. 
/// These innermost layers contain our domain objects and business rules. The code in this layer is mostly pure C# - no network connections, databases, etc. allowed. 
/// Interfaces represent those dependencies, and their implementations get injected into our use cases.
/// </summary>
namespace Ka4.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PlayConnect4>().As<IGameBoardUseCase>().InstancePerLifetimeScope();
           
        }
    }
}
