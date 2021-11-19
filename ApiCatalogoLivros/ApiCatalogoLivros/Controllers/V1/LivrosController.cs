using ApiCatalogoLivros.Exceptions;
using ApiCatalogoLivros.InputModel;
using ApiCatalogoLivros.Services;
using ApiCatalogoLivros.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Controllers.V1
{
    [Route("api/V1/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivrosController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        /// <summary>
        /// Buscar todos os livros de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os livros sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de reistros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de livros</response>
        /// <response code="204">Caso não haja livro</response>  
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var livros = await _livroService.Obter(pagina, quantidade);
            if (livros.Count() == 0)
                return NoContent();

            return Ok(livros);
        }

        /// <summary>
        /// Buscar um livro pelo seu Id
        /// </summary>
        /// <param name="idLivro">Id do livro buscado</param>
        /// <response code="200">Retorna o livro filtrado</response>
        /// <response code="204">Caso não haja livro com este id</response> 
        [HttpGet("{idLivro:guid}")]
        public async Task<ActionResult<LivroViewModel>> Obter([FromRoute] Guid idLivro)
        {
            var livro = await _livroService.Obter(idLivro);

            if (livro == null)
                return NoContent();

            return Ok(livro);
        }

        /// <summary>
        /// Inserir um livro no catálogo
        /// </summary>
        /// <param name="LivroInputModel">Dados do livro a ser inserido</param>
        /// <response code="200">Caso o livro seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um livro com o mesmo nome para a mesma editora</response> 
        [HttpPost]
        public async Task<ActionResult<LivroViewModel>> InserirLivro([FromBody]LivroInputModel livroImputModel)
        {
            try
            {
                var livro = await _livroService.Inserir(livroImputModel);
                return Ok(livro);
            }
            catch (LivroJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um jogo com esse nome para esta editora.");
            }
           
        }

        [HttpPut("{idLivro:guid}")]
        public async Task<ActionResult> AtualizarLivro([FromRoute] Guid idLivro, [FromBody] LivroInputModel livroInputModel)
        {
            try
            {
                await _livroService.Atualizar(idLivro, livroInputModel);
                return Ok();
            }
            catch (LivroNaoCadastradoException ex)
            {
                return NotFound("Esse livro não consta nos registros.");
            }

        }

        [HttpPatch("{idLivro:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarLivro([FromRoute] Guid idLivro, [FromRoute] double preco)
        {
            try
            {
                await _livroService.Atualizar(idLivro, preco);

                return Ok();
            }
            catch (LivroNaoCadastradoException ex)
            {
                return NotFound("Esse livro não consta nos registros.");
            }

        }

        [HttpDelete("{idLivro:guid}")]
        public async Task<ActionResult<LivroViewModel>> Deletar([FromRoute] Guid idLivro)
        {
            try
            {
                await _livroService.Remover(idLivro);

                return Ok();
            }
            catch (LivroNaoCadastradoException ex)
            {
                return NotFound("Esse livro não consta nos registros.");
            }
        }
    }
}
