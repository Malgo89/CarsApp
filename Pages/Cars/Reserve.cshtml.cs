using CarsApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarsApp.Pages.Cars
{
    public class ReserveModel : PageModel
    {
        private readonly CarsApp.Data.CarDbContext _context;

        public ReserveModel(CarsApp.Data.CarDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Reservation Reservation { get; set; }

        public List<Car> Cars { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            ViewData["CarId"] = car.Id;
            ViewData["CarBrand"] = car.Brand;
            ViewData["CarModel"] = car.Model;
            ViewData["CarYear"] = car.Year;

            Reservation = new Reservation
            {
                CarId = car.Id,
                StartDate = DateTime.Now, // ustaw datê pocz¹tkow¹ na dzieñ dzisiejszy
                EndDate = DateTime.Now.AddDays(1) // ustaw datê koñcow¹ na dzieñ nastêpny
            };

            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == Reservation.CarId);
                if (car != null)
                {
                    ViewData["CarId"] = car.Id;
                    ViewData["CarBrand"] = car.Brand;
                    ViewData["CarModel"] = car.Model;
                    ViewData["CarYear"] = car.Year;
                }
                return Page();
            }

            // Sprawdzenie dostêpnoœci samochodu w podanym okresie
            var overlappingReservations = await _context.Reservations
                .Where(r => r.CarId == Reservation.CarId &&
                            ((r.StartDate <= Reservation.EndDate && r.EndDate >= Reservation.StartDate)))
                .ToListAsync();

            if (overlappingReservations.Any())
            {
                // Jeœli s¹ kolizje w terminach, poka¿ komunikat o b³êdzie
                ModelState.AddModelError(string.Empty, "Samochód jest ju¿ zarezerwowany w wybranym terminie.");
                var car = await _context.Cars.FirstOrDefaultAsync(c => c.Id == Reservation.CarId);
                if (car != null)
                {
                    ViewData["CarId"] = car.Id;
                    ViewData["CarBrand"] = car.Brand;
                    ViewData["CarModel"] = car.Model;
                    ViewData["CarYear"] = car.Year;
                }
                return Page();
            }

            _context.Reservations.Add(Reservation);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }




    }

}
