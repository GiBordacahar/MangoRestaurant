using AutoMapper;
using Mango.services.ProductAPI.DbContexts;
using Mango.services.ProductAPI.Models;
using Mango.services.ProductAPI.Models.Dto;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.ProductAPI.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext db;
        private IMapper mapper;

        public ProductRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }
        public async Task<ProductDto> CreateUpdateProduct(ProductDto productDto)
        {
            Product product = mapper.Map<ProductDto, Product>(productDto);
            if (product.ProductId > 0)
            {
                db.Products.Update(product);
            }
            else
            {
                db.Products.Add(product);
            }
            await db.SaveChangesAsync();
            return mapper.Map<Product, ProductDto>(product);
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await db.Products.FirstOrDefaultAsync(u => u.ProductId == productId);
                if (product == null)
                {
                    return false;
                }
                db.Products.Remove(product);
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        public async Task<ProductDto> GetProductById(int productId)
        {
            Product product = await db.Products.Where(p => p.ProductId == productId).FirstOrDefaultAsync();
            return mapper.Map<Product, ProductDto>(product);
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            IEnumerable<Product> productList = await db.Products.ToListAsync();
            return mapper.Map<IEnumerable<Product>, IEnumerable<ProductDto>>(productList);
        }
    }
}
