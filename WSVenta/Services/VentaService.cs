using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WSVenta.Models;
using WSVenta.Models.Request;

namespace WSVenta.Services
{
    public class VentaService : IVentaService
    {
        public void Add(VentaRequest model)
        {
            using (VentaRealContext db = new VentaRealContext())
            {
                using (var transaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        var venta = new Venta
                        {
                            Total = model.Conceptos.Sum(d => d.Cantidad * d.PrecioUnitario),
                            Fecha = DateTime.Now,
                            IdCliente = model.IdCliente
                        };

                        db.Venta.Add(venta);
                        db.SaveChanges();

                        foreach (var modelConcepto in model.Conceptos)
                        {
                            var concepto = new Models.Concepto
                            {
                                Cantidad = modelConcepto.Cantidad,
                                IdProducto = modelConcepto.IdProducto,
                                PrecioUnitario = modelConcepto.PrecioUnitario,
                                Importe = modelConcepto.Importe,
                                IdVenta = venta.Id
                            };

                            db.Concepto.Add(concepto);
                            db.SaveChanges();
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw new Exception("Ocurrió un error en la inserción.");
                    }
                }
            }
        }
    }
}
