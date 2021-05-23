using GemSto.Common;
using GemSto.Common.Enum;
using GemSto.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface IGemService
    {
        Task<PaginationModel<GemModel>> GetAllAsync(PaginationBase paginationBase, bool isThirdParty);
        Task<PaginationModel<GemModel>> GetBySearchQueryAsync(PaginationBase paginationBase, bool isThirdParty);
        Task CreateNewGemAsync(SignleGemCreateModel gemModel);
        Task DeleteSingleGemAsync(int gemIdToDelete, string deletedByName, string createdById);
        Task<string> LastStockIdAsync(bool isThirdParty);
        Task SaveGemLotAsync(GemLotListCreateModel gemLotListCreateModel);
        Task<GemModel> GetByIdAsync(Guid gemIdentity);
        Task UpdateGemDetailAsync(GemUpdateModel gemUpdateModel);
        Task ReturnSingleGemAsync(int gemIdToReturn, string EditedBy, string editedByName);
        Task<IEnumerable<Object>> GetGemStockIdsAsync(string stockId, GemStatus gemStatus, bool? isThirdParty);
        Task GetGemByIdWithFilterAsync(Guid gemId, string filterBy);
        Task UpdateGemStatusAsync(int gemId, GemStatus gemStatus, string editedById, string editedByName);
        Task<NextAndPreviousModel> GetNextAndPreviouseGemIdsAsync(int currentGemId, bool isThirdParty);
        Task AddMoreGemsToLotAsync(GemLotCreateList gemLotCreate, int lotId, string lotStockId, string createdByName, string createdById);
        Task<PaginationModel<GemModel>> GlobalSearchAsync(GlobalSearchFilterModel globalSearchFilterModel, int skip, int take, bool isThirdParty);
        Task<GemModel> GetByintIdAsync(int gemIntId);
        Task<List<GemModel>> GetByintIdSaleCalculatorAsync(int gemIntId);
        Task<PaginationModel<GemModel>> GetUnPaidAmountBySellerAsync(string sellerName, bool isPrintable);
        Task<List<GemHistoryModel>> GetGemHistoryAsync(int gemId);
        Task<List<KeyValuePair<int, string>>> GetPaymentIncompleteGemsAsync(string sellerName);
        Task<IEnumerable<GemModel>> GetPaymentIncompleteGemsForBulkPaymentAsync(string sellerName);
        Task<IEnumerable<Object>> GetGemStockIdsForPayentLogAsync(string stockId);
    }
}
