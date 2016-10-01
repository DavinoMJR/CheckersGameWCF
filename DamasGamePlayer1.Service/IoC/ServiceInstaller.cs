using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using DamasGamePlayer1.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Service
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.AddFacility<WcfFacility>();

            //container.Register(Component.For<IDamasGamePlayer1Service>()
            //                            .ImplementedBy<DamasGamePlayer1Service>()
            //                            .AsWcfService());

            //container.Register(Classes.FromThisAssembly()
            //                          .BasedOn<IService>()
            //                          .WithServiceAllInterfaces()
            //                          .LifestyleSingleton());

            //container.Register(Component.For<IGameWindow>()
            //                            .ImplementedBy(typeof(DamasServer)));

        }
    }
}
