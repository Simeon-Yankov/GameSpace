using AutoMapper;

using GameSpace.Data.Models;
using GameSpace.Models.Messages;
using GameSpace.Models.SocialNetworks;
using GameSpace.Models.Teams;
using GameSpace.Models.User;
using GameSpace.Services.Messages.Modles;
using GameSpace.Services.SocialNetworks.Models;
using GameSpace.Services.Sumonners.Models;
using GameSpace.Services.Teams.Models;
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
            this.CreateMap<Region, SummonerRegionServiceModel>();
        }
    }
}