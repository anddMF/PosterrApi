using DataAnnotationsExtensions;
using System.ComponentModel.DataAnnotations;

namespace Posterr.API.Entities
{
    public class PostBody
    {
        [Required]
        [Range(1, 3, ErrorMessage = "IdType must be valid")]
        public int IdType { get; set; }

        [Required]
        [Min(1, ErrorMessage = "IdUser must be a valid number")]
        public int IdUser { get; set; }

        [Min(1, ErrorMessage = "IdOriginalPost must be a valid number")]
        public int? IdOriginalPost { get; set; }
        public string? Content { get; set; }

        [Required]
        [Range(typeof(DateTime), "0001-01-01", "9999-12-31", ErrorMessage = "StartDate must be a valid date")]
        public DateTime PostDate { get; set; }
    }
}
