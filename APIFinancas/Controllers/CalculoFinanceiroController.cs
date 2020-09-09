using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using APIFinancas.Models;

namespace APIFinancas.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CalculoFinanceiroController : ControllerBase
    {

        [HttpGet("juroscompostos")]
        [ProducesResponseType(typeof(Emprestimo), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(FalhaCalculo), (int)HttpStatusCode.BadRequest)]
        public ActionResult<Emprestimo> Get(
            [FromServices]ILogger<CalculoFinanceiroController> logger,
            double valorEmprestimo, int numMeses, double percTaxa)
        {
            if (valorEmprestimo <= 0)
                return new BadRequestObjectResult(new FalhaCalculo() { Mensagem = "O Valor do Empréstimo deve ser maior do que zero!" });

            if (numMeses <= 0)
                return new BadRequestObjectResult(new FalhaCalculo() { Mensagem = "O Número de Meses deve ser maior do que zero!" });

            if (percTaxa <= 0)
                return new BadRequestObjectResult(new FalhaCalculo() { Mensagem = "O Percentual da Taxa de Juros deve ser maior do que zero!" });

            logger.LogInformation(
                "Recebida nova requisição|" +
               $"Valor do empréstimo: {valorEmprestimo}|" +
               $"Número de meses: {numMeses}|" +
               $"% Taxa de Juros: {percTaxa}");

            double valorFinalJuros =
                CalculoFinanceiro.CalcularValorComJurosCompostos(
                    valorEmprestimo, numMeses, percTaxa);
            logger.LogInformation($"Valor Final com Juros: {valorFinalJuros}");

            return new Emprestimo()
            {
                ValorEmprestimo = valorEmprestimo,
                NumMeses = numMeses,
                TaxaPercentual = percTaxa,
                ValorFinalComJuros = valorFinalJuros
            };
        }
    }
}