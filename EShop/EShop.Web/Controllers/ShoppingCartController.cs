//using EShop.Web.Data;
//using EShop.Domain.DomainModels;
//using EShop.Domain.DTO;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Threading.Tasks;

//namespace EShop.Web.Controllers
//{
//    public class ShoppingCartController : Controller
//    {


//        private readonly ApplicationDbContext _context;

//        public ShoppingCartController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IActionResult> Index()
//        {
//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            //Select * from Users Where Id LIKE userId

//            var loggedInUser = await _context.Users.Where(z => z.Id == userId)
//                .Include("UserCart")
//                .Include("UserCart.ProductInShoppingCarts")
//                .Include("UserCart.ProductInShoppingCarts.Product")
//                .FirstOrDefaultAsync();

//            var userShoppingCart = loggedInUser.UserCart;

//            var AllProducts = userShoppingCart.ProductInShoppingCarts.ToList();

//            var allProductPrice = AllProducts.Select(z => new
//            {
//                ProductPrice = z.Product.ProductPrice,
//                Quanitity = z.Quantity
//            }).ToList();

//            var totalPrice = 0;


//            foreach (var item in allProductPrice)
//            {
//                totalPrice += item.Quanitity * item.ProductPrice;
//            }


//            ShoppingCartDto scDto = new ShoppingCartDto
//            {
//                Products = AllProducts,
//                TotalPrice = totalPrice
//            };


//            return View(scDto);
//        }

//        public async Task<IActionResult> DeleteFromShoppingCart(Guid id)
//        {
//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            if(!string.IsNullOrEmpty(userId) && id != null)
//            {
//                //Select * from Users Where Id LIKE userId

//                var loggedInUser = await _context.Users.Where(z => z.Id == userId)
//                    .Include("UserCart")
//                    .Include("UserCart.ProductInShoppingCarts")
//                    .Include("UserCart.ProductInShoppingCarts.Product")
//                    .FirstOrDefaultAsync();

//                var userShoppingCart = loggedInUser.UserCart;

//                var itemToDelete = userShoppingCart.ProductInShoppingCarts.Where(z => z.ProductId.Equals(id)).FirstOrDefault();

//                userShoppingCart.ProductInShoppingCarts.Remove(itemToDelete);

//                _context.Update(userShoppingCart);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToAction("Index", "ShoppingCart");
//        }

//        public async Task<IActionResult> Order()
//        {
//            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

//            if (!string.IsNullOrEmpty(userId))
//            {
//                //Select * from Users Where Id LIKE userId

//                var loggedInUser = await _context.Users.Where(z => z.Id == userId)
//                    .Include("UserCart")
//                    .Include("UserCart.ProductInShoppingCarts")
//                    .Include("UserCart.ProductInShoppingCarts.Product")
//                    .FirstOrDefaultAsync();

//                var userShoppingCart = loggedInUser.UserCart;

//                Order order = new Order
//                {
//                    Id = Guid.NewGuid(),
//                    User = loggedInUser,
//                    UserId = userId
//                };

//                _context.Add(order);
//                await _context.SaveChangesAsync();

//                List<ProductInOrder> productInOrders = new List<ProductInOrder>();

//                var result = userShoppingCart.ProductInShoppingCarts.Select(z => new ProductInOrder
//                {
//                    ProductId = z.Product.Id,
//                    OrderedProduct = z.Product,
//                    OrderId = order.Id,
//                    UserOrder = order
//                }).ToList();

//                productInOrders.AddRange(result);

//                foreach (var item in productInOrders)
//                {
//                    _context.Add(item);
//                }
//                await _context.SaveChangesAsync();

//                loggedInUser.UserCart.ProductInShoppingCarts.Clear();

//                _context.Update(loggedInUser);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToAction("Index", "ShoppingCart");
//        }
//    }
//}
