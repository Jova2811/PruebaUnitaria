using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaUnitaria.Models;

namespace PruebaUnitaria.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly DbPracticaContext _context;

        public ProveedoresController(DbPracticaContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {
            var proveedores = _context.Proveedores.ToList(); 
            

            return View(proveedores);

        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores
                .FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedore == null)
            {
                return NotFound();
            }

            return View(proveedore);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProveedor,Codigo,RazonSocial,Rfc")] Proveedore proveedore)
        {
            if (_context.Proveedores.Any(p => p.Codigo == proveedore.Codigo))
            {
                ModelState.AddModelError("Codigo", "El código del proveedor ya existe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(proveedore);
                    await _context.SaveChangesAsync();
                    return Json(new { success = true, message = "El proveedor se ha creado correctamente." });
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Hubo un problema al crear el proveedor: " + ex.Message });
                }
            }
            else
            {
                var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errorList });
            }
        }


        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedore = await _context.Proveedores.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }
            return View(proveedore);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProveedor,Codigo,RazonSocial,Rfc")] Proveedore proveedore)
        {
            if (id != proveedore.IdProveedor)
            {
                return Json(new { success = false, message = "El ID del proveedor no coincide." });
            }

            // Validación para el Codigo
            if (_context.Proveedores.Any(p => p.Codigo == proveedore.Codigo && p.IdProveedor != proveedore.IdProveedor))
            {
                ModelState.AddModelError("Codigo", "El código del proveedor ya existe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProveedor = await _context.Proveedores.FindAsync(id);
                    if (existingProveedor == null)
                    {
                        return Json(new { success = false, message = "Proveedor no encontrado." });
                    }

                    existingProveedor.Codigo = !string.IsNullOrEmpty(proveedore.Codigo) ? proveedore.Codigo : existingProveedor.Codigo;
                    existingProveedor.RazonSocial = !string.IsNullOrEmpty(proveedore.RazonSocial) ? proveedore.RazonSocial : existingProveedor.RazonSocial;
                    existingProveedor.Rfc = !string.IsNullOrEmpty(proveedore.Rfc) ? proveedore.Rfc : existingProveedor.Rfc;

                    _context.Update(existingProveedor);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "El proveedor se ha actualizado correctamente." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProveedoreExists(proveedore.IdProveedor))
                    {
                        return Json(new { success = false, message = "Proveedor no encontrado." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Hubo un problema de concurrencia al actualizar el proveedor." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Hubo un problema al actualizar el proveedor: " + ex.Message });
                }
            }
            else
            {
                var errorList = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, errors = errorList });
            }
        }

        private bool ProveedoreExists(int id)
        {
            return _context.Proveedores.Any(e => e.IdProveedor == id);
        }

        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Proveedores == null)
            {
                return NotFound();
            }

            var proveedor = await _context.Proveedores.FirstOrDefaultAsync(m => m.IdProveedor == id);
            if (proveedor == null)
            {
                return NotFound();
            }

            return View(proveedor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, bool deleteProducts = false)
        {
            var proveedor = await _context.Proveedores.FindAsync(id);
            if (proveedor == null)
            {
                return NotFound();
            }

            try
            {

                var tieneProductosAsociados = _context.Productos.Any(p => p.IdProveedor == id);

                if (tieneProductosAsociados && !deleteProducts)
                {
                    TempData["ShowDeleteConfirmation"] = id;
                    return RedirectToAction(nameof(Index));
                }

                if (tieneProductosAsociados && deleteProducts)
                {

                    var productosAEliminar = await _context.Productos.Where(p => p.IdProveedor == id).ToListAsync();
                    _context.Productos.RemoveRange(productosAEliminar);
                    await _context.SaveChangesAsync();
                }

                _context.Proveedores.Remove(proveedor);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                if (ex.InnerException is SqlException sqlException)
                {

                    Console.WriteLine($"Error al eliminar proveedor: {sqlException.Message}");
                }
                else
                {

                    Console.WriteLine($"Error de actualización de base de datos: {ex.Message}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error inesperado al eliminar proveedor: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }



        //public async Task<IActionResult> Verificar(int id)
        //{
        //    // Verificar si hay productos asociados al proveedor
        //    var tieneProductosAsociados = _context.Productos.Any(p => p.IdProveedor == id);

        //    if (tieneProductosAsociados)
        //    {
        //        // Mostrar la confirmación en el Index si hay productos asociados
        //        TempData["ShowDeleteConfirmation"] = id; // Guardar el ID del proveedor para mostrar la confirmación
        //        return RedirectToAction(nameof(Index));
        //    }

        //}



    }
}
