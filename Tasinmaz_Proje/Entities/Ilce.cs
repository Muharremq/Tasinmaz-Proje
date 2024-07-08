using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tasinmaz_Proje.Entities
{
    public class Ilce
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int IlId { get; set; }
        [ForeignKey("IlId")]
        public Il Il { get; set; }
    }
}
