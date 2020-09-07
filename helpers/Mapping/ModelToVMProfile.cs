using AutoMapper;
using identity.Models;
using identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace identity.Mapping
{
    public class ModelToVMProfile : Profile
    {

        public ModelToVMProfile()
        {
            // User
            CreateMap<ApplicationUser, UserSignUpVM>(MemberList.Destination);
            CreateMap<ApplicationUser, UserSignInVM>(MemberList.Destination);
            CreateMap<ApplicationUser, UserVM>(MemberList.Destination);

        }
    }
}