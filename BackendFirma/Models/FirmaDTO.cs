using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendFirma.Models
{
    
    public class FirmaDTO
    {
        
        public int idFirma { get; set; }        
        public char tipoFirma { get; set; }        
        public string razonSocial { get; set; } = null!;
        public string representanteLegal { get; set; } = null!;
        public string empresaAcreditadora { get; set; } = null!;

        //[Column(TypeName = "timestamp without time zone")]
        public DateTime fechaEmision { get; set; }

        //[Column(TypeName = "timestamp without time zone")]
        public DateTime fechaVencimiento { get; set; }       
        public string? rutaRubrica { get; set; }      
        public string? certificadoDigital { get; set; }
        public IFormFile? rubricaFile { get; set; }
        public IFormFile? certificadoFile { get; set; }
    }
}
