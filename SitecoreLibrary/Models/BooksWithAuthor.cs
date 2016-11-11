﻿using System;
using System.ComponentModel.DataAnnotations;

namespace SitecoreLibrary.Models
{
    public class BooksWithAuthor
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

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

        public string FullName
        {
            get
            {
                return (LastName + ", " + FirstName);
            }
        }
    }
}