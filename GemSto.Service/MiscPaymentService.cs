using AutoMapper;
using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Common.HelperMethods;
using GemSto.Data;
using GemSto.Domain;
using GemSto.Service.Contracts;
using GemSto.Service.Models;
using GemSto.Service.Models.Miscellaneous;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service
{
    public class MiscPaymentService : IMiscPaymentService
    {
        private readonly GemStoContext _gemStoContext;
        private readonly IMapper _mapper;

        public MiscPaymentService(GemStoContext gemStoContext)
        {
            _gemStoContext = gemStoContext;
        }
        public async Task<PaginationModel<SellerPaymentHistoryModel>> GetMiscPaymentHistoryAsync(int skip = 0, int take = 75)
        {
            try
            {
                var query = _gemStoContext.MiscPayments.Where(w => !w.IsDeleted);

                var totalCount = await query.CountAsync();

                query = query.OrderByDescending(o => o.CreatedOn).Skip(skip).Take(take);

                var result = query.Select(s => new SellerPaymentHistoryModel
                {
                    Id = s.Id,
                    SellerName = s.ParticipantId.HasValue ? s.Participant.Value : (s.ParticipantName ?? "-"),
                    StockNumber = (s.SubMiscCat.HasValue ? s.MiscSubCategory.Value + " - " : string.Empty) + s.MiscCategory.Value,
                    PaidAmount = s.Amount,
                    PaidOn = s.PaymentDate,
                    PaymentHistoryType = PaymentHistoryType.Miscellaneous,
                    Description = s.Description
                });

                var resultData = await result.AsNoTracking().ToListAsync();

                var resultSet = new PaginationModel<SellerPaymentHistoryModel>()
                {
                    Details = resultData,
                    TotalRecords = totalCount
                };

                return resultSet;
            }
            catch (Exception e)
            {

                throw;
            }

        }
    }
}
