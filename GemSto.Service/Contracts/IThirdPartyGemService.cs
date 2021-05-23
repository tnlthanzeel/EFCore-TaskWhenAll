using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IThirdPartyGemService
    {
        Task CreateNewThirdPartyGemAsync(SignleGemCreateModel gemModel);
        Task SaveGemLotAsync(GemLotListCreateModel gemLotListCreateModel);

    }
}
