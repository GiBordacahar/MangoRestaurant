using Mango.services.CouponAPI.Models.Dto;
using Mango.services.CouponAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponController : Controller
    {
        protected ResponseDto response { get; set; }
        private ICouponRepository couponRepository { get; set; }

        public CouponController(ICouponRepository couponRepository)
        {
            this.couponRepository = couponRepository;
            this.response = new ResponseDto();
        }

        [HttpGet("GetDiscount/{code}")]
        public async Task<object> GetDiscountForCode(string code)
        {
            try
            {
                var couponDto = await couponRepository.GetCouponByCode(code);
                response.Result = couponDto;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string>() { e.ToString() };
            }
            return response;
        }
    }
}
