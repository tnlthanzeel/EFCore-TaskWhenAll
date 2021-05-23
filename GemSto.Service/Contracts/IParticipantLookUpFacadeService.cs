using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IParticipantLookUpFacadeService
    {
        Task<List<string>> GetAllParticipantsAsync();
    }
}
