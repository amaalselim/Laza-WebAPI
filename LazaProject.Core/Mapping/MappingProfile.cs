using AutoMapper;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using Microsoft.AspNetCore.Http;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<RegisterDTO, ApplicationUser>()
			.ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

		CreateMap<Product, ProductDTO>()
			.ForMember(dest => dest.ProductType, opt => opt.MapFrom(src => src.type));

		CreateMap<ProductDTO, Product>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
			.ForMember(dest => dest.Images, opt => opt.MapFrom(src => MapImages(src.Images)))
			.ForMember(dest => dest.type, opt => opt.MapFrom(src => src.ProductType));

		CreateMap<WishListItem, WishListItemDTO>()
			.ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
			.ForMember(dest => dest.ProductPrice, opt => opt.MapFrom(src => src.Product.Price));

		CreateMap<WishListItemDTO, WishListItem>();
		CreateMap<ReviewDTO, Reviews>();
		CreateMap<Reviews, ReviewDTO>();
	}

	private List<productImage> MapImages(IEnumerable<IFormFile> images) 
	{
		return images.Select(img => new productImage { Image = img.FileName }).ToList(); 
	}
}
