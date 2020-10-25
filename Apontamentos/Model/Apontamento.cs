using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Configuration;
using System.IO;


namespace Apontamentos
{
    public class Apontamento
    {
        public int IdApontamento { get; private set; }
        public DateTime DataInicio { get; private set; }
        public DateTime DataFim { get; private set; }
        public int NumeroLote { get; private set; }
        public int IdEvento { get; private set; }
        public int Quantidade { get; private set; }
        
        /// <summary>
        /// Metodo estático que lê o arquivo data.csv que está no diretorio do programa executável e 
        /// retorna uma lista de apontamentos.
        /// </summary>
        /// <returns></returns>
        public static List<Apontamento> ReadFile()
        {
            var pathFile = ConfigurationManager.AppSettings["FilePath"];
            var apontamentoList = new List<Apontamento>();

            try
            {
                using (var fileStream = new FileStream(pathFile, FileMode.Open))
                using (var reader = new StreamReader(fileStream))
                {
                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        apontamentoList.Add(ParserFileToApontamento(line));
                    }
                }
                
                return apontamentoList;
            }
            catch (Exception e)
            {
                Console.WriteLine($" Falha ao ler o arquivo. {e.Message}");
                throw e;
            }

        }

        /// <summary>
        /// Metodo estatico que recebe um TimeSpan e retorna uma string no formato de hora: HH:MM:SS
        /// </summary>
        /// <param name="ts"></param>
        /// <returns></returns>
        protected static string ParserTimespanToDatetimeString(TimeSpan ts)
        {
            var days = ts.Days;
            var gapTime = "00:00:00";

            try
            {
                if (days == 0)
                    gapTime = ts.ToString();
                else
                {
                    var Totalhours = (days * 24) + ts.Hours;
                    gapTime = $"{Totalhours}:{ts.Minutes.ToString().PadLeft(2,'0')}:{ts.Seconds.ToString().PadLeft(2,'0')}";
                }
                return gapTime;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ocorreu um erro ao conveter timespan em datetime string -> {e.Message}");
                return gapTime;
            }
        }

        /// <summary>
        /// Método estatico que recebe uma lista de apontamentos.
        /// No loop, armazena a dataFim do apontamento anterior e compara com a dataInicio do apontamento atual.
        /// Se a dataInicio do apontamento for maior que a dataFim do apontamento anteriro, houve GAP.
        /// </summary>
        /// <param name="apontamentos"></param>
        internal static void CalcularGAPs(List<Apontamento> apontamentos)
        {
            int qtdGaps = 0;
            TimeSpan periodoTotal = new TimeSpan(0, 0, 0, 0);
            DateTime dataFimAnterior = new DateTime();
            int diferencaDataInt;

            try
            {
                foreach (var apontamento in apontamentos)
                {
                    if (apontamento == apontamentos.First())
                    {
                        dataFimAnterior = apontamento.DataFim;
                        continue;
                    }
                    diferencaDataInt = DateTime.Compare(apontamento.DataInicio, dataFimAnterior);
                    if (diferencaDataInt > 0)
                    {
                        periodoTotal += (apontamento.DataInicio - dataFimAnterior);
                        qtdGaps += 1;
                    }
                    dataFimAnterior = apontamento.DataFim;
                }
                Console.WriteLine("\n1. Funcionalidade Calcular GAPs");
                Console.WriteLine($"Quantidade de GAPs: {qtdGaps.ToString("0,0", CultureInfo.CreateSpecificCulture("el-GR"))}");
                Console.WriteLine($"Periodo Total: {ParserTimespanToDatetimeString(periodoTotal)}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao Calcular o GAP -> {e.Message}");
            }
        }

        // metodo estatico que recebe uma linha do arquivo data.csv e armazena as informações em um tipo da classe Apontamento 
        protected static Apontamento ParserFileToApontamento(string line)
        {
            string[] fileds = line.Split(ConfigurationManager.AppSettings["Separator"]);
            var numeroLote = 0;

            try
            {
                if (!String.IsNullOrEmpty(fileds[3])) numeroLote = int.Parse(fileds[3]);

                var apontamento = new Apontamento
                {
                    IdApontamento = int.Parse(fileds[0]),
                    DataInicio = DateTime.Parse(fileds[1]),
                    DataFim = DateTime.Parse(fileds[2]),
                    NumeroLote = numeroLote,
                    IdEvento = int.Parse(fileds[4]),
                    Quantidade = int.Parse(fileds[5])
                };


                return apontamento;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Erro ao Criar o Apontamento -> {e.Message}");
                throw e;
            }

        }


    }

}
