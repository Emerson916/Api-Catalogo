using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers.CategoriaController;

[Route("[controller]")]
[ApiController]

public class CategoriaController : ControllerBase
{
    private readonly AppDbContext _context;

    public CategoriaController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("produtos")]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategoriaProdutos()
    {
        if (_context?.Categorias is null)
        {
            return NotFound("Categorias não encontradas");
        }

        var response = await _context.Categorias.Include(p => p.Produtos).AsNoTracking().ToListAsync();

        if (response is null)
        {
            return NotFound("Categoria com produtos não encontrada");
        }

        return response;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Categoria>>> GetCategorias()
    {
        try
        {
            if (_context?.Categorias is null)
            {
                return NotFound("Categorias não encontradas");
            }

            var response = await _context.Categorias.AsNoTracking().ToListAsync();

            if (response is null)
            {
                return NotFound("Categorias não encontradas");
            }
            return response;
        }
        catch (Exception)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocorreu um problema ao tratar a sua solicitação");
        }
    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetCategoriaById(int id)
    {
        var response = _context?.Categorias?.AsNoTracking()?.FirstOrDefault(c => c.CategoriaId == id);

        if (response is null)
        {
            return NotFound("Categorias não encontrada");
        }

        return response;
    }

    [HttpPost]
    public async Task<ActionResult> PostCategoria(Categoria data)
    {
        if (data is null)
        {
            return BadRequest();
        }

        if (_context is null || _context?.Categorias is null)
        {
            return NotFound("Categorias não encontradas");
        }

        _context.Categorias.Add(data);
        await _context.SaveChangesAsync();

        return new CreatedAtRouteResult("ObterCategoria", new { id = data.CategoriaId }, data);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult> PutCategoria(int id, Categoria data)
    {
        if (id != data.CategoriaId)
        {
            return BadRequest();
        }

         if (_context is null || _context?.Categorias is null)
        {
            return NotFound("Categorias não encontradas");
        }

        _context.Entry(data).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(data);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteCategoria(int id)
    {
        var categoria = _context?.Categorias?.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrada");
        }

        if (_context is null || _context?.Categorias is null)
        {
            return NotFound("Categorias não encontradas");
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return Ok(categoria);
    }

}