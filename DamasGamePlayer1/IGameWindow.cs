using DamasGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1
{
    public interface IGameWindow
    {
        void ActOnReceive(Move move);

        void Show();

        void CloseGame(string message);
    }
}
