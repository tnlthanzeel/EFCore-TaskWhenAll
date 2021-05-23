using AutoMapper;
using GemSto.Domain;
using GemSto.Domain.LookUp;
using GemSto.Domain.User;
using GemSto.Service.Models;
using GemSto.Service.Models.Account;
using GemSto.Service.Models.Approval;
using GemSto.Service.Models.Approver;
using GemSto.Service.Models.Certification;
using GemSto.Service.Models.Export;
using GemSto.Service.Models.Miscellaneous;
using GemSto.Service.Models.Participant;
using GemSto.Service.Models.Sale;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemSto.Web
{
    public class GemStoMapperProfile : Profile
    {
        public GemStoMapperProfile()
        {
            CreateMap<StaffModel, StoreUser>().ReverseMap();
            CreateMap<IdentityResult, StaffModel>().ReverseMap();
            CreateMap<GemModel, Gem>().ReverseMap();
            CreateMap<ShapeModel, Shape>().ReverseMap();
            CreateMap<Variety, VarietyModel>().ReverseMap();
            CreateMap<Seller, SellerModel>().ReverseMap();
            CreateMap<SignleGemCreateModel, Gem>().ReverseMap();
            CreateMap<CertificateProviderCreateModel, CertificateProvider>().ReverseMap();
            CreateMap<CertificateProviderModel, CertificateProvider>().ReverseMap();
            CreateMap<ColourCreateModel, Colour>().ReverseMap();
            CreateMap<ColourModel, Colour>().ReverseMap();
            CreateMap<VarietyCreateModel, Variety>().ReverseMap();
            CreateMap<ShapeCreateModel, Shape>().ReverseMap();
            CreateMap<CertificateCreateModel, Certificate>();
            CreateMap<Certificate, CertificateModel>().ForMember(dest => dest.CerticateNumber, src => src.MapFrom(dest => dest.Number));
            CreateMap<Origin, OriginModel>().ReverseMap();
            CreateMap<Export, ExportCreateModel>().ReverseMap();
            CreateMap<GemToExport, GemExport>();
            CreateMap<ExportThirdPartyCreateModel, GemExport>();
            CreateMap<ThirdPartyUpdateModel, GemExport>();
            CreateMap<CertificationCreateModel, Certification>();
            CreateMap<CertificationCertificateCreateModel, CertificateCreateModel>().ForMember(dest => dest.Id, src => src.Ignore());
            CreateMap<Certification, CertificationModel>().ForMember(dest => dest.StockNumber, src => src.MapFrom(dest => dest.Owner));
            CreateMap<AddThirdPartyCertificateModel, ThirdPartyCertificate>();
            CreateMap<ApprovalSummaryCreateModel, Approval>();
            CreateMap<AddGemToApprovalListModel, GemApproval>();
            CreateMap<Approval, ApprovalSummaryModel>().ForMember(dest => dest.ApproverName, src => src.MapFrom(dest => dest.Approver.Name));
            CreateMap<ApproverCreateModel, Approver>();
            CreateMap<ApproverUpdateModel, Approver>();
            CreateMap<Approver, ApproverModel>();
            CreateMap<BuyerCreateModel, Buyer>();
            CreateMap<Buyer, BuyerModel>();
            CreateMap<BuyerUpdateModel, Buyer>();
            CreateMap<SaleCreateModel, Sale>();
            CreateMap<SalePayment, SalePaymentModel>().ForMember(src => src.Date, dest => dest.MapFrom(src => src.CreatedOn));
            CreateMap<MiscellaneousCreateModel, Miscellaneous>();
            CreateMap<Miscellaneous, MiscellaneousModel>();
            CreateMap<MiscellaneousUpdateModel, Miscellaneous>();
            CreateMap<MiscCategory, MiscCategoryCreateModel>().ReverseMap();
            CreateMap<MiscCategory, MiscCategoryModel>().ReverseMap();
            CreateMap<MiscSubCategoryCreateModel, MiscSubCategory>().ReverseMap();
            CreateMap<MiscSubCategoryUdateModel, MiscSubCategory>();
            CreateMap<ParticipantCreateModel, Participant>().ReverseMap();
            CreateMap<Participant, ParticipantModel>().ReverseMap();
            CreateMap<ParticipantUpdateModel, Participant>().ReverseMap();
            CreateMap<MiscPaymentCreateModel, MiscPayments>();
            CreateMap<MiscPaymentUpdateModel, MiscPayments>()
                .ForMember(dest => dest.Amount, src => src.MapFrom(d => d.PaidAmount))
                .ForMember(dest => dest.PaymentDate, src => src.MapFrom(d => d.PaidOn));
        }
    }
}
