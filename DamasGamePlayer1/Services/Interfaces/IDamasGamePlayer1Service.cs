using DamasGame.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DamasGamePlayer1
{
    [ServiceContract]
    public interface IDamasGamePlayer1Service : IService
    {
        [OperationContract]
        [FaultContract(typeof(FaultException))]
        void Play(Move move);

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        bool StartGame();

        [OperationContract]
        void Teste();

        void PlayRemote(Move move);
    }
}
