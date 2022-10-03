using System;
using System.ComponentModel.DataAnnotations;

namespace Sabio.Models.Requests.Licenses
{
    public class LicenseAddRequest
    {
        [Required]
        [Range(1, 51)]
        public int LicenseStateId { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int LicenseTypeId { get; set; }
        [Required]
        [MinLength(1)]
        public string LicenseNumber { get; set; }
        [Required]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime DateExpires { get; set; }
        [Required]
        [Range(1,int.MaxValue)]
        public int FileId { get; set; }
    }
}
