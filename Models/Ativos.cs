using System.ComponentModel.DataAnnotations;

namespace GUIDE.Models;

public class Ativos
{
    [Key]
    public int AtivoId { get; set; }

    public DateTime Data { get; set; }

    public Decimal Indicador { get; set; }
}

public class AtivosRet
{
    public int Id {get; set;}

    public DateTime Data {get; set;}

    public Decimal Valor {get; set;}

    public Decimal VarDiaMenosUm {get; set;}

    public Decimal VarPrimeiraData {get; set;}
}