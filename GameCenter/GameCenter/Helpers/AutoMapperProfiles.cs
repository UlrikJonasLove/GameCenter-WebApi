using System.Collections.Generic;
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
                .ForMember(x => x.Poster, options => options.Ignore())
                .ForMember(x => x.GamesGenres, options => options.MapFrom(MapGamesGenres))
                .ForMember(x => x.GamesActors, options => options.MapFrom(MapGamesPeople));
            CreateMap<Game, GamePatchDTO>().ReverseMap();

            CreateMap<Game, GameDetailDTO>()
                .ForMember(x => x.Genre, options => options.MapFrom(MapGamesGenres))
                .ForMember(x => x.Actor, options => options.MapFrom(MapGamesPeople));
            CreateMap<Game, GamePatchDTO>().ReverseMap();
        }

        private List<GenreDTO> MapGamesGenres(Game game, GameDetailDTO gameDetailDTO)
        {
            var result = new List<GenreDTO>();
                foreach(var gamegenre in game.GamesGenres)
                {
                    result.Add(new GenreDTO() { Id = gamegenre.GenreId, Name = gamegenre.Genre.Name});
                }
                return result;
        }

        private List<ActorDTO> MapGamesPeople(Game game, GameDetailDTO gameDetailDto)
        {
            var result = new List<ActorDTO>();
            {
                foreach(var person in game.GamesActors)
                {
                    result.Add(new ActorDTO() { PersonId = person.PersonId, RoleInGame = person.RoleInGame, PersonName = person.Person.Name });
                }

                return result;
            }
        }

        private List<GamesGenres> MapGamesGenres(GameCreationDTO gameCreationDto, Game game)
        {
            var result = new List<GamesGenres>();
            
                foreach(var id in gameCreationDto.GenresIds)
                {
                    result.Add(new GamesGenres() { GenreId = id});
                }

                return result;
            
        }

        private List<GamesActors> MapGamesPeople(GameCreationDTO gameCreationDto, Game game)
        {
            var result = new List<GamesActors>();
                foreach(var person in gameCreationDto.Actors)
                {
                    result.Add(new GamesActors() { PersonId = person.PersonId, RoleInGame = person.RoleInGame});
                }

                return result;
        }
    }
}