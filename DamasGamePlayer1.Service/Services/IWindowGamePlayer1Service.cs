using DamasGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Service
{
    public interface IWindowGamePlayer1Service : IService
    {
        void RegisterPlay(Move move);
    }
}
