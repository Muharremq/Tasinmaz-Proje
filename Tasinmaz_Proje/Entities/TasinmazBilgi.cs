using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Tasinmaz_Proje.Entities
{
    public class TasinmazBilgi
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        public int Ada { get; set; }

        [Required]
        public int Parsel { get; set; }

        [Required]
        [MaxLength(50)]
        public string Nitelik { get; set; }
        [Required]
        public string Adres{ get; set; }

        [Required]
        public double KoordinatX { get; set; }
        [Required]
        public double KoordinatY { get; set; }

        public int MahalleId { get; set; }
        [ForeignKey("MahalleId")]
        public Mahalle Mahalle { get; set; }
    }
}
