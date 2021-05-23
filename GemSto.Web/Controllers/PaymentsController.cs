using GemSto.Service;
using GemSto.Service.Stores.Contracts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace GemSto.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentStore _paymentStore;

        public PaymentsController(IPaymentStore paymentStore)
        {
            _paymentStore = paymentStore;
        }
        [Route("skip/{skip:int}/take/{take:int}")]
        [HttpGet]
        public async Task<IActionResult> GetSellerPaymentHistory(int skip = 0, int take = 75)
        {
            var result = await _paymentStore.GetPaymentHistory(skip, take);
            return Ok(result);
        }
    }
}
