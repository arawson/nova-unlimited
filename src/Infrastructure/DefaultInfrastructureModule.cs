
using System.Reflection;
using Autofac;
using MediatR;
using MediatR.Pipeline;
using NovaUnlimited.Core.Entities;
using NovaUnlimited.Infrastructure.Data;
using NovaUnlimited.Kernel.Interfaces;

namespace NovaUnlimited.Infrastructure;

public class DefaultInfrastructureModule : Autofac.Module
{
    private readonly bool isDevelopment = false;
    private readonly List<Assembly> assemblies = new List<Assembly>();

    public DefaultInfrastructureModule(
        bool isDevelopment,
        Assembly? callingAssembly = null
    )
    {
        this.isDevelopment = isDevelopment;
        var coreAssembly = Assembly.GetAssembly(typeof(Game)); // any type from core
        var infrastructureAssembly = Assembly.GetAssembly(typeof(StartupSetup));

        if (coreAssembly is not null) assemblies.Add(coreAssembly);
        if (infrastructureAssembly is not null) assemblies.Add(infrastructureAssembly);
        if (callingAssembly is not null) assemblies.Add(callingAssembly);
    }

    protected override void Load(ContainerBuilder builder)
    {
        if (isDevelopment)
        {
            RegisterDevelopmentOnlyDependencies(builder);
        }
        else
        {
            RegisterProductionOnlyDependencies(builder);
        }
        RegisterCommonDependencies(builder);
    }

    private void RegisterCommonDependencies(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EfRepository<>))
            .As(typeof(IRepository<>))
            .As(typeof(IReadRepository<>))
            .InstancePerLifetimeScope();

        builder.RegisterType<Mediator>()
            .As<IMediator>()
            .InstancePerLifetimeScope();
        
        builder.Register<ServiceFactory>(context =>
        {
            var c = context.Resolve<IComponentContext>();
            return t => c.Resolve(t);
        });

        var mediatrOpentypes = new []
        {
            typeof(IRequestHandler<,>),
            typeof(IRequestExceptionHandler<,,>),
            typeof(IRequestExceptionAction<,>),
            typeof(INotificationHandler<>),
        };

        foreach (var mediatrOpentype in mediatrOpentypes)
        {
            builder.RegisterAssemblyTypes(assemblies.ToArray())
                .AsClosedTypesOf(mediatrOpentype)
                .AsImplementedInterfaces();
        }

        // builder.RegisterType<EmailSender>().As<IEmailSender>()
            // .InstancePerLifetimeScope();
    }

    private void RegisterDevelopmentOnlyDependencies(ContainerBuilder builder)
    {
        // TODO: Add development only services
    }

    private void RegisterProductionOnlyDependencies(ContainerBuilder builder)
    {
        // TODO: add production only services
    }
}
