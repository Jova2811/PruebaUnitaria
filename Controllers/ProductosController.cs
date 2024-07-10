using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaUnitaria.Models;

namespace PruebaUnitaria.Controllers
{
    public class ProductosController : Controller
    {
        private readonly DbPracticaContext _context;

        public ProductosController(DbPracticaContext context)
        {
            _context = context;
        }

        // GET: Proveedores
        public async Task<IActionResult> Index()
        {

            return _context.Proveedores != null ? 
                          View(await _context.Productos.ToListAsync()) :
                          Problem("Entity set 'DbPracticaContext.Productos'  is null.");


        }

        // GET: Proveedores/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // GET: Proveedores/Create
        public IActionResult Create()
        {
            var provee = _context.Proveedores.ToList(); 

            ViewBag.Provedores = provee;
            return View();
        }

        // POST: Proveedores/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdProveedor,Codigo,Descripcion,Unidad,Costo")] Producto producto)
        {
            
            if (!_context.Proveedores.Any(p => p.IdProveedor == producto.IdProveedor))
            {
                ModelState.AddModelError("IdProveedor", "El proveedor seleccionado no existe.");
            }

            
            if (_context.Productos.Any(p => p.Codigo == producto.Codigo))
            {
                ModelState.AddModelError("Codigo", "El código del producto ya existe.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(producto);
                    await _context.SaveChangesAsync();

                    
                    return Json(new { success = true, message = "El producto se ha creado correctamente." });
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "Hubo un error al crear el producto.";
                    return Json(new { success = false, message = "Hubo un error al crear el producto." });
                }
            }

            ViewData["IdProveedor"] = new SelectList(_context.Proveedores, "IdProveedor", "RazonSocial", producto.IdProveedor);
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList() });
        }



        // GET: Proveedores/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var provee = _context.Proveedores.ToList(); 

            ViewBag.Provedores = provee;

            var proveedore = await _context.Productos.FindAsync(id);
            if (proveedore == null)
            {
                return NotFound();
            }
            return View(proveedore);
        }

        // POST: Proveedores/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdProveedor,Codigo,Descripcion,Unidad,Costo")] Producto producto)
        {
            if (id != producto.IdProducto)
            {
                return Json(new { success = false, message = "El ID del producto no coincide." });
            }

            
            if (!_context.Proveedores.Any(p => p.IdProveedor == producto.IdProveedor))
            {
                ModelState.AddModelError("IdProveedor", "El proveedor seleccionado no existe.");
            }

            
            if (_context.Productos.Any(p => p.Codigo == producto.Codigo && p.IdProducto != producto.IdProducto))
            {
                ModelState.AddModelError("Codigo", "El código del producto ya existe.");
            }

            if (!decimal.TryParse(producto.Costo.ToString(), out _))
            {
                ModelState.AddModelError("Costo", "El valor del Costo no es válido.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingProducto = await _context.Productos.FindAsync(id);
                    if (existingProducto == null)
                    {
                        return Json(new { success = false, message = "Producto no encontrado." });
                    }

                    existingProducto.IdProveedor = producto.IdProveedor != 0 ? producto.IdProveedor : existingProducto.IdProveedor;
                    existingProducto.Codigo = !string.IsNullOrEmpty(producto.Codigo) ? producto.Codigo : existingProducto.Codigo;
                    existingProducto.Descripcion = !string.IsNullOrEmpty(producto.Descripcion) ? producto.Descripcion : existingProducto.Descripcion;
                    existingProducto.Unidad = !string.IsNullOrEmpty(producto.Unidad) ? producto.Unidad : existingProducto.Unidad;
                    existingProducto.Costo = producto.Costo != 0 ? producto.Costo : existingProducto.Costo;

                    _context.Update(existingProducto);
                    await _context.SaveChangesAsync();

                    return Json(new { success = true, message = "El producto se ha actualizado correctamente." });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return Json(new { success = false, message = "Producto no encontrado." });
                    }
                    else
                    {
                        return Json(new { success = false, message = "Hubo un error de concurrencia al actualizar el producto." });
                    }
                }
                catch (Exception ex)
                {
                    return Json(new { success = false, message = "Hubo un error al actualizar el producto: " + ex.Message });
                }
            }

            var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
            return Json(new { success = false, errors });
        }

        private bool ProductoExists(int id)
        {
            return _context.Productos.Any(e => e.IdProducto == id);
        }




        // GET: Proveedores/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        // POST: Proveedores/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return Json(new { success = false, message = "Producto no encontrado." });
            }

            try
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                return Json(new { success = true, message = "El producto se ha eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hubo un problema al eliminar el producto: " + ex.Message });
            }
        }



    }
}
