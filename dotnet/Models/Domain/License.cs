using System;

namespace Sabio.Models.Domain.Licenses
{
    public class License
    {
        public int Id { get; set; }
        public LookUp StateType { get; set; }
        public LookUp LicenseType { get; set; }
        public string LicenseNumber { get; set; }
        public DateTime DateExpires { get; set; }
        public int CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
