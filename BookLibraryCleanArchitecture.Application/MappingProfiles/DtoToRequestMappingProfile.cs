using AutoMapper;
using BookLibraryCleanArchitecture.Application.Dtos;
using BookLibraryCleanArchitecture.Domain.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookLibraryCleanArchitecture.Application.MappingProfiles
{
    public class DtoToRequestMappingProfile : Profile
    {
        public DtoToRequestMappingProfile()
        {
            CreateMap<RegisterRequestDto, RegisterRequest>();
        }
    }
}
