using GemSto.Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class ParticipantLookUpFacadeService : IParticipantLookUpFacadeService
    {
        private readonly ISellerLookUpService _sellerLookUpService;
        private readonly IBuyerLookUpService _buyerLookUpService;
        private readonly IParticipantLookUpService _participantLookUpService;

        public ParticipantLookUpFacadeService(ISellerLookUpService sellerLookUpService, IBuyerLookUpService buyerLookUpService, IParticipantLookUpService participantLookUpService)
        {
            _sellerLookUpService = sellerLookUpService;
            _buyerLookUpService = buyerLookUpService;
            _participantLookUpService = participantLookUpService;
        }
        public async Task<List<string>> GetAllParticipantsAsync()
        {
            var allParticipantNames = new List<string>();

            var sellerNamesTask = _sellerLookUpService.GetAllSellerNamesAsync();

            var buyerNamesTask = _buyerLookUpService.GetAllBuyerNamesAsync();

            var participantNamesTask = _participantLookUpService.GetAllParticipantNamesAsync();

            await Task.WhenAll(sellerNamesTask, buyerNamesTask, participantNamesTask);

            var sellerNames = await sellerNamesTask;

            var buyerNames = await buyerNamesTask;

            var participantNames = await participantNamesTask;

            allParticipantNames.AddRange(sellerNames);
            allParticipantNames.AddRange(buyerNames);
            allParticipantNames.AddRange(participantNames);

            var distinctList = allParticipantNames.Distinct().ToList();

            distinctList.Sort();

            return distinctList;
        }
    }
}
