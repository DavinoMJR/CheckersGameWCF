using DamasGame.Entities;
using DamasGame.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1
{
    public class DamasGamePlayer1Service : IDamasGamePlayer1Service
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(DamasGamePlayer1Service));

        public DamasGamePlayer1Service()
        {
        }

        public void Play(Move move)
        {
            try
            {
                Log.Info(String.Format(CultureInfo.CurrentCulture, Messages.MSG_RECEBIMENTO_CHAMADA, Constants.JOGADOR2));
                IGameWindow gameWindow = InjectionContainer.Container.Resolve<IGameWindow>();
                gameWindow.ActOnReceive(move);
                Log.Info(String.Format(CultureInfo.CurrentCulture, Messages.MSG_JOGADA_COMPLETA, Constants.JOGADOR2));
            }
            catch (Exception ex)
            {
                Log.Error(String.Format(CultureInfo.CurrentCulture, Messages.MSG_ERRO_GENERICA, Constants.JOGADOR2, ex.Message));
            }    
        }

        public bool StartGame()
        {
            IHubWindow hubWindow = InjectionContainer.Container.Resolve<IHubWindow>();
            hubWindow.CloseHubAndStartGame();
            return true;
        }

        public void PlayRemote(Move move)
        {
            try
            {
                DamasGamePlayer2Service.DamasGamePlayer2ServiceClient client = new DamasGamePlayer2Service.DamasGamePlayer2ServiceClient();
                client.Play(move);
            }
            catch (Exception ex)
            {
                Log.Error(String.Format(CultureInfo.CurrentCulture, Messages.MSG_ERRO_GENERICA, Constants.JOGADOR2, ex.Message));
            }
        }

        #region IDamasGamePlayer1Service Members


        public void Teste()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
