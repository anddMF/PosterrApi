using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Posterr.API.Entities
{
    public class PostQueryParameters
    {
        [Required]
        public bool GetAll { get; set; }

        [Required]
        public int PageNumber { get; set; }

        [Required]
        public int PageSize { get; set; }

        [Min(1, ErrorMessage = "IdUser must be a valid number")]
        public int? UserId { get; set; }

        [Range(typeof(DateTime), "0001-01-01", "9999-12-31", ErrorMessage = "StartDate must be a valid date")]
        public DateTime StartDate { get; set; }

        [Range(typeof(DateTime), "0001-01-01", "9999-12-31", ErrorMessage = "EndDate must be a valid date")]
        public DateTime? EndDate { get; set; }
    }
}
