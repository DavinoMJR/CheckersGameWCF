using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.IoC
{
    public class WindowInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IGameWindow>()
                     .ImplementedBy<DamasPlayer1GameWindow>()
                     .LifestyleSingleton());

            container.Register(Component.For<IHubWindow>()
                                        .ImplementedBy<Hub>()
                                        .LifestyleSingleton());
        }
    }
}
