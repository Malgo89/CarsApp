using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarsApp.Pages.Cars
{
    public class IndexModel : PageModel
    {
        private readonly CarsApp.Data.CarDbContext _context;
        public IndexModel(CarsApp.Data.CarDbContext context)
        {
            _context = context;
        }

        public IList<CarsApp.Models.Car>? Cars { get; set; }

        public async Task OnGetAsync()
        {
            Cars = await _context.Cars.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var contact = await _context.Cars.FindAsync(id);

            if (contact != null)
            {
                _context.Cars.Remove(contact);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage();
        }
    }
}
