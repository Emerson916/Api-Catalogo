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
    private readonly IProdutoRepository _produtoRepository;
    private readonly IRepository<Produto> _repository;
    private readonly ILogger _logger;

    //Construtor
    public ProdutosController(IProdutoRepository produtoRepository, IRepository<Produto> repository, ILogger<ProdutosController> logger)
    {
        _produtoRepository = produtoRepository;
        _repository = repository;
        _logger = logger;
    }

    [HttpGet("categorias/{id}")]
    public ActionResult<IEnumerable<Produto>> GetProdutosCategoria(int id)
    {
          if (_produtoRepository == null) {
            return StatusCode(500, "Falha ao atualizar o produto");
        }
        var produtos = _produtoRepository.GetProdutosPorCategoriaRepository(id);

        if (produtos is null)
        {
            return NotFound();
        }

        return Ok(produtos);    
    }

    [HttpGet]
    public ActionResult<IQueryable<Produto>> GetProdutos()
    {
        var response = _repository.GetAll();

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
        var response = _repository.Get(c => c.ProdutoId == id);

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

        var response = _repository.Create(data);

         if (response is null)
        {
            return NotFound("Produto não criado...");
        }

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

        if (_repository == null) {
            return StatusCode(500, "Falha ao atualizar o produto");
        }

        var response = _repository.Update(data);

        if (response is null) {
            return StatusCode(500, "Falha ao atualizar o produto");
        }

      return Ok(response);
        
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteProduto(int id)
    {
        var produto = _repository.Get(p => p.ProdutoId == id);

        if (produto is null)
        {
            return NotFound("Produto não encontrado..");
        }

        var response = _repository.Delete(produto);
        return Ok(response);
    }
}
