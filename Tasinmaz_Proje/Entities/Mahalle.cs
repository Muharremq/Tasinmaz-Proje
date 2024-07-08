using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tasinmaz_Proje.Entities
{
    public class Mahalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int IlceId { get; set; }
        [ForeignKey("IlceId")]
        public Ilce Ilce { get; set; }
    }
}
