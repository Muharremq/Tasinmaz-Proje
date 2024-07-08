﻿using System.ComponentModel.DataAnnotations;

namespace Tasinmaz_Proje.Entities
{
    public class IslemTip
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
    }
}
