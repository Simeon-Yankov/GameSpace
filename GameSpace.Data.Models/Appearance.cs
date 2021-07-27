namespace GameSpace.Data.Models
{
    public class Appearance
    {
        public int Id { get; init; }

        public byte[] Image { get; set; }

        public byte[] Banner { get; set; }

        public Team Team { get; init; }
    }
}