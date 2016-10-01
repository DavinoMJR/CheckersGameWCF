using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Host
{
    public class InjectionContainer
    {
        public static readonly WindsorContainer container = new WindsorContainer();

        public static void Configure()
        {
            container.Install(FromAssembly.This());
            ConfigureWcf();
        }

        private static void ConfigureWcf()
        {
            container.Register(Component.For<IGameWindow>()
                                        .ImplementedBy<DamasServer>());

            container.AddFacility<WcfFacility>();

            container.Register(Component.For<IDamasGamePlayer1Service>()
                                        .ImplementedBy<DamasGamePlayer1Service>()
                                        .AsWcfService()
                                        .LifestyleSingleton());
        }
    }
}
