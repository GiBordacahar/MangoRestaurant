using AutoMapper;
using Mango.services.CouponAPI.DbContexts;
using Mango.services.CouponAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.CouponAPI.Repository
{
    public class CouponRepository : ICouponRepository
    {
        private readonly ApplicationDbContext db;
        protected IMapper mapper;

        public CouponRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<CouponDto> GetCouponByCode(string couponCode)
        {
            var couponFromDb = await db.Coupons.FirstOrDefaultAsync(c => c.CouponCode == couponCode);
            return mapper.Map<CouponDto>(couponFromDb);
        }
    }
}
