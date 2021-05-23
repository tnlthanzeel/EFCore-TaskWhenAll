using System;
using System.Collections.Generic;
using System.Text;

namespace GemSto.Common.Enum
{
    public enum GemHistoryStatusEnum
    {
        Purchased = 1,
        InStock,
        Returned,
        ReturnedToInStockFromSeller,
        Lost,
        DeletedFromInStock,
        EditedGemDetail,

        AddedToCertification,
        RemovedFromCertification,
        Certified,
        RemovedCertificate,
        UpdatedCertificateDetails,

        AddedToExport,
        RemovedFromExport,
        SoldAtExport,

        AddedToApproval,
        RemovedFromApproval,
        SoldAtApproval,

        AddedToSale,
        RemovedFromSale,
        DeletedFromSales,

        AddedSellerPayment,
        EditedSellerPayment,
        DeletedSellerPayment,

        AddedBuyerPayment,
        EditedBuyerPayment,
        DeletedBuyerPayment
    }
}
