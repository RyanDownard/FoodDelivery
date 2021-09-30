using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
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
    }
}
