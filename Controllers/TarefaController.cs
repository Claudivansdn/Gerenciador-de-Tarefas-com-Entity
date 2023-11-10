using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;
using System;
using System.Linq;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            try
            {
                var tarefa = _context.Tarefas.Find(id);

                if (tarefa == null)
                    return NotFound();

                return Ok(tarefa);
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao buscar a tarefa por ID." });
            }
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            try
            {
                var tarefas = _context.Tarefas.ToList();
                return Ok(tarefas);
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao buscar todas as tarefas." });
            }
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            try
            {
                var tarefas = _context.Tarefas.Where(x => x.Titulo.Contains(titulo)).ToList();

                if (tarefas.Count == 0)
                    return NotFound("Nenhuma tarefa encontrada com o título especificado.");

                return Ok(tarefas);
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao buscar as tarefas por título." });
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            try
            {
                var tarefas = _context.Tarefas.Where(x => x.Data.Date == data.Date).ToList();

                if (tarefas.Count == 0)
                    return NotFound("Nenhuma tarefa encontrada com a data especificada.");

                return Ok(tarefas);
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao buscar as tarefas por data." });
            }
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            try
            {
                var tarefas = _context.Tarefas.Where(x => x.Status == status).ToList();

                if (tarefas.Count == 0)
                    return NotFound($"Nenhuma tarefa encontrada com o status '{status}'.");

                return Ok(tarefas);
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao buscar as tarefas por status." });
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            try
            {
                _context.Tarefas.Add(tarefa);
                _context.SaveChanges();
                return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao criar a tarefa." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            try
            {
                var tarefaBanco = _context.Tarefas.Find(id);

                if (tarefaBanco == null)
                    return NotFound();

                if (tarefa.Data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

                // Atualizar as informações da tarefaBanco com a tarefa recebida via parâmetro
                tarefaBanco.Titulo = tarefa.Titulo;
                tarefaBanco.Descricao = tarefa.Descricao;
                tarefaBanco.Data = tarefa.Data;
                tarefaBanco.Status = tarefa.Status;

                _context.Tarefas.Update(tarefaBanco);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao atualizar a tarefa." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                var tarefaBanco = _context.Tarefas.Find(id);

                if (tarefaBanco == null)
                    return NotFound();

                _context.Tarefas.Remove(tarefaBanco);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception )
            {
                return StatusCode(500, new { Erro = "Ocorreu um erro ao excluir a tarefa." });
            }
        }
    }
}
