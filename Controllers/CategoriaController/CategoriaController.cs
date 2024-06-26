using APICatalogo.Context;
using APICatalogo.DTOs;
using APICatalogo.Filters;
using APICatalogo.Models;
using APICatalogo.Repository;
using Microsoft.AspNetCore.Mvc;

namespace APICatalogo.Controllers.CategoriaController;

[Route("[controller]")]
[ApiController]

public class CategoriaController : ControllerBase
{
    private readonly IUnitOfWork _uof;
    private readonly ILogger _logger;

    public CategoriaController(IUnitOfWork uof, ILogger<CategoriaController> logger)
    {
        _uof = uof;
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
    public ActionResult<IEnumerable<CategoriaDTO>> GetCategorias()
    {

        // var response = _repository.GetCategoriasRepository();
        var response = _uof.CategoriaRepository.GetAll();

        if (response is null)
        {
            return NotFound("Categorias não encontradas");
        }

        var categoriasDTO = new List<CategoriaDTO>();

        foreach(var categoria in response){
            var categoriaDTO = new CategoriaDTO{
                CategoriaId = categoria.CategoriaId,
                Nome = categoria.Nome,
                ImageUrl = categoria.ImageUrl
            };
            categoriasDTO.Add(categoriaDTO);
        }

        return Ok(categoriasDTO);

    }

    [HttpGet("{id:int}", Name = "ObterCategoria")]
    public ActionResult<CategoriaDTO> GetCategoriaById(int id)
    {
        var response = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (response is null)
        {
            return NotFound("Categorias não encontrada");
        }

        var categoraDTO = new CategoriaDTO()
        {
            CategoriaId = response.CategoriaId,
            Nome = response.Nome,
            ImageUrl = response.ImageUrl
        };

        return Ok(categoraDTO);
    }

    [HttpPost]
    public ActionResult<CategoriaDTO> PostCategoria(CategoriaDTO data)
    {
        if (data is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest();
        }

        var categoria = new Categoria()
        {
            CategoriaId = data.CategoriaId,
            Nome = data.Nome,
            ImageUrl = data.ImageUrl
        };

        var response = _uof.CategoriaRepository.Create(categoria);
        _uof.Commit();

        if (response is null)
        {
            return NotFound("Categoria não criada...");
        }

        var categoriaDTO = new CategoriaDTO()
        {
            CategoriaId = categoria.CategoriaId,
            Nome = categoria.Nome,
            ImageUrl = categoria.ImageUrl
        };

        return new CreatedAtRouteResult("ObterCategoria", new { id = categoriaDTO.CategoriaId }, categoriaDTO);
    }

    [HttpPut("{id:int}")]
    public ActionResult<CategoriaDTO> PutCategoria(int id, CategoriaDTO data)
    {
        if (data is null)
        {
            _logger.LogWarning($"Dados inválidos...");
            return BadRequest();
        }


        var categoria = new Categoria()
        {
            CategoriaId = data.CategoriaId,
            Nome = data.Nome,
            ImageUrl = data.ImageUrl
        };

        var response = _uof.CategoriaRepository.Update(categoria);
        _uof.Commit();

        if (response is null)  {
            return NotFound("Categoria não atualizada...");
        }

        var categoriaDTO = new CategoriaDTO()
        {
            CategoriaId = response.CategoriaId,
            Nome = response.Nome,
            ImageUrl = response.ImageUrl
        };

        return Ok(categoriaDTO);
    }

    [HttpDelete("{id:int}")]
    public ActionResult<CategoriaDTO> DeleteCategoria(int id)
    {
        var categoria = _uof.CategoriaRepository.Get(c => c.CategoriaId == id);

        if (categoria is null)
        {
            return NotFound("Categoria não encontrado...");
        }

        var response = _uof.CategoriaRepository.Delete(categoria);
        _uof.Commit();

        
        if (response is null)  {
            return NotFound("Categoria não excluida...");
        }

        var categoriaDTO = new CategoriaDTO()
        {
            CategoriaId = response.CategoriaId,
            Nome = response.Nome,
            ImageUrl = response.ImageUrl
        };

        return Ok(categoriaDTO);
    }

}