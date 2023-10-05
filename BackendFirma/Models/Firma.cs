using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendFirma.Models
{
    [Table("firmadigital")]
    public class Firma
    {
        [Key]
        [Display(Name = "id_firma")]
        public int id_firma { get; set; }

        [Display(Name = "tipo_firma")]
        public char tipo_firma { get; set; }

        [Display(Name = "razon_social")]
        public string razon_social { get; set; } = null!;

        [Display(Name = "representante_legal")]
        public string representante_legal { get; set; } = null!;

        [Display(Name = "empresa_acreditadora")]
        public string empresa_acreditadora { get; set; } = null!;

        [Display(Name = "fecha_emision")]
        //[Column(TypeName = "timestamp without time zone")]
        public DateTime fecha_emision { get; set; }

        [Display(Name = "fecha_vencimiento")]
        //[Column(TypeName = "timestamp without time zone")]
        public DateTime fecha_vencimiento { get; set; }

        [Display(Name = "ruta_rubrica")]
        public string? ruta_rubrica { get; set; }

        [Display(Name = "certificado_digital")]
        public string? certificado_digital { get; set; }

        
    }
}
