using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers.ProdutosController;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{

   //variavel privada que só pode ser lida
   //private/public - definindo que só pode ser lida - tipagem da variavel - nome (Variaveis privadas no C# são iniciadas com '_')
   private readonly IProdutoRepository _repository;
   private readonly ILogger _logger;

   //Construtor
   public ProdutosController(IProdutoRepository repository, ILogger<ProdutosController> logger)
   {
      _repository = repository;
      _logger = logger;
   }

   [HttpGet]
   public ActionResult<IQueryable<Produto>> GetProdutos()
   {
      var response = _repository.GetProdutosRepository().ToList();;

      if (response is null)
      {
         return NotFound("Produtos não encontrados");
      }

      return Ok(response);
   }

   [HttpGet("{id:int}", Name = "ObterProduto")]
   // Definindo um parametro obrigatório
   // public async Task<ActionResult<Produto>> GetProdutoById(int id , [BindRequired] string nome)
   public ActionResult<Produto> GetProdutoById(int id)
   {
      var response = _repository.GetProdutoByIdRepository(id);

      if (response is null)
      {
         return NotFound("Produto não encontrado");
      }

      return Ok(response);
   }

   [HttpPost]
   public ActionResult PostProduto(Produto data)
   {
      if (data is null)
      {
         return BadRequest();
      }

      var response = _repository.PostProdutoRepository(data);

      return new CreatedAtRouteResult("ObterProduto", new { id = response.ProdutoId }, response);
   }

   [HttpPut("{id:int}")]
   public ActionResult PutProduto(int id, Produto data)
   {
      if (id != data.ProdutoId)
      {
         _logger.LogWarning($"Dados inválidos...");
         return BadRequest();
      }

      var response = _repository.PutProdutoRepository(id, data);

      if (response)  {
      return Ok(response);
      } else {
         return StatusCode(500, "Falha ao atualizar o produto");
      }
   }

   [HttpDelete("{id:int}")]
   public ActionResult DeleteProduto(int id)
   {
      var response = _repository.DeleteProdutoRepository(id);
      return Ok(response);
   }
}