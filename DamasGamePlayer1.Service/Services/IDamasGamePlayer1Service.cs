using DamasGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1.Service
{
    [ServiceContract]
    public interface IDamasGamePlayer1Service 
    {
        [OperationContract]
        void Play(Move move);


        [OperationContract]
        void StartGame();

    }
}
