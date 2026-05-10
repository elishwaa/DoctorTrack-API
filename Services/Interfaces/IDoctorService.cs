using DoctorTrack.DTOs;

namespace DoctorTrack.Services.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorResponseDTO>> GetAllAsync(string? search, string? status);

        Task<DoctorResponseDTO?> GetByIdAsync(int id);

        Task<DoctorResponseDTO> CreateAsync(CreateDoctorDTO dto);

        Task<bool> UpdateAsync(int id, UpdateDoctorDTO dto);

        Task<bool> UpdateStatusAsync(int id, string status);

        Task<bool> DeleteAsync(int id);
    }
}
