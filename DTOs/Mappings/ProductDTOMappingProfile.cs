using apicatalogo.Models;
using AutoMapper;

namespace ApiCatalogo.DTOs.Mappings
{
    public class ProductDTOMappingProfile : Profile
    {
        public ProductDTOMappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Category, CategoryRequest>().ReverseMap();
        }

    }
}