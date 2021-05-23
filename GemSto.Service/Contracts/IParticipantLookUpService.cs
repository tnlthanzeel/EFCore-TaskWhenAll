using GemSto.Service.Models.Participant;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IParticipantLookUpService
    {
        Task<bool> AddNewParticipantAsync(ParticipantCreateModel participantCreateModel);
        Task<List<ParticipantModel>> GetAllParticipantsAsync();
        Task ParticipantDeleteAsync(int id);
        Task<ParticipantModel> ParticipantGetAsync(int id);
        Task<bool> ParticipantUpdateAsync(ParticipantUpdateModel participantUpdateModel);
        Task<List<string>> GetAllParticipantNamesAsync();
    }
}
