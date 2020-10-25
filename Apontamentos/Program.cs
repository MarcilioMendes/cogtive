using System;
using Apontamentos.Model;

namespace Apontamentos
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var ApontamentoList = Apontamento.ReadFile();
                Apontamento.CalcularGAPs(ApontamentoList);
                ApontamentoProducao.CalcularQtdProduzidas(ApontamentoList);
                ApontamentoManutencao.CalcularHorasManutencao(ApontamentoList);

                Console.WriteLine("\n\nFim da Aplicação, feche o console clicando no X.");
            }
            catch (Exception e)
            {
            }
            finally
            {
                Console.ReadLine();
            }
        }

    }
}
