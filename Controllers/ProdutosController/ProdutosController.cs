using APICatalogo.Context;
using APICatalogo.Models;
using Microsoft.AspNetCore.Mvc;
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
   public ActionResult<IEnumerable<Produto>> GetProdutos()
   {
      var response = _context?.Produtos?.AsNoTracking()?.ToList();

      if (response is null)
      {
         return NotFound("Produtos não encontrados");
      }

      return response;
   }

   [HttpGet("{id:int}", Name = "ObterProduto")]
   public ActionResult<Produto> GetProdutoById(int id)
   {
      var response = _context?.Produtos?.AsNoTracking()?.FirstOrDefault(p => p.ProdutoId == id);

      if (response is null)
      {
         return NotFound("Produto não encontrado");
      }

      return response;
   }

   [HttpPost]
   public ActionResult PostProduto(Produto data)
   {

      if (data is null)
      {
         return BadRequest();
      }

      _context?.Produtos?.Add(data);
      //Precisa do saveChanges para inserir no banco 
      _context?.SaveChanges();

      return new CreatedAtRouteResult("ObterProduto", new { id = data.ProdutoId }, data);
   }

   [HttpPut("{id:int}")]
   public ActionResult PutProduto(int id, Produto data){
      if (id != data.ProdutoId){
         return BadRequest();
      }

      _context.Entry(data).State = EntityState.Modified;
      _context.SaveChanges();

      return Ok(data);
   }

   [HttpDelete("{id:int}")]
   public ActionResult DeleteProduto(int id){
      var produto = _context?.Produtos?.FirstOrDefault(p => p.ProdutoId == id);

      if (produto is null) {
         return NotFound("Produto não encontrado");
      }

      _context?.Produtos?.Remove(produto);
      _context?.SaveChanges();

      return Ok(produto);
   }
}