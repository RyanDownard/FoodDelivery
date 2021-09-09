using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDelivery.Pages.Admin.Categories
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        [BindProperty]
        public Category Category { get; set; }

        public UpsertModel(IUnitOfWork unitOfWork) => _unitOfWork = unitOfWork;
        public IActionResult OnGet(int? id)
        {
            Category = id == 0 ? new Category() : _unitOfWork.Category.Get(i => i.ID == id);
            if (Category == null)
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

            if(Category.ID == 0)
            {
                _unitOfWork.Category.Add(Category);
            }
            else
            {
                _unitOfWork.Category.Update(Category);
            }

            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
