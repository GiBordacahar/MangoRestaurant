using Mango.services.ProductAPI.Models.Dto;
using Mango.services.ProductAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.ProductAPI.Controllers
{
    [Route("api/products")]
    public class ProductAPIController : ControllerBase
    {
        protected ResponseDto response { get; set; }
        private IProductRepository productRepository { get; set; }

        public ProductAPIController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
            this.response = new ResponseDto();
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ResponseDto> Get()
        {
            try
            {
                IEnumerable<ProductDto> productDtos = await productRepository.GetProducts();
                response.Result = productDtos;
            }
            catch(Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString()};
            }
            return response;
        }
        [HttpGet]
        [Authorize]
        [Route("{id}")]
        public async Task<ResponseDto> Get(int id)
        {
            try
            {
                ProductDto productDto = await productRepository.GetProductById(id);
                response.Result = productDto;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }
            return response;
        }
        [HttpPost]
        [Authorize]
        public async Task<object> Post([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto dbProduct = await productRepository.CreateUpdateProduct(productDto);
                response.Result = dbProduct;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }
            return response;
        }

        [HttpPut]
        [Authorize]
        public async Task<object> Put([FromBody] ProductDto productDto)
        {
            try
            {
                ProductDto dbProduct = await productRepository.CreateUpdateProduct(productDto);
                response.Result = dbProduct;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }
            return response;
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [Route("{id}")]
        public async Task<object> Delete(int id)
        {
            try
            {
                bool isSuccess = await productRepository.DeleteProduct(id);
                response.Result = isSuccess;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { e.ToString() };
            }
            return response;
        }

    }
}
