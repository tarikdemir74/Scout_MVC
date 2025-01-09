using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScoutModels
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Name { get; set; } //Identity ile gelen name nullable olduğu için

        public string? Street {  get; set; }
        public string? City { get; set; }   
        public string? PostalCode { get; set; }
        public int? StoreId { get; set; }
        [ForeignKey("StoreId")]
        [ValidateNever]
        public Store Store { get; set; }

    }
}
