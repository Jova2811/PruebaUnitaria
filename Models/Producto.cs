using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;



namespace PruebaUnitaria.Models
{
    public partial class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "El Código del producto es obligatorio.")]
        [StringLength(20, ErrorMessage = "El campo Código no puede tener más de 20 caracteres.")]
        public string Codigo { get; set; } = null!;

        [Required(ErrorMessage = "La Descripcion del producto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El campo Descripción no puede tener más de 150 caracteres.")]
        public string Descripcion { get; set; } = null!;

        [Required(ErrorMessage = "La Unidad del producto es obligatorio.")]
        [StringLength(3, ErrorMessage = "El campo Unidad no puede tener más de 3 caracteres.")]
        public string Unidad { get; set; } = null!;

        [Required(ErrorMessage = "El campo Costo es obligatorio.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "El campo Costo debe ser un valor decimal con hasta dos decimales.")]
        public decimal Costo { get; set; }


    }
}
