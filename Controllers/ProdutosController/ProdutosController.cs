using APICatalogo.DTOs;
using APICatalogo.Models;
using APICatalogo.Pagination;
using APICatalogo.Repository;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers.ProdutosController;

[Route("[controller]")]
[ApiController]
public class ProdutosController : ControllerBase
{
    //variavel privada que só pode ser lida
    //private/public - definindo que só pode ser lida - tipagem da variavel - nome (Variaveis privadas no C# são iniciadas com '_')
    private readonly IUnitOfWork _uof;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    //Construtor
    public ProdutosController(IUnitOfWork uof, IMapper mapper, ILogger<ProdutosController> logger)
    {
        _uof = uof;
        _mapper = mapper;
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

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(produtos);

        return Ok(produtosDTO);
    }

    [HttpGet("pagination")]
    public ActionResult<IEnumerable<ProdutoDTO>> Get([FromQuery] ProdutosParameters produtosParameters)
    {
        var response = _uof.ProdutoRepository.GetProdutos(produtosParameters);

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(response);

        return Ok(produtosDTO);
    }

    [HttpGet]
    public ActionResult<IQueryable<ProdutoDTO>> GetProdutos()
    {
        var response = _uof.ProdutoRepository.GetAll();

        if (response is null)
        {
            return NotFound("Produtos não encontrados");
        }

        var produtosDTO = _mapper.Map<IEnumerable<ProdutoDTO>>(response);

        return Ok(produtosDTO);
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

        var produtosDTO = _mapper.Map<ProdutoDTO>(response);

        return Ok(produtosDTO);
    }

    [HttpPost]
    public ActionResult<ProdutoDTO> PostProduto(ProdutoDTO data)
    {
        if (data is null)
        {
            return BadRequest();
        }

        var produto = _mapper.Map<Produto>(data);

        var response = _uof.ProdutoRepository.Create(produto);

        if (response is null)
        {
            return NotFound("Produto não criado...");
        }

        _uof.Commit();
        return new CreatedAtRouteResult("ObterProduto", new { id = response.ProdutoId }, response);
    }

    /// <summary>
    /// Utilize esse body para fazer as alterações nas requisições: 
    /// [
    ///     { "op": "replace", "path": "/estoque", "value": 100 },
    ///     { "op": "replace", "path": "/datacadastro", "value": "2025-09-03T00:00:00Z" }
    /// ]
    /// </summary>
    [HttpPatch("{id:int}")]
    public ActionResult<ProdutoDTOUpdateResponse> Patch(int id, [FromBody] JsonPatchDocument<ProdutoDTOUpdateRequest> patchProdutoDTO)
    {
        if (patchProdutoDTO is null || id <= 0)
        {
            return BadRequest();
        }

        var produto = _uof.ProdutoRepository.Get(c => c.ProdutoId == id);

        if (produto is null)
        {
            return NotFound();
        }

        var produtoUpdateRequest = _mapper.Map<ProdutoDTOUpdateRequest>(produto);

        patchProdutoDTO.ApplyTo(produtoUpdateRequest, ModelState);

        if (!ModelState.IsValid)
        {
            Console.WriteLine("ModelState is invalid.");
            return BadRequest(ModelState);
        }

        if (!TryValidateModel(produtoUpdateRequest))
        {
            Console.WriteLine("TryValidateModel failed.");
            return BadRequest(ModelState);
        }

        _mapper.Map(produtoUpdateRequest, produto);
        _uof.ProdutoRepository.Update(produto);
        _uof.Commit();

        return Ok(_mapper.Map<ProdutoDTOUpdateResponse>(produto));
    }

    [HttpPut("{id:int}")]
    public ActionResult<ProdutoDTO> PutProduto(int id, ProdutoDTO data)
    {
        if (id != data.ProdutoId)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest();
        }

        var produto = _mapper.Map<Produto>(data);

        var response = _uof.ProdutoRepository.Update(produto);

        if (response is null)
        {
            return StatusCode(500, "Falha ao atualizar o produto");
        }

        _uof.Commit();

        var produtoDTO = _mapper.Map<ProdutoDTO>(response);

        return Ok(produtoDTO);

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
