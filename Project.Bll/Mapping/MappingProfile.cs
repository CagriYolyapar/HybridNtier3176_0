using AutoMapper;
using Project.Bll.DtoClasses;
using Project.Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Bll.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AppUser, AppUserDto>().ReverseMap();
            CreateMap<AppUserProfile,AppUserProfileDto>().ReverseMap();
            CreateMap<Category,CategoryDto>().ReverseMap();
            CreateMap<Product,ProductDto>().ReverseMap();   
            CreateMap<Order,OrderDto>().ReverseMap();   
            CreateMap<OrderDetail,OrderDetailDto>().ReverseMap();
           
        }
    }
}
