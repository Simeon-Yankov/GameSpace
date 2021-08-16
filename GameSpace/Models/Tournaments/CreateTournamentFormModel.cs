using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using GameSpace.Services.Regions.Models;
using GameSpace.Services.Tournaments.Models;

using Microsoft.AspNetCore.Mvc;

using static GameSpace.Common.GlobalConstants.Tournament;

namespace GameSpace.Models.Tournaments
{
    public class CreateTournamentFormModel
    {
        public int Id { get; init; }

        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        public string Information { get; init; }

        [Display(Name = "Starts On")]
        [BindProperty, DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mmZ}", ApplyFormatInEditMode = true)]
        public DateTime StartsOn { get; set; }

        [Display(Name = "Prize Pool")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal PrizePool { get; init; }

        [Display(Name = "Ticket Prize")]
        [Range(0, (double)decimal.MaxValue)]
        public decimal TicketPrize { get; init; }

        [Display(Name = "Check-In Timer")]
        [Range(CheckInPeriodMinMinutes, CheckInPeriodMaxMinutes)]
        public int CheckInPeriod { get; init; }

        [Display(Name = "Go-To-Game Timer")]
        [Range(GoToGamePeriodMinMinutes, GoToGamePeriodMaxMinutes)]
        public int GoToGamePeriod { get; init; }

        [Display(Name = "Region")]
        public int RegionId { get; init; }

        public IEnumerable<RegionServiceModel> Regions { get; set; }

        [Display(Name = "Bracket Type")]
        public int BracketTypeId { get; init; }

        public IEnumerable<BracketTypeServiceModel> BracketTypes { get; set; }

        [Display(Name = "Maximum Teams")]
        public int MaximumTeamsId { get; init; }

        public IEnumerable<MaximumTeamsFormatServiceModle> MaximumTeamsFormats { get; set; }

        [Range(TeamsMinCount, TeamsMaxCount)]
        public int MinimumTeams { get; init; }

        [Display(Name = "Team Size")]
        public int TeamSizeId { get; init; }

        public IEnumerable<TeamSizeServiceModel> TeamSizes { get; set; }

        [Display(Name = "Map")]
        public int MapId { get; init; }

        public IEnumerable<MapServiceModel> Maps { get; set; }

        [Display(Name = "Mode")]
        public int ModeId { get; init; }

        public IEnumerable<ModeServiceModel> Modes { get; set; }

        [Display(Name = "Play Bronze Match")]
        public bool BronzeMatch { get; init; }

        [Display(Name = "Is Verified")]
        public bool IsVerified { get; set; }

        [Display(Name = "Hoster Name")]
        public string HosterName { get; init; }
    }
}