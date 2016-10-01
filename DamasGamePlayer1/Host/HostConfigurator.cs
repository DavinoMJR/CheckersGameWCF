using DamasGame.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Host
{
    public class HostConfigurator
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HostConfigurator));

        public static void RunServer()
        {
            Log.Info(Messages.MSG_INICIANDO_SERVIDOR_JOGADOR1);
            ServiceHost host = new ServiceHost(typeof(DamasGamePlayer1Service));
            host.Open();
            Log.Info(Messages.MSG_SERVIDOR_INICIADO);
        }
    }
}
