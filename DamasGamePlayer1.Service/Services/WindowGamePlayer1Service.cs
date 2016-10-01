using DamasGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Service
{
    public class WindowGamePlayer1Service
    {
         private IGameWindow _window;
         public WindowGamePlayer1Service(IGameWindow window)
        {
            _window = window;
        }

        public void RegisterPlay(Move move)
        {
            _window.ActOnReceive(move);
        }
    }
}
