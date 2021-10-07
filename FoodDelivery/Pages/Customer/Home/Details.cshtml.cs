using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Customer.Home
{
    public class DetailsModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        public DetailsModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        [BindProperty]
        public ShoppingCart ShoppingCartObj { get; set; }


        public async Task OnGetAsync(int ID)
        {
            ShoppingCartObj = new ShoppingCart()
            {
                MenuItem = await _unitOfWork.MenuItem.GetAsync(i => i.ID == ID, false, "Category,FoodType")
            };

            ShoppingCartObj.MenuItemID = ID;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)this.User.Identity;
                var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
                ShoppingCartObj.ApplicationUserID = claim.Value;

                ShoppingCart cartFromDb = await _unitOfWork.ShoppingCart.GetAsync(i => i.ApplicationUserID == ShoppingCartObj.ApplicationUserID && i.MenuItemID == ShoppingCartObj.MenuItemID);
                if(cartFromDb == null)
                {
                    _unitOfWork.ShoppingCart.Add(ShoppingCartObj);
                }
                else
                {
                    cartFromDb.Count += ShoppingCartObj.Count;
                }
                await _unitOfWork.CommitAsync();

                var count = _unitOfWork.ShoppingCart.List(i => i.ApplicationUserID == ShoppingCartObj.ApplicationUserID).Count();
                HttpContext.Session.SetInt32(StaticDetails.ShoppingCart, count);
                return RedirectToPage("Index");
            }
            else
            {
                ShoppingCartObj.MenuItem = _unitOfWork.MenuItem.Get(i => i.ID == ShoppingCartObj.MenuItemID, false, "Category,FoodType");
            }
            return Page();
        }
    }
}
