using AutoMapper;
using Bogus.DataSets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class TournamentMappings : Profile
    {
        public TournamentMappings() 
        {
            CreateMap<Tournament.Core.Entities.Tournament, TournamentDto>()
                .ForMember(
                dest => dest.Title,
                from => from.MapFrom(
                    c => string.IsNullOrEmpty(c.Title) ?
                    string.Empty :
                    c.Title));

            CreateMap<TournamentDto, Tournament.Core.Entities.Tournament>();

            CreateMap<GameDto, Game>()
                .ForMember(
                dest => dest.Time,
                from => from.MapFrom(
                    c => c.StartDate));

            CreateMap<Game, GameDto>()
                .ForMember(
                dest => dest.StartDate,
                from => from.MapFrom(
                    c => c.Time));

            CreateMap<Tournament.Core.Entities.Tournament, TournamentForUpdateDto>()
                .ForMember(
                dest => dest.TournamentId,
                from => from.MapFrom(
                    c => c.Id));

            
            CreateMap<TournamentForUpdateDto, Tournament.Core.Entities.Tournament>();

            CreateMap<Game, GameForUpdateDto>()
                .ForMember(
                dest => dest.GameId,
                from => from.MapFrom(
                    c => c.Id));

            CreateMap<GameForUpdateDto, Game>()
                .ForMember(
                dest => dest.Id,
                from => from.MapFrom(
                    c => c.GameId));
        }
    }
}
