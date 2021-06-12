using AutoMapper;
using GameCenter.DTOs;
using GameCenter.Models;

namespace GameCenter.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genre, GenreDTO>().ReverseMap();
            CreateMap<GenreCreationDTO, Genre>();
            CreateMap<Person, PersonDTO>().ReverseMap();
            CreateMap<PersonCreationDTO, Person>()
                .ForMember(x => x.Picture, options => options.Ignore());
            CreateMap<Person, PersonPatchDTO>().ReverseMap();
            CreateMap<Game, GameDTO>().ReverseMap();
            CreateMap<GameCreationDTO, Game>()
                .ForMember(x => x.Poster, options => options.Ignore());
            CreateMap<Game, GamePatchDTO>().ReverseMap();
        }
    }
}