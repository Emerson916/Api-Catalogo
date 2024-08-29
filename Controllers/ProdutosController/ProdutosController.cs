using APICatalogo.DTOs;
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
    private readonly IUnitOfWork _uof;
    private readonly ILogger _logger;

    //Construtor
    public ProdutosController(IUnitOfWork uof, ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _logger = logger;
    }

    [HttpGet("categorias/{id}")]
    public ActionResult<IEnumerable<ProdutoDTO>> GetProdutosCategoria(int id)
    {
       
        var produtos = _uof.ProdutoRepository.GetProdutosPorCategoriaRepository(id);

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);    
    }

    [HttpGet]
    public ActionResult<IQueryable<ProdutoDTO>> GetProdutos()
    {
        var response = _uof.ProdutoRepository.GetAll();

        if (response is null)
        {
            return NotFound("Produtos não encontrados");
        }

        return Ok(response);
    }

    [HttpGet("{id:int}", Name = "ObterProduto")]
    // Definindo um parametro obrigatório
    // public async Task<ActionResult<Produto>> GetProdutoById(int id , [BindRequired] string nome)
    public ActionResult<ProdutoDTO> GetProdutoById(int id)
    {
        var response = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (response is null)
        {
            return NotFound("Produto não encontrado");
        }

        return Ok(response);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> PostProduto(ProdutoDTO data)
    {
        if (data is null)
        {
            return BadRequest();
        }

        var response = _uof.ProdutoRepository.Create(data);

         if (response is null)
        {
            return NotFound("Produto não criado...");
        }

        return new CreatedAtRouteResult("ObterProduto", new { id = response.ProdutoId }, response);
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> PutProduto(int id, ProdutoDTO data)
    {
        if (id != data.ProdutoId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest();
        }

        var response = _uof.ProdutoRepository.Update(data);

        if (response is null) {
            return StatusCode(500, "Falha ao atualizar o produto");
        }

        _uof.Commit();
      return Ok(response);
        
    }

    [HttpDelete("{id:int}")]
    public ActionResult<ProdutoDTO> DeleteProduto(int id)
    {
        var produto = _uof.ProdutoRepository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado..");
        }

        var response = _uof.ProdutoRepository.Delete(produto);
        _uof.Commit();
        return Ok(response);
    }
}
