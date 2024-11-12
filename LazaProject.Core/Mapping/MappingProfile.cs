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


		CreateMap<Product, ProductDTO>();

		CreateMap<ProductDTO, Product>()
			.ForMember(dest => dest.Id, opt => opt.Ignore())
			.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
			.ForMember(dest => dest.Images, opt => opt.MapFrom(src => MapImages(src.Images)));

		CreateMap<WishListItem, WishListItemDTO>();



		CreateMap<WishListItemDTO, WishListItem>();
		CreateMap<ReviewDTO, Reviews>();
		CreateMap<Reviews, ReviewDTO>();


		CreateMap<Cart,CartDTO>();
		CreateMap<CartDTO, Cart>();

		CreateMap<CartItem, CartItemDTO>()
			.ForMember(d => d.ProductName, o => o.MapFrom(src => src.Product.Name))
			.ForMember(d => d.ProductImg, o => o.MapFrom(src => src.Product.Img))
			.ForMember(d => d.Price, o => o.MapFrom(src => src.Product.Price));
		CreateMap<CartItemDTO, CartItem>();


		CreateMap<AddressUser, AddressDTO>();
		CreateMap<AddressDTO, AddressUser>();

		CreateMap<Card,CardDTO>();
		CreateMap<CardDTO, Card>();
	}

	private List<productImage> MapImages(IEnumerable<IFormFile> images) 
	{
		return images.Select(img => new productImage { Image = img.FileName }).ToList(); 
	}
}
