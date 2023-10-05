using BackendFirma.Interfaces;
using BackendFirma.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;

namespace BackendFirma.Controllers
{
    [Route("api")]
    [ApiController]
    public class FirmaController : ControllerBase
    {
        private readonly IFirmaProvider firmaProvider;

        public FirmaController(IFirmaProvider firmaProvider)
        {
            this.firmaProvider = firmaProvider;
        }

        [HttpGet("firmas")]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await firmaProvider.GetAllAsync();

            var DTO = from f in results
                      select new FirmaDTO
                      {
                          idFirma = f.id_firma,
                          tipoFirma = f.tipo_firma,
                          razonSocial = f.razon_social,
                          representanteLegal = f.representante_legal,
                          empresaAcreditadora = f.empresa_acreditadora,
                          fechaEmision = f.fecha_emision,
                          fechaVencimiento = f.fecha_vencimiento,
                          rutaRubrica = f.ruta_rubrica,
                          certificadoDigital = f.certificado_digital
                      };

            if (DTO != null)
            {
                return Ok(DTO);
            }
            return NotFound("No se encontraron registros de firmas.");
        }

        [HttpGet("firma/{id}")]
        public async Task<IActionResult> GetAsync(int id)
        {
            var result = await firmaProvider.GetAsync(id);

            var DTO = new FirmaDTO 
                      {
                          idFirma = result.id_firma,
                          tipoFirma = result.tipo_firma,
                          razonSocial = result.razon_social,
                          representanteLegal = result.representante_legal,
                          empresaAcreditadora = result.empresa_acreditadora,
                          fechaEmision = result.fecha_emision,
                          fechaVencimiento = result.fecha_vencimiento,
                          rutaRubrica = result.ruta_rubrica,
                          certificadoDigital = result.certificado_digital
                      };

            if (DTO != null)
            {
                return Ok(DTO);
            }
            return NotFound("No se encontro registro de la firma.");

        }

        [HttpDelete("firma/{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var result = await firmaProvider.DeleteAsync(id);

            if (result == true)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }

        }

        //[HttpPost("firmaDigitalA")]
        //public async Task<IActionResult> AddAsync([FromForm] FirmaDTO firmaDTO)
        //{
        //    byte[] rubricaData;
        //    byte[] certificadoData;

        //    using (var rubricaStream = new MemoryStream())
        //    using (var certificadoStream = new MemoryStream())
        //    {
        //        await firmaDTO.rubricaFile!.CopyToAsync(rubricaStream);
        //        await firmaDTO.certificadoFile!.CopyToAsync(certificadoStream);

        //        rubricaData = rubricaStream.ToArray();
        //        certificadoData = certificadoStream.ToArray();
        //    }

        //    var firma = new Firma
        //    {
        //        id_firma = firmaDTO.idFirma,
        //        tipo_firma = firmaDTO.tipoFirma,
        //        razon_social = firmaDTO.razonSocial,
        //        representante_legal = firmaDTO.representanteLegal,
        //        empresa_acreditadora = firmaDTO.empresaAcreditadora,
        //        fecha_emision = firmaDTO.fechaEmision,
        //        fecha_vencimiento = firmaDTO.fechaVencimiento,
        //        ruta_rubrica = Convert.ToBase64String(rubricaData),
        //        certificado_digital = Convert.ToBase64String(certificadoData)
        //    };

        //    var result = await firmaProvider.AddAsync(firma);
        //    if (result)
        //    {
        //        return Ok(result);
        //    }
        //    return BadRequest("No se pudo registrar el área.");
        //}

        [HttpPost("firmaDigitalA")]
        public async Task<IActionResult> AdddAsync([FromForm] FirmaDTO firmaDTO)
        {
            if (firmaDTO.rubricaFile == null || firmaDTO.certificadoFile == null)
            {
                return BadRequest("Los archivos de rubrica y certificado son obligatorios.");
            }


            byte[] rubricaData;
            byte[] certificadoData;

            using (var rubricaStream = new MemoryStream())
            using (var certificadoStream = new MemoryStream())
            {
                await firmaDTO.rubricaFile.CopyToAsync(rubricaStream);
                await firmaDTO.certificadoFile.CopyToAsync(certificadoStream);

                rubricaData = rubricaStream.ToArray();
                certificadoData = certificadoStream.ToArray();
            }

            var firma = new Firma
            {

                id_firma = firmaDTO.idFirma,
                tipo_firma = firmaDTO.tipoFirma,
                razon_social = firmaDTO.razonSocial,
                representante_legal = firmaDTO.representanteLegal,
                empresa_acreditadora = firmaDTO.empresaAcreditadora,
                fecha_emision = firmaDTO.fechaEmision,
                fecha_vencimiento = firmaDTO.fechaVencimiento,
                ruta_rubrica = Convert.ToBase64String(rubricaData),
                certificado_digital = Convert.ToBase64String(certificadoData)
            };

            firma.fecha_emision = firma.fecha_emision.ToUniversalTime();
            firma.fecha_vencimiento = firma.fecha_vencimiento.ToUniversalTime();

            var result = await firmaProvider.AddAsync(firma);
            if (result)
            {
                return Ok(result);
            }
            return BadRequest("No se pudo registrar la firma.");

        }

        [HttpPut("firmaDigitalA/{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] FirmaDTO firmaDTO)
        {
            //if (firmaDTO.rubricaFile == null || firmaDTO.certificadoFile == null)
            //{
            //    return BadRequest("Los archivos de rubrica y certificado son obligatorios.");
            //}

            if (id != firmaDTO.idFirma)
            {
                return BadRequest("El ID proporcionado en la URL no coincide con el ID de la firma proporcionada en el cuerpo de la solicitud.");
            }

            // Obtener la firma existente por su ID
            var firmaExistente = await firmaProvider.GetAsync(id);

            if (firmaExistente == null)
            {
                return NotFound("Firma no encontrada.");
            }

            if (firmaDTO.rubricaFile != null && firmaDTO.certificadoFile == null)
            {
                byte[] rubricaData;
                

                using (var rubricaStream = new MemoryStream())
                
                {
                    await firmaDTO.rubricaFile.CopyToAsync(rubricaStream);
                   

                    rubricaData = rubricaStream.ToArray();
                    
                }


                // Actualizar los campos de la firma
                firmaExistente.id_firma = firmaDTO.idFirma;
                firmaExistente.tipo_firma = firmaDTO.tipoFirma;
                firmaExistente.razon_social = firmaDTO.razonSocial;
                firmaExistente.representante_legal = firmaDTO.representanteLegal;
                firmaExistente.empresa_acreditadora = firmaDTO.empresaAcreditadora;
                firmaExistente.fecha_emision = firmaDTO.fechaEmision.ToUniversalTime();
                firmaExistente.fecha_vencimiento = firmaDTO.fechaVencimiento.ToUniversalTime();
                firmaExistente.ruta_rubrica = Convert.ToBase64String(rubricaData);
                firmaExistente.certificado_digital = firmaDTO.certificadoDigital;

            } else if (firmaDTO.certificadoFile != null && firmaDTO.rubricaFile == null)
            {
                
                byte[] certificadoData;
                
                using (var certificadoStream = new MemoryStream())
                {
                    
                    await firmaDTO.certificadoFile.CopyToAsync(certificadoStream);

                    certificadoData = certificadoStream.ToArray();
                }

                // Actualizar los campos de la firma
                firmaExistente.id_firma = firmaDTO.idFirma;
                firmaExistente.tipo_firma = firmaDTO.tipoFirma;
                firmaExistente.razon_social = firmaDTO.razonSocial;
                firmaExistente.representante_legal = firmaDTO.representanteLegal;
                firmaExistente.empresa_acreditadora = firmaDTO.empresaAcreditadora;
                firmaExistente.fecha_emision = firmaDTO.fechaEmision.ToUniversalTime();
                firmaExistente.fecha_vencimiento = firmaDTO.fechaVencimiento.ToUniversalTime();
                firmaExistente.ruta_rubrica = firmaDTO.rutaRubrica;
                firmaExistente.certificado_digital = Convert.ToBase64String(certificadoData);

            } else if (firmaDTO.rubricaFile != null && firmaDTO.certificadoFile != null)
            {
                byte[] rubricaData;
                byte[] certificadoData;

                using (var rubricaStream = new MemoryStream())
                using (var certificadoStream = new MemoryStream())
                {
                    await firmaDTO.rubricaFile.CopyToAsync(rubricaStream);
                    await firmaDTO.certificadoFile.CopyToAsync(certificadoStream);

                    rubricaData = rubricaStream.ToArray();
                    certificadoData = certificadoStream.ToArray();
                }

                // Actualizar los campos de la firma
                firmaExistente.id_firma = firmaDTO.idFirma;
                firmaExistente.tipo_firma = firmaDTO.tipoFirma;
                firmaExistente.razon_social = firmaDTO.razonSocial;
                firmaExistente.representante_legal = firmaDTO.representanteLegal;
                firmaExistente.empresa_acreditadora = firmaDTO.empresaAcreditadora;
                firmaExistente.fecha_emision = firmaDTO.fechaEmision.ToUniversalTime();
                firmaExistente.fecha_vencimiento = firmaDTO.fechaVencimiento.ToUniversalTime();
                firmaExistente.ruta_rubrica = Convert.ToBase64String(rubricaData);
                firmaExistente.certificado_digital = Convert.ToBase64String(certificadoData);
            }
            else if (firmaDTO.rubricaFile == null && firmaDTO.certificadoFile == null)
            {
                // Actualizar los campos de la firma
                firmaExistente.id_firma = firmaDTO.idFirma;
                firmaExistente.tipo_firma = firmaDTO.tipoFirma;
                firmaExistente.razon_social = firmaDTO.razonSocial;
                firmaExistente.representante_legal = firmaDTO.representanteLegal;
                firmaExistente.empresa_acreditadora = firmaDTO.empresaAcreditadora;
                firmaExistente.fecha_emision = firmaDTO.fechaEmision.ToUniversalTime();
                firmaExistente.fecha_vencimiento = firmaDTO.fechaVencimiento.ToUniversalTime();
                firmaExistente.ruta_rubrica = firmaDTO.rutaRubrica;
                firmaExistente.certificado_digital = firmaDTO.certificadoDigital;
            }          

            // Actualizar la firma en la base de datos
            var result = await firmaProvider.UpdateAsync(id, firmaExistente);

            if (result)
            {
                return Ok(result);
            }

            return BadRequest("No se pudo actualizar la firma.");
        }

        //[HttpPost("firmaDigitalA")]
        //public async Task<IActionResult> AdddAsync([FromForm] FirmaDTO firmaDTO)
        //{
        //    if (firmaDTO.rubricaFile == null || firmaDTO.certificadoFile == null)
        //    {
        //        return BadRequest("Los archivos de rubrica y certificado son obligatorios.");
        //    }

        //    try

        //    {          
        //        byte[] rubricaData;
        //        byte[] certificadoData;

        //        using (var rubricaStream = new MemoryStream())
        //        using (var certificadoStream = new MemoryStream())
        //        {
        //            await firmaDTO.rubricaFile.CopyToAsync(rubricaStream);
        //            await firmaDTO.certificadoFile.CopyToAsync(certificadoStream);

        //            rubricaData = rubricaStream.ToArray();
        //            certificadoData = certificadoStream.ToArray();
        //        }

        //        var firma = new Firma
        //        {

        //            id_firma = firmaDTO.idFirma,
        //            tipo_firma = firmaDTO.tipoFirma,
        //            razon_social = firmaDTO.razonSocial,
        //            representante_legal = firmaDTO.representanteLegal,
        //            empresa_acreditadora = firmaDTO.empresaAcreditadora,
        //            fecha_emision = firmaDTO.fechaEmision,
        //            fecha_vencimiento = firmaDTO.fechaVencimiento,
        //            ruta_rubrica = Convert.ToBase64String(rubricaData),
        //            certificado_digital = Convert.ToBase64String(certificadoData)
        //        };


        //        var result = await firmaProvider.AddAsync(firma);
        //        if (result)
        //        {
        //            return Ok(result);
        //        }
        //        return BadRequest("No se pudo registrar la firma.");
        //    }
        //    catch (Exception ex)
        //    {
        //        // Manejar cualquier excepción que pueda ocurrir durante el proceso
        //        return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        //    }
        //}
    }
}
