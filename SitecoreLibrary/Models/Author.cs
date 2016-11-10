using System.ComponentModel.DataAnnotations;

namespace SitecoreLibrary.Models
{
    public class Author
    {
        [Display(Name = "Id")]
        public int Authorid { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
    }
}