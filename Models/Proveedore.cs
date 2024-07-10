using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PruebaUnitaria.Models
{
    public partial class Proveedore
    {
        [Key]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El campo Código es obligatorio.")]
        [StringLength(20, ErrorMessage = "El campo Código no puede tener más de 20 caracteres.")]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "El campo Razón Social es obligatorio.")]
        [StringLength(150, ErrorMessage = "El campo Razón Social no puede tener más de 150 caracteres.")]
        public string RazonSocial { get; set; } = null!;

        [Required(ErrorMessage = "El campo RFC es obligatorio.")]
        [StringLength(13, ErrorMessage = "El campo RFC no puede tener más de 13 caracteres.")]
        [RegularExpression(@"^[A-Za-z]{4}\d{6}[A-Za-z]\d{2}$", ErrorMessage = "El RFC del proveedor no cumple con el formato esperado.")]
        public string Rfc { get; set; } = null!;


    }
}