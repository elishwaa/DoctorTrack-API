using DoctorTrack.Models;

namespace DoctorTrack.Services.Interfaces
{
    public interface IDoctorRepository
    {
        Task<IEnumerable<Doctor>> GetAllAsync(string? search, string? status);

        Task<Doctor?> GetByIdAsync(int id);

        Task<bool> ExistsByLicenseAsync(string licenseNumber);

        Task CreateAsync(Doctor doctor);

        Task UpdateAsync(Doctor doctor);
    }
}
