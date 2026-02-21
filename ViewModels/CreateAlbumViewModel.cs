namespace Mediar.ViewModels
{
    public class CreateAlbumViewModel
    {
        public required string Name { get; set; }
        public string? Description { get; set; }

        public IFormFile? CoverImage { get; set; }
    }

}