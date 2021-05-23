using GemSto.Data;
using GemSto.Service;
using GemSto.Service.Contracts;
using GemSto.Service.Security;
using GemSto.Service.Stores;
using GemSto.Service.Stores.Contracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GemSto.Web
{
    public static class ServiceInjector
    {
        public static void InjectServices(IServiceCollection services)
        {
            services.AddScoped<IGemService, GemService>();
            services.AddScoped<ISellerLookUpService, SellerLookUpService>();
            services.AddScoped<ISellerPaymentService, SellerPaymentService>();
            services.AddScoped<ISalesService, SalesService>();
            services.AddScoped<ISalePaymentsService, SalePaymentsService>();
            services.AddScoped<IPaymentStore, PaymentStore>();
            services.AddScoped<IMiscCategoryLookUpService, MiscCategoryLookUpService>();
            services.AddScoped<IParticipantLookUpService, ParticipantLookUpService>();
            services.AddScoped<IMiscPaymentService, MiscPaymentService>();
            services.AddScoped<IParticipantLookUpFacadeService, ParticipantLookUpFacadeService>();
        }
    }
}
