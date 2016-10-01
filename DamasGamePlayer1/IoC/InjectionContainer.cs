using Castle.Facilities.WcfIntegration;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1
{
    public class InjectionContainer
    {
        public static readonly WindsorContainer Container = new WindsorContainer();

        public static void Configure()
        {
            Container.Install(FromAssembly.This());
        }
    }
}
