using DamasGame.Entities;
using DamasGamePlayer1.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Service
{
    public class DamasGamePlayer1Service : IDamasGamePlayer1Service
    {
        private readonly IGameWindow window;

        public DamasGamePlayer1Service()
        {

        }
        public DamasGamePlayer1Service(IGameWindow window)
        {
            this.window = window;
        }

        public void Play(Move move)
        {
            try
            {
                IGameWindow gameWindow = InjectionContainer.container.Resolve<IGameWindow>();
                gameWindow.ActOnReceive(move);
            }
            catch (Exception ex)
            {

            }
    
        }

        public void StartGame()
        {
            
        }
    }
}
