using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace Apontamentos.Model
{
    public class ApontamentoProducao : Apontamento
    {
        // poderia ter deixado esses 3 campos aqui, mas, nesse caso, resolvi deixar na super classe.
        //    public int NumeroLote { get; set; }
        //    public int IdEvento { get; set; }
        //    public int Quantidade { get; set; }

        protected static int[] GetIDsEventosProducao() { return new[] { 1, 2 }; }

        /// <summary>
        /// Método estatico que recebe uma lista de apontamentos. 
        /// Cria uma nova lista apenas com apontamentos de eventos de produção, eventos de IdEvento 1 e 2.
        /// Atraves da função agregada SUM, calcula a quantidade total produzida da lista.
        /// Dessa lista de apontamentos de produção, agrupa-se o campo NumeroLote e soma suas quantidades e 
        /// cria-se um dicionario com base nesses campos. Onde a Key é o NumeroLote e o Value é a soma das quantidades de cada lote.
        /// </summary>
        /// <param name="apontamentoList"></param>
        internal static void CalcularQtdProduzidas(List<Apontamento> apontamentoList)
        {
            var index = 1;
            try
            {
                var apontamentoProducaoList = apontamentoList.FindAll(it => GetIDsEventosProducao().Contains(it.IdEvento));

                Console.WriteLine("\n2. Funcionalidade Calcular Quantidades Produzidas");

                var qtdProduzidas = apontamentoProducaoList.Sum(it => it.Quantidade);
                Console.WriteLine($"Quantidade total produzida: {qtdProduzidas.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))}");
                
                var lotes = apontamentoProducaoList
                    .GroupBy(item => item.NumeroLote)
                    .ToDictionary(g => g.Key, g => g.Sum(s => s.Quantidade));
                
                var firstLotes = lotes.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value).Take(3);

                foreach (var item in firstLotes)
                {
                    Console.WriteLine($"{index}º Lote {item.Key} produziu {item.Value.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))}");
                    index++;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao Calcular a quantidade total produzida -> {e.Message}");
            }
        }

    }

}
