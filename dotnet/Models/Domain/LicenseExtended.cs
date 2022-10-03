namespace Sabio.Models.Domain.Licenses
{
    public class LicenseExtended : License
    {
        public LookUp ValidationType { get; set; }
        public int ValidatedBy { get; set; }
        public string RejectMessage { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public LookUp FileUrl { get; set; }
    }
}
