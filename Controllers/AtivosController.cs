using GUIDE.Models;
using GUIDE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Guide.Controllers;

[ApiController]
[Route("[controller]")]
public class AtivosController : ControllerBase
{
    private readonly AtivosRepository _ativosRepository;

    public AtivosController(AtivosRepository ativoRepository)
    {
        this._ativosRepository = ativoRepository;
    }

    [HttpGet]
    [Route("GetAtivos")]
    [Authorize]
    public async Task<ActionResult<List<AtivosRet>>> GetAtivos()
    {
        List<AtivosRet> ret = await _ativosRepository.GetAtivos();

        return ret;
    }

    [HttpPatch]
    [Route("UpdateDatabase")]
    [Authorize]
    public async Task AtualizaBaseAtivos()
    {
        await _ativosRepository.AtualizaBaseAtivos();
    }
}
