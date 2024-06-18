using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers.ProdutosController;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{

   //variavel privada que só pode ser lida
   //private/public - definindo que só pode ser lida - tipagem da variavel - nome (Variaveis privadas no C# são iniciadas com '_')
   private readonly AppDbContext _context;

   //Construtor
   public ProdutosController(AppDbContext context)
   {
      _context = context;
   }

   [HttpGet]
   public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
   {
      if (_context is null || _context?.Produtos is null)
      {
         return NotFound("Produto não encontrado");
      }

      var response = await _context.Produtos.AsNoTracking().ToListAsync();

      if (response is null)
      {
         return NotFound("Produtos não encontrados");
      }

      return response;
   }

   [HttpGet("{id:int}", Name = "ObterProduto")]
   // Definindo um parametro obrigatório
   // public async Task<ActionResult<Produto>> GetProdutoById(int id , [BindRequired] string nome)
   public async Task<ActionResult<Produto>> GetProdutoById(int id)
   {
      if (_context is null || _context?.Produtos is null)
      {
         return NotFound("Produto não encontrado");
      }

      var response = await _context.Produtos.AsNoTracking().FirstOrDefaultAsync(p => p.ProdutoId == id);

      if (response is null)
      {
         return NotFound("Produto não encontrado");
      }

      return response;
   }

   [HttpPost]
   public async Task<ActionResult> PostProduto(Produto data)
   {
      if (data is null)
      {
         return BadRequest();
      }

      if (_context is null || _context?.Produtos is null)
      {
         return NotFound("Produto não encontrado");
      }

      _context.Produtos.Add(data);
      //Precisa do saveChanges para inserir no banco 
      await _context.SaveChangesAsync();

      return new CreatedAtRouteResult("ObterProduto", new { id = data.ProdutoId }, data);
   }

   [HttpPut("{id:int}")]
   public async Task<ActionResult> PutProduto(int id, Produto data)
   {
      if (id != data.ProdutoId)
      {
         return BadRequest();
      }

      _context.Entry(data).State = EntityState.Modified;
      await _context.SaveChangesAsync();

      return Ok(data);
   }

   [HttpDelete("{id:int}")]
   public async Task<ActionResult> DeleteProduto(int id)
   {

      if (_context is null || _context?.Produtos is null)
      {
         return NotFound("Produto não encontrado");
      }

      var produto = await _context.Produtos.FirstOrDefaultAsync(p => p.ProdutoId == id);

      if (produto is null)
      {
         return NotFound("Produto não encontrado");
      }

      _context.Produtos.Remove(produto);
      await _context.SaveChangesAsync();

      return Ok(produto);
   }
}