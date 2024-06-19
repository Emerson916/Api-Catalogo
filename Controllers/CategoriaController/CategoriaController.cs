using APICatalogo.Context;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Controllers.CategoriaController;

[Route("[controller]")]
[ApiController]

public class CategoriaController : ControllerBase
{
    private readonly IcategoriaRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger _logger;

    public CategoriaController(IcategoriaRepository repository, IConfiguration configuration, ILogger<CategoriaController> logger)
    {
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    // [HttpGet("LerConfig")]
    // public string GetConfig()
    // {
    //     var valor1 = _configuration["chave1"];
    //     var valor2 = _configuration["chave2"];

    //     var secao1 = _configuration["secao1:chave2"];

    //     return $"Chave1 = {valor1} \nChave2 = {valor2} \n Secao = {secao1}";
    // }

    [HttpGet("produtos")]
    public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
    {
        _logger.LogInformation("##-/GET/-##");
        _logger.LogInformation("##-/PRODUTOS/-##");
        _logger.LogInformation("##-/GET/-##");
        var response = _repository.GetCategoriasComProdutos();

        if (response is null)
        {
            return NotFound("Categoria com produtos não encontrada");
        }

        return Ok(response);
    }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> GetCategorias()
    {

        var response = _repository.GetCategoriasRepository();

        if (response is null)
        {
            return NotFound("Categorias não encontradas");
        }

        return Ok(response);

    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetCategoriaById(int id)
    {
        var response = _repository.GetCategoriaByIdRepository(id);

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
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest();
        }

        var response = _repository.PostCategoriaRepository(data);

        return new CreatedAtRouteResult("ObterCategoria", new { id = response.CategoriaId }, response);
    }

    [HttpPut("{id:int}")]
    public ActionResult PutCategoria(int id, Categoria data)
    {
        if (data is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest();
        }

        var response = _repository.PutCategoriaRepository(id, data);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategoria(int id)
    {
        var response = _repository.DeleteCategoriaRepository(id);

        return Ok(response);
    }

}