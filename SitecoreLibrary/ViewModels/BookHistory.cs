using System;
using System.ComponentModel.DataAnnotations;

namespace SitecoreLibrary.ViewModels
{
    public class BookHistory
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        [Display(Name = "Book reader")]
        public string UserName { get; set; }

        [Display(Name = "Book name")]
        public string BookName { get; set; }

        [Display(Name="Taken at")]
        public DateTime Date { get; set; }
    }
}