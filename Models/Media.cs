namespace Mediar.Models
{
    public class Media
    {
        public int Id { get; set; }
        public required string Url { get; set; }

        public int AlbumId { get; set; }
        public virtual Album? Album { get; set; }
    }
}