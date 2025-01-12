using System.ComponentModel.DataAnnotations;

namespace CarsApp.Models
{
    public class Car
    {
        public int Id { get; set; }

        [Required, StringLength(20)]
        public string? Brand { get; set; }

        [Required, StringLength(20)]
        public string? Model { get; set; }

        public string? Year { get; set; }

        public int? Capacity { get; set; }

        public string? ImagePath { get; set; }
    }
}
