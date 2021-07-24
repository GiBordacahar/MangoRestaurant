using AutoMapper;
using Mango.services.ShoppingCartAPI.DbContexts;
using Mango.services.ShoppingCartAPI.Models;
using Mango.services.ShoppingCartAPI.Models.Dtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.services.ShoppingCartAPI.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext db;
        private IMapper mapper;

        public CartRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<bool> ApplyCoupon(string userId, string couponCode)
        {
            var cartHeaderInDb = await db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeaderInDb != null)
            {
                cartHeaderInDb.CouponCode = couponCode;
                db.CartHeaders.Update(cartHeaderInDb);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ClearCart(string userId)
        {
            var cartHeaderInDb = await db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeaderInDb != null)
            {
                db.CartDetails.RemoveRange(db.CartDetails.Where(u => u.CartHeaderId ==
                                       cartHeaderInDb.CartHeaderId));
                db.CartHeaders.Remove(cartHeaderInDb);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<CartDto> CreateUpdateCart(CartDto cartDto)
        {
            Cart cart = mapper.Map<Cart>(cartDto);

            //Check if product exists in database, if not create it
            var productInDb = await db.Products.FirstOrDefaultAsync(u => u.ProductId == cartDto.CartDetails.FirstOrDefault().ProductId);
            if (productInDb == null)
            {
                db.Products.Add(cart.CartDetails.FirstOrDefault().Product);
                await db.SaveChangesAsync();
            }

            //check if header exists in database, if not create it
            var cartHeaderInDb = await db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == cart.CartHeader.UserId);
            if (cartHeaderInDb == null)
            {
                db.CartHeaders.Add(cart.CartHeader);
                await db.SaveChangesAsync();
                //populate HeaderId in Details
                cart.CartDetails.FirstOrDefault().CartHeaderId = cart.CartHeader.CartHeaderId;
                cart.CartDetails.FirstOrDefault().Product = null;
                //now we store the details in the db
                db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                await db.SaveChangesAsync();
            }
            else
            {
                //check if the details contains the same product
                var cartDetailsInDb = await db.CartDetails.AsNoTracking().FirstOrDefaultAsync(u => 
                                            u.ProductId == cart.CartDetails.FirstOrDefault().ProductId &&
                                            u.CartHeaderId == cartHeaderInDb.CartHeaderId);

                //if not we create the CartDetails
                if (cartDetailsInDb == null)
                {
                    cart.CartDetails.FirstOrDefault().CartHeaderId = cartHeaderInDb.CartHeaderId;
                    cart.CartDetails.FirstOrDefault().Product = null;
                    db.CartDetails.Add(cart.CartDetails.FirstOrDefault());
                    await db.SaveChangesAsync();
                }
                else
                {
                    //we just update the count
                    cart.CartDetails.FirstOrDefault().Count += cartDetailsInDb.Count;
                    db.CartDetails.Update(cart.CartDetails.FirstOrDefault());
                    await db.SaveChangesAsync();
                }
            }

            return mapper.Map<CartDto>(cart);
        }

        public async Task<CartDto> GetCartByUserId(string userId)
        {
            Cart cart = new()
            {
                CartHeader = await db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId)
            };
            cart.CartDetails = db.CartDetails.Where(u => u.CartHeaderId == cart.CartHeader.CartHeaderId).Include(p => p.Product);
            return mapper.Map<CartDto>(cart);
        }

        public async Task<bool> RemoveCoupon(string userId)
        {
            var cartHeaderInDb = await db.CartHeaders.AsNoTracking().FirstOrDefaultAsync(u => u.UserId == userId);
            if (cartHeaderInDb != null)
            {
                cartHeaderInDb.CouponCode = "";
                db.CartHeaders.Update(cartHeaderInDb);
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> RemoveFromCart(int cartDetailsId)
        {
            try
            {
                CartDetails cartDetails = await db.CartDetails.FirstOrDefaultAsync(u => u.CartDetailsId == cartDetailsId);

                int cartItemsCount = db.CartDetails.Where(c => c.CartHeaderId == cartDetails.CartHeaderId).Count();

                db.CartDetails.Remove(cartDetails);

                //if this was the only item in header we remove the header too
                if (cartItemsCount == 1)
                {
                    var cartHeaderToRemove = await db.CartHeaders.
                        FirstOrDefaultAsync(h => h.CartHeaderId == cartDetails.CartHeaderId);

                    db.CartHeaders.Remove(cartHeaderToRemove);
                }
                await db.SaveChangesAsync();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }
    }
}
