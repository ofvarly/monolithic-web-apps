using AutoMapper;
using MovieReviewApi.DTOs;
using MovieReviewApi.Models;

namespace MovieReviewApi.Mappings
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Movie, MovieReadDto>();

            CreateMap<MovieCreateDto, Movie>();

            CreateMap<Review, ReviewReadDto>();
            
            CreateMap<ReviewCreateDto, Review>();

            CreateMap<User, UserReadDto>();
            
            CreateMap<RegisterDto, User>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.DateAdded, opt => opt.MapFrom(src => DateTime.UtcNow)); // Set the DateAdded to the current date and time in UTC
        }
    }
}