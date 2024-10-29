using AutoMapper;
using LazaProject.Core.DTO_S;
using LazaProject.Core.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LazaProject.Core.Mapping
{
	public class MappingProfile : Profile
	{
        public MappingProfile()
        {
            CreateMap<RegisterDTO, ApplicationUser>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

			CreateMap<Product, ProductDTO>();
			CreateMap<ProductDTO, Product>()
			.ForMember(dest => dest.Id, opt => opt.Ignore()) // إذا كنت تستخدم Guid.NewGuid() بشكل افتراضي
			.ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
			.ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
			.ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.Images.Select(img => new productImage { Image = img }).ToList()));

		}


	}
}
