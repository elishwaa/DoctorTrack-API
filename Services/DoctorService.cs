using AutoMapper;
using DoctorTrack.DTOs;
using DoctorTrack.Models;
using DoctorTrack.Services.Interfaces;

namespace DoctorTrack.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _repository;
        private readonly IMapper _mapper;

        public DoctorService(
            IDoctorRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<DoctorResponseDTO> CreateAsync(CreateDoctorDTO dto)
        {
            var exists = await _repository.ExistsByLicenseAsync(dto.LicenseNumber);

            if (exists)
            {
                return null;
            }

            var doctor = _mapper.Map<Doctor>(dto);

            doctor.Status = GetStatus(doctor.LicenseExpiryDate);

            await _repository.CreateAsync(doctor);

            return _mapper.Map<DoctorResponseDTO>(doctor);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            doctor.IsDeleted = true;

            await _repository.UpdateAsync(doctor);

            return true;
        }

        public async Task<IEnumerable<DoctorResponseDTO>> GetAllAsync(string? search, string? status)
        {
            var doctors = await _repository.GetAllAsync(search, status);

            return _mapper.Map<IEnumerable<DoctorResponseDTO>>(doctors);
        }

        public async Task<DoctorResponseDTO?> GetByIdAsync(int id)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                return null;

            return _mapper.Map<DoctorResponseDTO>(doctor);
        }

        public async Task<bool> UpdateAsync(int id, UpdateDoctorDTO dto)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            doctor.FullName = dto.FullName;
            doctor.Email = dto.Email;
            doctor.Specialization = dto.Specialization;
            doctor.LicenseExpiryDate = dto.LicenseExpiryDate;

            doctor.Status = GetStatus(dto.LicenseExpiryDate);

            await _repository.UpdateAsync(doctor);

            return true;
        }

        public async Task<bool> UpdateStatusAsync(int id, string status)
        {
            var doctor = await _repository.GetByIdAsync(id);

            if (doctor == null)
                return false;

            doctor.Status = status;

            await _repository.UpdateAsync(doctor);

            return true;
        }
        private string GetStatus(DateTime expiryDate)
        {
            return expiryDate.Date < DateTime.UtcNow.Date
                ? "Expired"
                : "Active";
        }
    }
}
