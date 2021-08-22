using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using GameSpace.Services.Sumonners.Models;

using static GameSpace.Common.GlobalConstants.GamingAccount;

namespace GameSpace.Models.Summoners
{
    public class AddSummonerFormModel
    {
        [Display(Name = "Summoner Name")]
        [Required]
        [StringLength(SummonerNameMaxLength, MinimumLength = SummonerNameMinLength)]
        public string Name { get; init; }

        [Display(Name = "Region")]
        public int RegionId { get; init; }

        public List<SummonerRegionServiceModel> Regions { get; set; }
    }
}