using Mango.web.Models;
using Mango.web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.web.Services
{
    public class CouponService : BaseService, ICouponService
    {
        private readonly IHttpClientFactory httpClient;
        public CouponService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this.httpClient = httpClient;
        }
        public async Task<T> GetCoupon<T>(string couponCode, string token = null)
        {
            return await this.SendAsync<T>(new APIRequest
            {
                ApyType = SD.ApiType.GET,
                Url = SD.CouponAPIBase + "/api/coupon/GetDiscount/" + couponCode,
                AccessToken = token
            });
        }
    }
}
