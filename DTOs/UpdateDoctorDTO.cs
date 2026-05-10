namespace DoctorTrack.DTOs
{
    public class UpdateDoctorDTO
    {
        public string FullName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Specialization { get; set; } = string.Empty;

        public DateTime LicenseExpiryDate { get; set; }
    }
}
