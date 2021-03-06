﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CmsCoreV2.Models
{
    public class FormField:BaseEntity
    {
        public FormField()
        {
            Position = 0;
        }
        [Required]
        [StringLength(200)]
        [Display(Name = "Form Alanı Adı")]
        public string Name { get; set; }
        [Display(Name = "Zorunlu Mu?")]
        public bool Required { get; set; }
        [Display(Name = "Değeri")]
        public string Value { get; set; }
        [Display(Name = "Pozisyonu")]
        public int Position { get; set; }
        [Display(Name = "Alan Tipi")]
        public FieldType FieldType { get; set; }
        [Display(Name = "Form")]
        public long? FormId { get; set; }
        [ForeignKey("FormId")]
        [Display(Name = "Form")]
        public virtual Form Form { get; set; }
    }
}
