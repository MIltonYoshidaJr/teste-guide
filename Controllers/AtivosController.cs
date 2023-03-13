using GUIDE.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Guide.Controllers;

[ApiController]
[Route("[controller]")]
public class AtivosController : ControllerBase
{
    private readonly DataBaseContext _context;

    public AtivosController(DataBaseContext context)
    {
        this._context = context;
    }

    [HttpGet]
    [Route("GetAtivos")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<AtivosRet>>> GetAtivos()
    {
        List<Ativos> lstAtivos = await _context.Ativos.ToListAsync();
        lstAtivos = lstAtivos.TakeLast(30).ToList();

        List<AtivosRet> lstRetorno = new List<AtivosRet>();

        for (int i = 0; i < lstAtivos.Count; i++)
        {
            Decimal diaMenosUm = 0;
            Decimal primeiraData = 0;

            if (i > 0)
            {
                if (lstAtivos[i - 1].Indicador != 0)
                {
                    diaMenosUm = Math.Abs(((lstAtivos[i].Indicador / lstAtivos[i - 1].Indicador) * 100) - 100);
                    diaMenosUm = lstAtivos[i].Indicador < lstAtivos[i - 1].Indicador ? diaMenosUm * -1 : diaMenosUm;

                    primeiraData = Math.Abs(((lstAtivos[i].Indicador / lstAtivos[0].Indicador) * 100) - 100);
                    primeiraData = lstAtivos[i].Indicador < lstAtivos[0].Indicador ? primeiraData * -1 : primeiraData;
                }
            }

            AtivosRet r = new AtivosRet();
            r.Id = i + 1;
            r.Data = lstAtivos[i].Data.Date;
            r.Valor = lstAtivos[i].Indicador;
            r.VarDiaMenosUm = Math.Round(diaMenosUm, 2);
            r.VarPrimeiraData = Math.Round(primeiraData, 2);

            lstRetorno.Add(r);
        }

        return lstRetorno;
    }

    [HttpPatch]
    [Route("UpdateDatabase")]
    [Authorize]
    public async Task AtualizaBaseAtivos()
    {
        Yahoo result = new Yahoo();

        using (var httpClient = new HttpClient())
        {
            DateTime currentTime = DateTime.UtcNow;
            currentTime = currentTime.AddDays(-60);
            long unixTime = ((DateTimeOffset)currentTime).ToUnixTimeSeconds();

            Console.WriteLine(unixTime);

            using (var response = await httpClient.GetAsync($"https://query2.finance.yahoo.com/v8/finance/chart/PETR4.SA?period1={unixTime.ToString()}&period2=9999999999&interval=1d"))
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<Yahoo>(apiResponse);

                List<Double> lstValores = new List<double>();
                List<int> lstTimeStamps = new List<int>();

                if (result != null)
                {
                    foreach (Quote q in result.chart.result[0].indicators.quote)
                    {
                        lstValores = q.open;
                    }

                    foreach (Result r in result.chart.result)
                    {
                        lstTimeStamps = r.timestamp;
                    }

                    int i = 0;
                    DateTime dataInicial = Convert.ToDateTime("1970-01-01");

                    await _context.Database.ExecuteSqlRawAsync("TRUNCATE TABLE Ativos");

                    foreach (Double d in lstValores)
                    {
                        Console.WriteLine($"Data: {dataInicial.AddSeconds(lstTimeStamps[i]).ToString("yyyy-MM-dd")} | Valor: {d}");

                        Ativos ativo = new Ativos();
                        ativo.Data = dataInicial.AddSeconds(lstTimeStamps[i]);
                        ativo.Indicador = Convert.ToDecimal(d);

                        _context.Ativos.Add(ativo);

                        i++;
                    }

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}
