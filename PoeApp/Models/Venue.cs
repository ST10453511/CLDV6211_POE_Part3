﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace PoeApp.Models
{
    public class Venue
    {
        public int VenueID { get; set; }
        [Required]

        public string? VenueName { get; set; }
        [Required]
        public string? Location { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Capacity must be greater than 0")]
        public int Capacity { get; set; }
        public string? ImageUrl { get; set; }

        [NotMapped]

        public IFormFile? ImageFile { get; set; }

    }
}