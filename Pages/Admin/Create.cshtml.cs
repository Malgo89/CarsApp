using CarsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CarsApp.Pages.Admin
{
    public class CreateModel : PageModel
    {
        private readonly CarsApp.Data.CarDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(CarsApp.Data.CarDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Car Car { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Jeœli u¿ytkownik wybra³ plik
            if (file != null)
            {
                // SprawdŸ rozszerzenie pliku i inne wymagania
                var fileExtension = Path.GetExtension(file.FileName);
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (Array.Exists(allowedExtensions, ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                {
                    // Zapisz plik w folderze wwwroot/images
                    var filePath = Path.Combine(_environment.WebRootPath, "images", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Zapisz œcie¿kê pliku w bazie danych
                    Car.ImagePath = Path.Combine("images", file.FileName);
                }
                else
                {
                    // Jeœli plik ma nieobs³ugiwany format, mo¿esz dodaæ walidacjê b³êdu
                    ModelState.AddModelError("Car.ImagePath", "Nieobs³ugiwany format pliku. Dozwolone formaty to .jpg, .jpeg, .png, .gif.");
                    return Page();
                }
            }

            // Dodaj samochód do bazy danych
            _context.Cars.Add(Car);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
