namespace Mediar.Models
{
    public class Album
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public required string CoverImageUrl { get; set; }

        //public int UserId { get; set; }
        //public virtual User? User { get; set; }
    }
}