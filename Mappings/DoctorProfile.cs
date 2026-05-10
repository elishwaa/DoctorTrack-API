using DoctorTrack.DTOs;
using DoctorTrack.Models;
using AutoMapper;

namespace DoctorTrack.Mappings
{
    public class DoctorProfile : Profile
    {
        public DoctorProfile()
        {
            CreateMap<CreateDoctorDTO, Doctor>()
             .ForMember(dest => dest.LicenseExpiryDate,
                 opt => opt.MapFrom(src => src.LicenseExpiryDate))
            .ForMember(dest => dest.CreatedDate,
                 opt => opt.MapFrom(src => DateTime.UtcNow));
            

            CreateMap<UpdateDoctorDTO, Doctor>()
            .ForMember(dest => dest.LicenseExpiryDate,
                opt => opt.MapFrom(src => src.LicenseExpiryDate));

            CreateMap<UpdateDoctorDTO, Doctor>()
                .ForMember(dest => dest.LicenseExpiryDate,
                 opt => opt.MapFrom(src => src.LicenseExpiryDate));

            CreateMap<Doctor, DoctorResponseDTO>()
             .ForMember(dest => dest.LicenseExpiryDate,
                 opt => opt.MapFrom(src => src.LicenseExpiryDate))
            .ForMember(dest => dest.CreatedDate,
                 opt => opt.MapFrom(src => src.CreatedDate));
        }
    }
}
