using AutoMapper;
using BookLibraryCleanArchitecture.Application.Commands;
using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Domain.Request;

namespace BookLibraryCleanArchitecture.Application.MappingProfiles
{
    public class DtoToRequestMappingProfile : Profile
    {
        public DtoToRequestMappingProfile()
        {
            CreateMap<RegisterUserCommand, RegisterRequestDto>();
            CreateMap<RegisterRequestDto, RegisterRequest>();
            CreateMap<LoginRequestDto, LoginRequest>();
        }
    }
}
