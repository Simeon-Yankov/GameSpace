using AutoMapper;

using GameSpace.Models.Messages;
using GameSpace.Models.SocialNetworks;
using GameSpace.Models.Teams;
using GameSpace.Models.User;
using GameSpace.Services.Messages.Modles;
using GameSpace.Services.SocialNetworks.Models;
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
            this.CreateMap<SocialNotworkServiceModel, SocialNetworkViewModel>();
            this.CreateMap<UserProfileServiceModel, EditUserFormModel>();
                //.ForMember(f => f.Appearance, cfg => cfg.MapFrom(s => s.Appearance))
                //.ForMember(f => f.SocialNetwork, cfg => cfg.MapFrom(s => s.SocialNetwork));

                //.ForMember(e => e.SocialNetwork.FacebookUrl, cfg => cfg.MapFrom(u => u.SocialNetwork.FacebookUrl))
                //.ForMember(e => e.SocialNetwork.TwitchUrl, cfg => cfg.MapFrom(u => u.SocialNetwork.TwitchUrl))
                //.ForMember(e => e.SocialNetwork.TwitterUrl, cfg => cfg.MapFrom(u => u.SocialNetwork.TwitterUrl))
                //.ForMember(e => e.SocialNetwork.YoutubeUrl, cfg => cfg.MapFrom(u => u.SocialNetwork.YoutubeUrl));



            this.CreateMap<SocialNotworkServiceModel, SocialNetworkViewModel>();

        }
    }
}