using GemSto.Common;
using GemSto.Service.Models.Sale;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Service.Contracts
{
    public interface ISalesService
    {
        Task CreateSaleAsync(SaleCreateModel saleCreateModel);
        Task<int> GetLastSaleNumberAsync();
        Task RemoveGemFromSaleAsync(int gemSaleId, string editedBy, string editedByName);
        Task<PaginationModel<SaleModel>> GetAllSalesAsync(PaginationBase paginationBase);
        Task UpdateSaleAsync(SaleUpdateModel saleUpdateModel);
        Task<SaleModel> GetSaleByIdAsync(int saleId);
        Task UpdateThirdPartySaleAsync(ThirdPartySaleUpdateModel thirdPartySaleUpdateModel, string editedBy);
        Task<PaginationModel<SaleModel>> GlobalSearchAsync(SaleGlobalSearchFilterModel globalSearchFilterModel, int skip, int take);
        Task<IEnumerable<KeyValuePair<int, string>>> GetAllSaleNumbersAsync(string saleNumber);
        Task<List<SaleProfitModel>> GetProfitByIdAsync(int saleId);
        Task DeleteGemFromSale(int gemSaleId, string editedById, string editedByName);
        Task<List<SaleGemsWithCertificateModel>> GetSaleGemsWithCertifiate(string buyerName, bool isLocalCurrency);
        Task<List<SaleGemsWithCertificatePendingModel>> GetSaleGemsWithCertificatePending(string buyerName, bool isLocalCurrency);
        Task<IEnumerable<KeyValuePair<int, string>>> GetAllSaleNumbersWithNoItemsinLotAsync(string saleNumber);
    }
}
