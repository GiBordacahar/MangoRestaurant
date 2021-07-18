using AutoMapper;
using Mango.services.CouponAPI.Models;
using Mango.services.CouponAPI.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.CouponAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Coupon, CouponDto>().ReverseMap();
            });

            return mappingConfig;
        }
    }
}
