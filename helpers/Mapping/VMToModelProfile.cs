using AutoMapper;
using identity.Models;
using identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Mapping
{
    public class VMToModelProfile : Profile
    {

        public VMToModelProfile()
        {
            // User
            CreateMap<UserSignUpVM, ApplicationUser>(MemberList.Destination);
            CreateMap<UserSignInVM, ApplicationUser>(MemberList.Destination);
            CreateMap<UserVM, ApplicationUser>(MemberList.Destination);

        }
    }
}