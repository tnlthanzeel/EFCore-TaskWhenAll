using AutoMapper;
using GemSto.Data;
using GemSto.Domain.LookUp;
using GemSto.Service.Contracts;
using GemSto.Service.Models.Participant;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class ParticipantLookUpService : IParticipantLookUpService
    {
        private readonly GemStoContext _gemStoContext;
        private readonly IMapper _mapper;

        public ParticipantLookUpService(GemStoContext gemStoContext, IMapper mapper)
        {
            _gemStoContext = gemStoContext;
            _mapper = mapper;
        }
        public async Task<bool> AddNewParticipantAsync(ParticipantCreateModel participantCreateModel)
        {
            var isAny = await _gemStoContext.Participants.AnyAsync(x => x.Value == participantCreateModel.Value && !x.IsDeleted);
            if (isAny)
            {
                return false;
            }
            else
            {
                var entity = _mapper.Map<Participant>(participantCreateModel);
                _gemStoContext.Participants.Add(entity);
                await _gemStoContext.SaveChangesAsync();
                return true;
            }
        }

        public async Task<List<ParticipantModel>> GetAllParticipantsAsync()
        {
            var catList = await _gemStoContext.Participants.Where(w => w.IsDeleted == false)
                .OrderBy(o => o.Value)
                .Select(s => new ParticipantModel
                {
                    Id = s.Id,
                    Value = s.Value
                }).ToListAsync();

            return catList;
        }

        public async Task ParticipantDeleteAsync(int id)
        {
            var entity = await _gemStoContext.Participants.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);

            entity.IsDeleted = true;
            await _gemStoContext.SaveChangesAsync();
        }

        public async Task<ParticipantModel> ParticipantGetAsync(int id)
        {
            var entity = await _gemStoContext.Participants.AsNoTracking().FirstOrDefaultAsync(f => f.Id == id);
            var model = _mapper.Map<ParticipantModel>(entity);
            return model;
        }

        public async Task<bool> ParticipantUpdateAsync(ParticipantUpdateModel participantUpdateModel)
        {
            var isAny = await _gemStoContext.Participants.AnyAsync(x => x.Value == participantUpdateModel.Value && !x.IsDeleted && x.Id != participantUpdateModel.Id);
            if (isAny)
            {
                return false;
            }
            var entity = await _gemStoContext.Participants.FirstOrDefaultAsync(f => f.Id == participantUpdateModel.Id);

            entity = _mapper.Map(participantUpdateModel, entity);
            _gemStoContext.Participants.Update(entity);
            await _gemStoContext.SaveChangesAsync();
            return true;
        }


        public async Task<List<string>> GetAllParticipantNamesAsync()
        {
            var participantNames = new List<string>();
            var participantsNotInStore = await _gemStoContext.MiscPayments.Where(w => w.ParticipantName != null).Select(s => s.ParticipantName).Distinct().ToListAsync();

            participantNames.AddRange(participantsNotInStore);
            var participantsInStore = await _gemStoContext.Participants.Where(w => !w.IsDeleted).Select(s => s.Value).ToListAsync();

            participantNames.AddRange(participantsInStore);
            participantNames.Sort();
            return participantNames;
        }
    }
}
