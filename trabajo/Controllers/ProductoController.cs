using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using trabajo.Models;

namespace trabajo.Controllers
{
    public class ProductoController : Controller
    {
        private readonly tpcarritoContext _context;
        
        public ProductoController(tpcarritoContext context)
        {
            _context = context;
        }


        // GET: Producto
        public async Task<IActionResult> Index()
        {
            var tpcarritoContext = _context.Productos.Include(p => p.IdCategoriaNavigation);
            return View(await tpcarritoContext.ToListAsync());
        }

        // GET: Producto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewData["RutaImagen"] = producto.RutaImagen;

            return View(producto);
        }




        // GET: Producto/Create
        public IActionResult Create()
        {
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion");
            return View();
        }

        // POST: Producto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdProducto,IdCategoria,Nombre,Descripcion,Precio,RutaImagen,NombreImagen,FechaCarga")] Producto producto, IFormFile archivoImagen)
        {
            if (ModelState.IsValid)
            {
                if (archivoImagen != null && archivoImagen.Length > 0)
                {
                    var fileName = Path.GetFileName(archivoImagen.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await archivoImagen.CopyToAsync(stream);
                    }

                    producto.NombreImagen = fileName;
                    producto.RutaImagen = "/images/" + fileName;
                }
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion", producto.IdCategoria);

            ViewData["RutaImagen"] = producto.RutaImagen;
            return View(producto);
        }

        // GET: Producto/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion", producto.IdCategoria);
            return View(producto);
        }

        // POST: Producto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdProducto,IdCategoria,Nombre,Descripcion,Precio,RutaImagen,NombreImagen,FechaCarga")] Producto producto, IFormFile archivoImagen)
        {
            if (id != producto.IdProducto)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Obtener el producto existente
                    var productoExistente = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.IdProducto == id);

                    if (productoExistente == null)
                    {
                        return NotFound();
                    }

                    // Manejo de la imagen
                    if (archivoImagen != null)
                    {
                        // Eliminar la imagen anterior si existe
                        if (!string.IsNullOrEmpty(productoExistente.RutaImagen))
                        {
                            var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", productoExistente.RutaImagen.TrimStart('/'));
                            if (System.IO.File.Exists(oldFilePath))
                            {
                                System.IO.File.Delete(oldFilePath);
                            }
                        }

                        // Guardar la nueva imagen
                        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + archivoImagen.FileName;
                        var filePath = Path.Combine(uploads, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await archivoImagen.CopyToAsync(fileStream);
                        }

                        producto.RutaImagen = "/images/" + uniqueFileName;
                        producto.NombreImagen = uniqueFileName;
                    }
                    else
                    {
                        // Mantener la información de la imagen existente
                        producto.RutaImagen = productoExistente.RutaImagen;
                        producto.NombreImagen = productoExistente.NombreImagen;
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.IdProducto))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCategoria"] = new SelectList(_context.Categoria, "IdCategoria", "Descripcion", producto.IdCategoria);
            return View(producto);
        }
    

        // GET: Producto/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Productos == null)
            {
                return NotFound();
            }

            var producto = await _context.Productos
                .Include(p => p.IdCategoriaNavigation)
                .FirstOrDefaultAsync(m => m.IdProducto == id);
            if (producto == null)
            {
                return NotFound();
            }

            ViewData["RutaImagen"] = producto.RutaImagen;

            return View(producto);
        }

        // POST: Producto/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Productos == null)
            {
                return Problem("Entity set 'tpcarritoContext.Productos' is null.");
            }

            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                // Eliminar el archivo de imagen asociado si existe
                if (!string.IsNullOrEmpty(producto.RutaImagen))
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", producto.RutaImagen.TrimStart('/'));
                    if (System.IO.File.Exists(filePath))
                    {
                        System.IO.File.Delete(filePath);
                    }
                }

                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            

            return RedirectToAction(nameof(Index));
        }

        private bool ProductoExists(int id)
        {
            return (_context.Productos?.Any(e => e.IdProducto == id)).GetValueOrDefault();
        }

    }
}
