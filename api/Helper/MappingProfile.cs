using api.Dtos;
using AutoMapper;
using Core.Entities;

namespace api.Helper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Product,ProductToReturnDto>()
                .ForMember(dest=>dest.productBrand,src=>src.MapFrom(e=>e.productBrand.Name))
                .ForMember(dest => dest.ProductType, src => src.MapFrom(e => e.ProductType.Name))
                .ForMember(dest => dest.PictureUrl, src => src.MapFrom<ProductUrlResolver>());
                 CreateMap<Job, JobDto>().ReverseMap();
                 CreateMap<Department, DepartmentDto>().ReverseMap();
                 CreateMap<AddressBook, AddressBookDto>().ReverseMap()
                .ForMember(dest => dest.UserName, src => src.MapFrom(e => e.FullName))
                .ForMember(dest => dest.NormalizedUserName, src => src.MapFrom(e => e.FullName))
                .ForMember(dest => dest.EmailConfirmed, src => src.MapFrom(e => true))
                .ForMember(dest => dest.PhoneNumber, src => src.MapFrom(e => e.MobileNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed, src => src.MapFrom(e => true));
            CreateMap<AddressBook, ExportAddressExcel>().ReverseMap();
        }
    }
}
