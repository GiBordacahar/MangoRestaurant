﻿using Mango.web.Models;
using Mango.web.Services.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.web.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IHttpClientFactory httpClient;
        public CartService(IHttpClientFactory httpClient) : base(httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<T> AddToCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new APIRequest
            {
                ApyType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            return await this.SendAsync<T>(new APIRequest
            {
                ApyType = SD.ApiType.GET,
                Url = SD.ShoppingCartAPIBase + "/api/cart/GetCart/"+userId,
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartDetailsId, string token = null)
        {
            return await this.SendAsync<T>(new APIRequest
            {
                ApyType = SD.ApiType.DELETE,
                Data = cartDetailsId,
                Url = SD.ShoppingCartAPIBase + "/api/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDto cartDto, string token = null)
        {
            return await this.SendAsync<T>(new APIRequest
            {
                ApyType = SD.ApiType.POST,
                Data = cartDto,
                Url = SD.ShoppingCartAPIBase + "/api/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}