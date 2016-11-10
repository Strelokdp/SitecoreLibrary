using System.ComponentModel.DataAnnotations;

namespace SitecoreLibrary.Models
{
    public class Book
    {
        [Display(Name = "Id")]
        public int Bookid { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

    }
}