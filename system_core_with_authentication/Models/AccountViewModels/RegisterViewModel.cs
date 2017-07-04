﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using system_core_with_authentication.Data;

namespace system_core_with_authentication.Models.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*(_|[^\\w])).+$", ErrorMessage = "La contraseña necesita almenos tener una letra mayuscula, una minuscula y un caracter especial")]
        [DataType(DataType.Password)]
        [Display(Name = "Contraseña")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirmar contraseña")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Nombre(s)")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Apellido Paterno")]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Apellido Materno")]
        [MaxLength(50)]
        public string SecondLastName { get; set; }

        [Required]
        [Display(Name = "Telefono")]
        [MaxLength(20)]
        public string Telephone { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Rol:")]
        [UIHint("List")]
        public List<SelectListItem> Roles { get; set; }
        public string Role { get; set; }

        [Display(Name = "Imagen de usuario:")]
        public IFormFile UserImage { get; set; }

        public bool valid { get; set; }

        public RegisterViewModel()
        {
            Roles = new List<SelectListItem>();

        }

        public void getRoles(ApplicationDbContext _context)
        {
            var roles = from r in _context.identityRole select r;
            var listRole = roles.ToList();
            foreach (var Data in listRole)
            {
                Roles.Add(new SelectListItem()
                {
                    Value = Data.Id,
                    Text = Data.Name
                });
            }

        }

    }
}

