using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarsApp.Pages.Cars
{
    public class ReserveModel : PageModel
    {
        private readonly CarsApp.Data.CarDbContext _context;

        public ReserveModel(CarsApp.Data.CarDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
    }
}
