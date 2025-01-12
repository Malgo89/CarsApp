using CarsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace CarsApp.Pages.Admin
{
    public class EditModel : PageModel
    {
        private readonly CarsApp.Data.CarDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(CarsApp.Data.CarDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Car EditCar { get; set; }

        public IActionResult OnGet(int id)
        {
            EditCar = _context.Cars.Find(id);
            if (EditCar == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, IFormFile? file, bool DeleteImage)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var carToUpdate = _context.Cars.FirstOrDefault(c => c.Id == id);
            if (carToUpdate == null)
            {
                return NotFound();
            }

            // Aktualizowanie innych pól samochodu
            carToUpdate.Brand = EditCar.Brand;
            carToUpdate.Model = EditCar.Model;
            carToUpdate.Year = EditCar.Year;
            carToUpdate.Capacity = EditCar.Capacity;

            // Jeœli u¿ytkownik przes³a³ nowe zdjêcie
            if (file != null)
            {
                // SprawdŸ rozszerzenie pliku
                var fileExtension = Path.GetExtension(file.FileName);
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

                if (Array.Exists(allowedExtensions, ext => ext.Equals(fileExtension, StringComparison.OrdinalIgnoreCase)))
                {
                    // Zapisz nowy plik w folderze wwwroot/images
                    var filePath = Path.Combine(_environment.WebRootPath, "images", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    // Zapisz œcie¿kê nowego zdjêcia w bazie danych
                    carToUpdate.ImagePath = Path.Combine("images", file.FileName);
                }
                else
                {
                    // Walidacja formatu pliku
                    ModelState.AddModelError("Car.ImagePath", "Nieobs³ugiwany format pliku. Dozwolone formaty to .jpg, .jpeg, .png, .gif.");
                    return Page();
                }
            }

            // Jeœli u¿ytkownik chce usun¹æ zdjêcie
            if (DeleteImage)
            {
                var imagePath = Path.Combine(_environment.WebRootPath, carToUpdate.ImagePath);
                if (System.IO.File.Exists(imagePath))
                {
                    // Usuñ zdjêcie z folderu
                    System.IO.File.Delete(imagePath);
                }

                // Usuñ œcie¿kê do zdjêcia z bazy danych
                carToUpdate.ImagePath = null;
            }

            // Zaktualizuj samochód w bazie danych
            _context.Cars.Update(carToUpdate);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
