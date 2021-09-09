using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.FoodTypes
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public FoodType FoodType { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public IActionResult OnGet(int? id)
        {
            FoodType = id == 0 ? new FoodType() : _unitOfWork.FoodType.Get(i => i.ID == id);
            if (FoodType == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (FoodType.ID == 0)
            {
                _unitOfWork.FoodType.Add(FoodType);
            }
            else
            {
                _unitOfWork.FoodType.Update(FoodType);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
