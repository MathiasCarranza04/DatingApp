using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }

        public int AppUserId { get; set; }  // se relaciona con AppUser entity,va a ser la clave foranea
        public AppUser AppUser { get; set; }
    }
}