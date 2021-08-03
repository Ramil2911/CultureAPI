using System.ComponentModel.DataAnnotations.Schema;

namespace MovieWebsite.Images.Models
{
    public class Image
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string ImageTitle { get; set; }
        public string ImageType { get; set; }
        public byte[] ImageData { get; set; }
    }
}