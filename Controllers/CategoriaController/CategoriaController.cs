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
    public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
    {
        var response = _context?.Categorias?.Include(p => p.Produtos)?.AsNoTracking()?.ToList();

        if (response is null)
        {
            return NotFound("Categoria com produtos não encontrada");
        }

        return response;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Categoria>> GetCategorias()
    {
        try
        {
            var response = _context?.Categorias?.AsNoTracking()?.ToList();

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
    public ActionResult PostCategoria(Categoria data)
    {
        if (data is null)
        {
            return BadRequest();
        }

        _context?.Categorias?.Add(data);
        _context?.SaveChanges();

        return new CreatedAtRouteResult("ObterCategoria", new { id = data.CategoriaId }, data);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutCategoria(int id, Categoria data)
    {
        if (id != data.CategoriaId)
        {
            return BadRequest();
        }

        _context.Entry(data).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(data);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategoria(int id)
    {
        var categoria = _context?.Categorias?.FirstOrDefault(p => p.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrada");
        }

        _context?.Categorias?.Remove(categoria);
        _context?.SaveChanges();

        return Ok(categoria);
    }

}