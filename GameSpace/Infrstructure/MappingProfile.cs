using AutoMapper;

using GameSpace.Models.Messages;
using GameSpace.Models.SocialNetworks;
using GameSpace.Models.Teams;
using GameSpace.Models.Tournaments;
using GameSpace.Models.User;
using GameSpace.Services.Messages.Modles;
using GameSpace.Services.Regions.Models;
using GameSpace.Services.SocialNetworks.Models;
using GameSpace.Services.Sumonners.Models;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Models;
using GameSpace.Services.Users.Models;

namespace GameSpace.Infrstructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<TeamInvitationMessageServiceModel, MessageViewModel>();
            this.CreateMap<NotificationMessageServiceModel, MessageViewModel>();
            this.CreateMap<TeamDetailsServiceModel, EditTeamFromModel>();
            //this.CreateMap<TeamDetailsServiceModel, EditTeamFromModel>();
            this.CreateMap<SocialNotworkServiceModel, SocialNetworkViewModel>();
            this.CreateMap<UserProfileServiceModel, EditUserFormModel>();
            this.CreateMap<RegionServiceModel, SummonerRegionServiceModel>();
            this.CreateMap<BracketTypeServiceModel, SummonerRegionServiceModel>();

            this.CreateMap<CreateTournamentFormModel, CreateTournamentFormModel>();
            this.CreateMap<TournamentServiceModel, CreateTournamentFormModel>();
            this.CreateMap<TournamentServiceModel, TournamentViewModel>();
            this.CreateMap<TeamServiceModel, TeamViewModel>();
        }
    }
}