using Mango.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.web.Services.IServices
{
    public interface ICartService : IBaseService
    {
        Task<T> GetCartByUserIdAsync<T>(string userId, string token=null);
        Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null);
        Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token = null);
    }
}
