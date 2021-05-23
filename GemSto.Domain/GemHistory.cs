using GemSto.Common.Enum;
using GemSto.Common.HelperMethods;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GemSto.Domain
{
    public class GemHistory : DomainBase
    {
        public long Id { get; set; }
        public GemHistoryStatusEnum GemHistoryStatusEnum { get; set; }
        public RelatedEntityNameEnum RelatedEntityNameEnum { get; set; }
        public int RelatedEntityId { get; set; }
        [Required, StringLength(2000)]
        public string Description { get; set; }
        public int? GemId { get; set; }
        public ActionEnum ActionEnum { get; set; }
        [Required, StringLength(50)]
        public string CreatedByName { get; set; }
        public GemStatus GemStatus { get; set; }
        public DateTimeOffset CreatedOn { get; set; } = DateTimeOffset.Now;

        public GemHistory CreateSingleGemPurchased(int gemId, string sellerName, string createdByName, string createdById, GemStatus gemStatus)
        {
            var lotDesc = gemStatus == GemStatus.GemLot ? "lot" : string.Empty;

            CreatedById = createdById;
            GemHistoryStatusEnum = GemHistoryStatusEnum.Purchased;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = $"Purchased gem {lotDesc} from {sellerName}";
            GemId = gemId;
            ActionEnum = ActionEnum.Created;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory CreateSingleGemTPReceived(int gemId, string sellerName, string createdByName, string createdById, GemStatus gemStatus)
        {
            var lotDesc = gemStatus == GemStatus.GemLot ? "lot" : string.Empty;

            CreatedById = createdById;
            GemHistoryStatusEnum = GemHistoryStatusEnum.Purchased;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = $"Received gem {lotDesc} from {sellerName}";
            GemId = gemId;
            ActionEnum = ActionEnum.Created;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }


        public GemHistory CreateNewGemAddedToLot(int gemId, string description, string createdByName, string createdById, GemStatus gemStatus)
        {
            var action = gemStatus == GemStatus.GemLot ? ActionEnum.Edited : ActionEnum.Created;
            var gemHistoryStatusEnum = gemStatus == GemStatus.GemLot ? GemHistoryStatusEnum.EditedGemDetail : GemHistoryStatusEnum.Purchased;

            CreatedById = createdById;
            GemHistoryStatusEnum = gemHistoryStatusEnum;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = description;
            GemId = gemId;
            ActionEnum = action;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory DeleteGem(int gemId, string deletedByName, string createdById, bool isLot, string stockNumber, GemStatus gemStatus)
        {
            var desc = isLot ? "lot" : string.Empty;

            CreatedById = createdById;
            GemHistoryStatusEnum = GemHistoryStatusEnum.DeletedFromInStock;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = $"Gem {desc} - {stockNumber}, deleted by {deletedByName}";
            GemId = null;
            ActionEnum = ActionEnum.Deleted;
            CreatedByName = deletedByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory EditGemDetail(int gemId, string createdByName, string createdById, GemStatus gemStatus, string description)
        {
            CreatedById = createdById;
            GemHistoryStatusEnum = GemHistoryStatusEnum.EditedGemDetail;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = description;
            GemId = gemId;
            ActionEnum = ActionEnum.Edited;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory ReturnGem(int gemId, string editedByName, string editedById, GemStatus gemStatus, bool isLotHeader, string stockNumber, ActionEnum actionEnum)
        {
            var des = !isLotHeader ? "Gem returned to seller" : (gemStatus == GemStatus.GemLot ? "Gem lot returned to seller" : $"Gem in lot - {stockNumber}, returned to seller");
            CreatedById = editedById;
            GemHistoryStatusEnum = GemHistoryStatusEnum.Returned;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = des;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = editedByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory UpdateGemStatus(int gemId, string editedByName, string editedById, GemStatus oldStatus, GemStatus gemStatus, bool isLotHeader, string stockNumber, ActionEnum actionEnum)
        {
            var prevStatus = oldStatus.DescriptionAttr().ToUpper();
            var curStatus = gemStatus.DescriptionAttr().ToUpper();

            var des = !isLotHeader ? $"Gem status changed from {prevStatus} to {curStatus}" : (gemStatus == GemStatus.GemLot ? $"Gem lot status changed from {prevStatus} to {curStatus}" : $"Gem in lot - {stockNumber}, status changed from {prevStatus} to {curStatus}");

            CreatedById = editedById;
            GemHistoryStatusEnum = GemHistoryStatusEnum.ReturnedToInStockFromSeller;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Gems;
            RelatedEntityId = gemId;
            Description = des;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = editedByName;
            GemStatus = oldStatus;

            return this;
        }

        public GemHistory CreateApprovalHistory(string createdById, GemHistoryStatusEnum gemHistoryStatusEnum, int relatedEntityId, string description, int gemId, ActionEnum actionEnum, string createdByName, GemStatus gemStatus)
        {

            CreatedById = createdById;
            GemHistoryStatusEnum = gemHistoryStatusEnum;
            RelatedEntityNameEnum = RelatedEntityNameEnum.GemApprovals;
            RelatedEntityId = relatedEntityId;
            Description = description;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory CreateCertificateHistory(string createdById, GemHistoryStatusEnum gemHistoryStatusEnum, int relatedEntityId, string description, int gemId, ActionEnum actionEnum, string createdByName, GemStatus gemStatus)
        {

            CreatedById = createdById;
            GemHistoryStatusEnum = gemHistoryStatusEnum;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Certificates;
            RelatedEntityId = relatedEntityId;
            Description = description;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }

        public GemHistory CreateCertificationHistory(string createdById, GemHistoryStatusEnum gemHistoryStatusEnum, int relatedEntityId, string description, int gemId, ActionEnum actionEnum, string createdByName, GemStatus gemStatus)
        {
            CreatedById = createdById;
            GemHistoryStatusEnum = gemHistoryStatusEnum;
            RelatedEntityNameEnum = RelatedEntityNameEnum.Certifications;
            RelatedEntityId = relatedEntityId;
            Description = description;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }


        public GemHistory CreateExportHistory(string createdById, GemHistoryStatusEnum gemHistoryStatusEnum, int relatedEntityId, string description, int gemId, ActionEnum actionEnum, string createdByName, GemStatus gemStatus)
        {
            CreatedById = createdById;
            GemHistoryStatusEnum = gemHistoryStatusEnum;
            RelatedEntityNameEnum = RelatedEntityNameEnum.GemExports;
            RelatedEntityId = relatedEntityId;
            Description = description;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }


        public GemHistory CreateSaleHistory(string createdById, GemHistoryStatusEnum gemHistoryStatusEnum, int relatedEntityId, string description, int gemId, ActionEnum actionEnum, string createdByName, GemStatus gemStatus)
        {
            CreatedById = createdById;
            GemHistoryStatusEnum = gemHistoryStatusEnum;
            RelatedEntityNameEnum = RelatedEntityNameEnum.GemSales;
            RelatedEntityId = relatedEntityId;
            Description = description;
            GemId = gemId;
            ActionEnum = actionEnum;
            CreatedByName = createdByName;
            GemStatus = gemStatus;

            return this;
        }
    }
}
