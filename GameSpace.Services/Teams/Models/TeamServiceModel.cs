namespace GameSpace.Services.Teams.Models
{
    public class TeamServiceModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public byte[] Image { get; init; }

        public byte[] Banner { get; init; }

        //public bool IsCheckedIn { get; init; }

        //public bool IsEliminated { get; init; }

        public bool IsDefaultImage => Image is null ? true : false;
    }
}