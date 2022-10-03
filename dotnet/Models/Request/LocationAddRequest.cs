using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Location
{
    public class LocationAddRequest
    {
        [Required]
        [Range(1,int.MaxValue)]
        public int LocationTypeId { get; set; }
        [Required]
        [MaxLength(255)]
        public string LineOne { get; set; }
        [MaxLength(255)]
        public string LineTwo { get; set; }
        [Required]
        [MaxLength(255)]
        public string City { get; set; }
        [MaxLength(50)]
        public string Zip { get; set; }
        [Required]
        [Range(1, int.MaxValue)]
        public int StateId { get; set; }
        [Required]
        [Range(-90,90)]
        public double Latitude { get; set; }
        [Required]
        [Range(-180,180)]
        public double Longitude { get; set; }
    }
}
