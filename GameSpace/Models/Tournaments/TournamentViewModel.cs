using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using GameSpace.Services.Regions.Models;
using GameSpace.Services.Teams.Models;
using GameSpace.Services.Tournaments.Models;

using Microsoft.AspNetCore.Mvc;

namespace GameSpace.Models.Tournaments
{
    public class TournamentViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public string Information { get; init; }

        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mmZ}", ApplyFormatInEditMode = true)]
        public DateTime StartsOn { get; set; }

        public decimal PrizePool { get; init; }

        public decimal TicketPrize { get; init; }

        public int CheckInPeriod { get; init; }

        public int GoToGamePeriod { get; init; }

        public int RegionId { get; init; }

        public string RegionName { get; init; }

        public IEnumerable<RegionServiceModel> Regions { get; set; }

        public int BracketTypeId { get; init; }

        public string BracketTypeFormat { get; init; }

        public IEnumerable<BracketTypeServiceModel> BracketTypes { get; set; }

        public int MaximumTeamsId { get; init; }

        public int MaximumTeams { get; init; }

        public IEnumerable<MaximumTeamsFormatServiceModle> MaximumTeamsFormats { get; set; }

        public int MinimumTeams { get; init; }

        public int TeamSizeId { get; init; }

        public string TeamSizeFormat { get; init; }

        public IEnumerable<TeamSizeServiceModel> TeamSizes { get; set; }

        public int MapId { get; init; }

        public string MapName { get; init; }

        public IEnumerable<MapServiceModel> Maps { get; set; }

        public int ModeId { get; init; }

        public string ModeName { get; init; }

        public IEnumerable<ModeServiceModel> Modes { get; set; }

        public bool BronzeMatch { get; init; }

        public bool IsVerified { get; set; }

        public string HosterName { get; init; }

        public string StartsInMessage { get; init; }

        public bool IsRegistrated { get; set; }

        public bool IsUserChecked { get; set; }

        public bool IsHoster { get; set; }

        public bool IsTeamChecked { get; set; }

        public IEnumerable<TeamServiceModel> Participants { get; set; }

        public IEnumerable<RegisteredMemberServiceModel> RegisteredMembers { get; set; }
    }
}