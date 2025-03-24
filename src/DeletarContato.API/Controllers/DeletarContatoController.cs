using DeletarContato.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DeletarContato.API.Controllers;

[Route("api/")]
[ApiController]
public class DeletarContatoController : ControllerBase
{
    // private readonly IContatoService _contatoService;
    private readonly ContactZoneDbContext _context;

    public DeletarContatoController(ContactZoneDbContext context)
    {
        _context = context;
    }

    [HttpDelete("deletar-contato-api/{id}")]
    public async Task<IActionResult> DeletarContato(int id)
    {
        var contato = await _context.Contatos.FirstOrDefaultAsync(x => x.Id == id);

        if (contato == null)
            return NotFound("Contato não encontrado.");

        _context.Contatos.Remove(contato);
        await _context.SaveChangesAsync();

        return Ok("Contato deletado com sucesso.");
    }
    
    [HttpGet("listar-contatos")]
    public async Task<IActionResult> GetAllContatos()
    {
        var contatos = await _context.Contatos.ToListAsync();
        return Ok(contatos);
    }
}