using System;
using System.ComponentModel.DataAnnotations;

namespace CarsApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public int CarId { get; set; } // Powiązanie z samochodem

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required, StringLength(50)]
        public string CustomerName { get; set; }

        [Required, StringLength(50)]
        public string CustomerPhone { get; set; }

        // Nawigacja do modelu Car
        public Car? Car { get; set; }
    }
}
