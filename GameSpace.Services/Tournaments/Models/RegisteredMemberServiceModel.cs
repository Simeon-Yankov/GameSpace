namespace GameSpace.Services.Tournaments.Models
{
    public class RegisteredMemberServiceModel
    {
        public int TeamTournamentId { get; init; }

        public string UserId { get; init; }

        public bool IsChecked { get; init; }
    }
}