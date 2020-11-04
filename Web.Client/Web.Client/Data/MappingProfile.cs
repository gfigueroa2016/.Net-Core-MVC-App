﻿using AutoMapper;
using Web.Client.Models;
using System.Security.Claims;

namespace Web.Client.Data
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegistration, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email));

            CreateMap<ExternalLogin, User>()
                .ForMember(u => u.UserName, opt => opt.MapFrom(x => x.Email))
                .ForMember(u => u.FirstName, opt => opt.MapFrom(x => x.Principal.FindFirst(ClaimTypes.GivenName).Value))
                .ForMember(u => u.LastName, opt => opt.MapFrom(x => x.Principal.FindFirst(ClaimTypes.Surname).Value));
        }
    }
}
