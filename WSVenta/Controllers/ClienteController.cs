
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models;
using WSVenta.Models.Request;
using WSVenta.Models.Response;

namespace WSVenta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (Models.VentaRealContext db = new Models.VentaRealContext())
                { 
                    var lst = db.Cliente.OrderByDescending(d => d.Id).ToList();
                    respuesta.Exito = 1;
                    respuesta.Data = lst;
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpPost]
        public IActionResult Add(ClienteRequest oModel)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (Models.VentaRealContext db = new Models.VentaRealContext())
                {
                    Cliente oCliente = new Cliente();
                    oCliente.Nombre = oModel.Nombre;
                    db.Add(oCliente);
                    db.SaveChanges();
                    respuesta.Exito = 1;
                    respuesta.Mensaje = "Cliente agregado satisfactoriamente.";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);

        }

        [HttpPut]
        public IActionResult Edit(ClienteRequest oModel)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (Models.VentaRealContext db = new Models.VentaRealContext())
                {
                    Cliente oCliente = db.Cliente.Find(oModel.Id);
                    oCliente.Nombre = oModel.Nombre;
                    db.Entry(oCliente).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    db.SaveChanges();
                    respuesta.Exito = 1;
                    respuesta.Mensaje = "Cliente editado satisfactoriamente.";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                using (Models.VentaRealContext db = new Models.VentaRealContext())
                {
                    Cliente oCliente = db.Cliente.Find(Id);
                    db.Remove(oCliente);
                    db.SaveChanges();
                    respuesta.Exito = 1;
                    respuesta.Mensaje = "Cliente eliminado satisfactoriamente.";
                }
            }
            catch (Exception ex)
            {
                respuesta.Mensaje = ex.Message;
            }

            return Ok(respuesta);
        }
    }
}
