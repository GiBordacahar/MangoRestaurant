using Mango.services.ShoppingCartAPI.Models.Dtos;
using Mango.services.ShoppingCartAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController : Controller
    {
        private ICartRepository cartRepository { get; set; }
        protected ResponseDto responseDto { get; set; }

        public CartController(ICartRepository cartRepository)
        {
            this.responseDto = new ResponseDto();
            this.cartRepository = cartRepository;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDto cartDto = await cartRepository.GetCartByUserId(userId);
                responseDto.Result = cartDto;
            }
            catch(Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { e.ToString() };
            }
            return responseDto;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart([FromBody]CartDto cartDto)
        {
            try
            {
                CartDto cartDb = await cartRepository.CreateUpdateCart(cartDto);
                responseDto.Result = cartDb;
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { e.ToString() };
            }
            return responseDto;
        }
        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart([FromBody]CartDto cartDto)
        {
            try
            {
                CartDto cartDb = await cartRepository.CreateUpdateCart(cartDto);
                responseDto.Result = cartDb;
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { e.ToString() };
            }
            return responseDto;
        }
        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody]int cartDetailsId)
        {
            try
            {
                bool isSuccess = await cartRepository.RemoveFromCart(cartDetailsId);
                responseDto.Result = isSuccess;
            }
            catch (Exception e)
            {
                responseDto.IsSuccess = false;
                responseDto.ErrorMessages = new List<string>() { e.ToString() };
            }
            return responseDto;
        }
    }
}
