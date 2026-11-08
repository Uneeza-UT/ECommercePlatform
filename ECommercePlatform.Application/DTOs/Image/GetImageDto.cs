namespace ECommercePlatform.Application.DTOs.Image
{
    public class GetImageDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string ContentType { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
