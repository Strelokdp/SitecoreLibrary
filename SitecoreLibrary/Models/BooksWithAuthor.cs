using System.ComponentModel.DataAnnotations;

namespace SitecoreLibrary.Models
{
    public class BooksWithAuthor
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string AuthorName { get; set; }
    }
}