namespace GameSpace.Models.Teams
{
    public class TeamViewModel
    {
        public int Id { get; init; }

        public string Name { get; init; }

        public byte[] Image { get; init; }

        public bool IsDefaultImage { get; init; }
    }
}