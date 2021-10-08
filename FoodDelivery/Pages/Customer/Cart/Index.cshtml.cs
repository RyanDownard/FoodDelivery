using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using FoodDelivery.ViewModels;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Cart
{
    public class IndexModel : PageModel
    {
        IUnitOfWork _unitOfWork;
        public OrderDetailsCartVM OrderDetailsCart { get; set; }

        public IndexModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public void OnGet()
        {
            OrderDetailsCart = new OrderDetailsCartVM
            {
                OrderHeader = new OrderHeader(),
                ListCart = new List<ShoppingCart>()
            };

            OrderDetailsCart.OrderHeader.OrderTotal = 0;
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == claim.Value);
                if(cart != null)
                {
                    OrderDetailsCart.ListCart = cart.ToList();
                }

                foreach(var cartList in OrderDetailsCart.ListCart)
                {
                    cartList.MenuItem = _unitOfWork.MenuItem.Get(i => i.ID == cartList.MenuItemID);
                    OrderDetailsCart.OrderHeader.OrderTotal += (cartList.MenuItem.Price * cartList.Count);
                }
            }
        }

        public IActionResult OnPostMinus(int CartID)
        {
            var cart = _unitOfWork.ShoppingCart.Get(i => i.ID == CartID);
            if(cart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Delete(cart);
            }
            else
            {
                cart.Count--;
                _unitOfWork.ShoppingCart.Update(cart);
            }

            _unitOfWork.Commit();
            var cartCount = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == cart.ApplicationUserID).Count();

            HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, cartCount);
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostPlus(int CartID)
        {
            var cart = _unitOfWork.ShoppingCart.Get(i => i.ID == CartID);
            cart.Count++;
            _unitOfWork.ShoppingCart.Update(cart);

            _unitOfWork.Commit();
            var cartCount = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == cart.ApplicationUserID).Count();

            HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, cartCount);
            return RedirectToPage("/Customer/Cart/Index");
        }

        public IActionResult OnPostRemove(int CartID)
        {
            var cart = _unitOfWork.ShoppingCart.Get(i => i.ID == CartID);
            _unitOfWork.ShoppingCart.Delete(cart);

            _unitOfWork.Commit();
            var cartCount = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == cart.ApplicationUserID).Count();

            HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, cartCount);
            return RedirectToPage("/Customer/Cart/Index");
        }
    }
}
