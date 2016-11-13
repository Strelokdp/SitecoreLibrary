using System;
using System.ComponentModel.DataAnnotations;

namespace SitecoreLibrary.ViewModels
{
    public class Books
    {
        [Display(Name = "Id")]
        public int Id { get; set; }
        
        public int BookRecordId { get; set; }

        [Display(Name = "Book Quantity")]
        public int BookQuantity { get; set; }

        [Display(Name = "Book name")]
        [Required(ErrorMessage = "Name is required.")]
        public string BookName { get; set; }

        [Display(Name = "Author first name")]
        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; }

        [Display(Name = "Author last name")]
        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; }

        public bool IsTaken { get; set; }

        public Guid TakenByUserId { get; set; }

        [Display(Name = "Author full name")]
        public string FullName => (FirstName + " " + LastName);
    }
}