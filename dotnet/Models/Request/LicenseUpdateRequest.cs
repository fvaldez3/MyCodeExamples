using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sabio.Models.Requests.Licenses
{
    public class LicenseUpdateRequest : LicenseAddRequest, IModelIdentifier
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [Range(1,3)]
        public int ValidationTypeId { get; set; }
        [MaxLength(4000)]
        public string RejectMessage { get; set; }
    }
}
