using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using FoodDelivery.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDelivery.Pages.Admin.MenuItems
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public MenuItemVM MenuItemVM { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;

        public IActionResult OnGet(int? ID)
        {
            var categories = _unitOfWork.Category.List();
            var footTypes = _unitOfWork.FoodType.List();
            MenuItemVM = new MenuItemVM()
            {
                MenuItem = ID.HasValue && ID > 0 ? _unitOfWork.MenuItem.Get(x => x.ID == ID, true) : new MenuItem(),
                CategoryList = categories.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name }),
                FoodTypeList = footTypes.Select(x => new SelectListItem { Value = x.ID.ToString(), Text = x.Name })
            };
            return Page();
        }
    }
}
