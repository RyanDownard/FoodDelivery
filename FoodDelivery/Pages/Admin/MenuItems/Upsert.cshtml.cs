using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Models;
using FoodDelivery.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDelivery.Pages.Admin.MenuItems
{
    public class UpsertModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _hostingEnvironment;
        [BindProperty]
        public MenuItemVM MenuItemVM { get; set; }
        public UpsertModel(IUnitOfWork unitOfWork, IWebHostEnvironment hostingEnvironment) { _unitOfWork = unitOfWork; _hostingEnvironment = hostingEnvironment; }

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

        public IActionResult OnPost()
        {
            string webRootPath = _hostingEnvironment.WebRootPath;

            var files = HttpContext.Request.Form.Files;

            if(!ModelState.IsValid)
            {
                return Page();
            }

            if(MenuItemVM.MenuItem.ID == 0)
            {
                if (files.Any())
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(webRootPath, @"images\menuitems\");
                    var extension = Path.GetExtension(files.First().FileName);
                    var fullPath = uploadPath + fileName + extension;

                    using (var fileStream = System.IO.File.Create(fullPath))
                    {
                        files[0].CopyTo(fileStream); 
                    }

                    MenuItemVM.MenuItem.Image = @"\images\menuitems\" + fileName + extension;
                }
                _unitOfWork.MenuItem.Add(MenuItemVM.MenuItem);
            }
            else
            {
                var fromDb = _unitOfWork.MenuItem.Get(m => m.ID == MenuItemVM.MenuItem.ID, true);
                if (files.Any())
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploadPath = Path.Combine(webRootPath, @"images\menuitems\");
                    var extension = Path.GetExtension(files.First().FileName);

                    if(fromDb.Image != null)
                    {
                        var imagePath = Path.Combine(webRootPath, fromDb.Image.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }

                        var fullPath = uploadPath + fileName + extension;
                        using (var fileStream = System.IO.File.Create(fullPath))
                        {
                            files[0].CopyTo(fileStream);
                        }

                        MenuItemVM.MenuItem.Image = @"\images\menuitems\" + fileName + extension;
                    }
                }
                else
                {
                    MenuItemVM.MenuItem.Image = fromDb.Image;
                }

                _unitOfWork.MenuItem.Update(MenuItemVM.MenuItem);
            }
            _unitOfWork.Commit();
            return RedirectToPage("./Index");
        }
    }
}
