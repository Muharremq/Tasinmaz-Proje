using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tasinmaz_Proje.Entities
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        
        public ICollection<TasinmazBilgi> Tasinmazlar { get; set; }
    }
}
