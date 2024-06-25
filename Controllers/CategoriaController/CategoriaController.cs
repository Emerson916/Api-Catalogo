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
    private readonly IRepository<Categoria> _repository;
    private readonly ILogger _logger;

    public CategoriaController(IcategoriaRepository repository, ILogger<CategoriaController> logger)
    {
        _repository = repository;
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

    // [HttpGet("produtos")]
    // public ActionResult<IEnumerable<Categoria>> GetCategoriaProdutos()
    // {
    //     _logger.LogInformation("##-/GET/-##");
    //     _logger.LogInformation("##-/PRODUTOS/-##");
    //     _logger.LogInformation("##-/GET/-##");
    //     var response = _produtoRepository.GetCategoriasComProdutos();

    //     if (response is null)
    //     {
    //         return NotFound("Categoria com produtos não encontrada");
    //     }

    //     return Ok(response);
    // }

    [HttpGet]
    [ServiceFilter(typeof(ApiLoggingFilter))]
    public ActionResult<IEnumerable<Categoria>> GetCategorias()
    {

        // var response = _repository.GetCategoriasRepository();
        var response = _repository.GetAll();

        if (response is null)
        {
            return NotFound("Categorias não encontradas");
        }

        return Ok(response);

    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<Categoria> GetCategoriaById(int id)
    {
        var response = _repository.Get(c => c.CategoriaId == id);

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

        if (_repository == null)  {
            return StatusCode(500, "Falha ao atualizar o produto");
        }

        var response = _repository.Create(data);

         if (response is null)
        {
            return NotFound("Categoria não criada...");
        }

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

        var response = _repository.Update(data);

        return Ok(response);
    }

    [HttpDelete("{id:int}")]
    public ActionResult DeleteCategoria(int id)
    {
        var categoria = _repository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrado...");
        }

        var response = _repository.Delete(categoria);

        return Ok(response);
    }

}