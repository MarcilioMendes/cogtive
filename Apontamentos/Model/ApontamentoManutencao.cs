using System;
using System.Collections.Generic;
using System.Linq;

namespace Apontamentos.Model
{
    public class ApontamentoManutencao : Apontamento
    {
        protected static int[] GetIDsEventosMantencao() { return new[] { 19 }; }
        /// <summary>
        /// Método estatico que recebe uma lista de apontamentos. 
        /// Cria uma nova lista apenas com apontamentos de eventos de manutenção, evento de IdEvento 19
        /// Faz-se um loop nos apontamentos e subtrai-se a dataFim da dataInicio de cada evento de manutençao,
        /// encontrando a quatidade total de horas de manutenção.
        /// </summary>
        /// <param name="apontamentoList"></param>
        internal static void CalcularHorasManutencao(List<Apontamento> apontamentoList)
        {
            TimeSpan periodoTotal = new TimeSpan(0, 0, 0, 0);

            try
            {
                var apontamentoHorasManutencaoList = apontamentoList.FindAll(it => GetIDsEventosMantencao().Contains(it.IdEvento));

                foreach (var apontamento in apontamentoHorasManutencaoList)
                {
                    periodoTotal += (apontamento.DataFim - apontamento.DataInicio);
                }
                Console.WriteLine("\n3. Funcionalidade Calcular Horas de Manutenção");
                Console.WriteLine($"Periodo total de manutenção: {ParserTimespanToDatetimeString(periodoTotal)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao Calcular o periodo total de manutenção{e.Message}");
            }
        }

    }
}
