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
using Stripe;

namespace FoodDelivery.Pages.Customer.Cart
{
    public class SummaryModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public SummaryModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [BindProperty]
        public OrderDetailsCartVM OrderDetailsCart { get; set; }
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
            if (claim != null)
            {
                IEnumerable<ShoppingCart> cart = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == claim.Value);
                if (cart != null)
                {
                    OrderDetailsCart.ListCart = cart.ToList();
                }

                foreach (var cartList in OrderDetailsCart.ListCart)
                {
                    cartList.MenuItem = _unitOfWork.MenuItem.Get(i => i.ID == cartList.MenuItemID);
                    OrderDetailsCart.OrderHeader.OrderTotal += (cartList.MenuItem.Price * cartList.Count);
                }

                OrderDetailsCart.OrderHeader.OrderTotal += OrderDetailsCart.OrderHeader.OrderTotal * StaticDetails.SalesTaxPercent;
                ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(i => i.Id == claim.Value);
                OrderDetailsCart.OrderHeader.DeliveryName = applicationUser.FullName;
                OrderDetailsCart.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
                OrderDetailsCart.OrderHeader.DeliveryTime = DateTime.Now;
                OrderDetailsCart.OrderHeader.DeliveryDate = DateTime.Now;
            }
        }

        public IActionResult OnPost(string stripeToken)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderDetailsCart.ListCart = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == claim.Value).ToList();
            OrderDetailsCart.OrderHeader.Status = StaticDetails.PaymentStatusPending;
            OrderDetailsCart.OrderHeader.OrderDate = DateTime.Now;
            OrderDetailsCart.OrderHeader.UserID = claim.Value;
            OrderDetailsCart.OrderHeader.Status = StaticDetails.StatusSubmitted;
            OrderDetailsCart.OrderHeader.DeliveryDate = Convert.ToDateTime(OrderDetailsCart.OrderHeader.OrderDate.ToShortDateString() + " " + OrderDetailsCart.OrderHeader.DeliveryTime.ToShortTimeString());

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _unitOfWork.OrderHeader.Add(OrderDetailsCart.OrderHeader);
            _unitOfWork.Commit();

            foreach(var item in OrderDetailsCart.ListCart)
            {
                item.MenuItem = _unitOfWork.MenuItem.Get(i => i.ID == item.MenuItemID);
                OrderDetails orderDetails = new OrderDetails
                {
                    MenuItemID = item.MenuItemID,
                    OrderID = OrderDetailsCart.OrderHeader.ID,
                    Name = item.MenuItem.Name,
                    Price = item.MenuItem.Price,
                    Count = item.Count
                };
                OrderDetailsCart.OrderHeader.OrderTotal += (orderDetails.Count * orderDetails.Price) * (1 + StaticDetails.SalesTaxPercent);
                _unitOfWork.OrderDetails.Add(orderDetails);
            }

            OrderDetailsCart.OrderHeader.OrderTotal = Convert.ToDouble(String.Format("{0:.##}", OrderDetailsCart.OrderHeader.OrderTotal));
            HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, 0);
            _unitOfWork.Commit();

            if(stripeToken != null)
            {
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderDetailsCart.OrderHeader.OrderTotal * 100),
                    Currency = "usd",
                    Description = $"Order ID: {OrderDetailsCart.OrderHeader.ID}",
                    Source = stripeToken
                };

                var service = new ChargeService();
                Charge charge = service.Create(options);
                OrderDetailsCart.OrderHeader.TransactionID = charge.Id;
                if(charge.Status.ToLower() == "succeeded")
                {
                    OrderDetailsCart.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusApproved;
                }
                else
                {
                    OrderDetailsCart.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusRejected;
                }
                _unitOfWork.Commit();
            }

            return RedirectToPage("/Customer/Cart/OrderConfirmation", new { id = OrderDetailsCart.OrderHeader.ID });
        }
    }
}
