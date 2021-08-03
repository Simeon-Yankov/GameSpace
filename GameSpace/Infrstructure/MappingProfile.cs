using AutoMapper;

using GameSpace.Models.Messages;
using GameSpace.Models.Teams;
using GameSpace.Services.Messages.Modles;

namespace GameSpace.Infrstructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<TeamInvitationMessageServiceModel, TeamInvitationMessageViewModel>();
            this.CreateMap<int, InviteTeamFormModel>();

        }
    }
}
