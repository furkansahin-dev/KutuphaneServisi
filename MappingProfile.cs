using AutoMapper;
using KutuphaneServisi.DTOs;
using KutuphaneServisi.Models;

namespace KutuphaneServisi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Book -> BookDto eşleştirmesi
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : "Kategori Yok"));

            // BookCreateDto -> Book eşleştirmesi (Hem ekleme hem güncelleme için)
            CreateMap<BookCreateDto, Book>();
        }
    }
}