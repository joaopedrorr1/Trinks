using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConsumirApi
{
    class Program
    {
        public static HttpClient webClient = new HttpClient();
        IEnumerable<ConsumirProcesso> processo;
        IEnumerable<ConsumirCliente> cliente;
        static void Main(string[] args)
        {
            Program program = new Program();

            program.resposta1();
            program.resposta2();
            program.resposta3();
            //program.resposta4();
            program.resposta5();
            program.resposta6();
        }

        private void acessarResultProcesso()
        { 
            HttpResponseMessage webResponse = webClient.GetAsync("https://localhost:5001/api/processo").Result;
            processo = webResponse.Content.ReadAsAsync<IEnumerable<ConsumirProcesso>>().Result;
        }

        private void acessarResultCliente()
        {
            HttpResponseMessage webResponse = webClient.GetAsync("https://localhost:5001/api/cliente").Result;
            cliente = webResponse.Content.ReadAsAsync<IEnumerable<ConsumirCliente>>().Result;
        }

        private void resposta1()
        {
            //1) Calcular a soma dos processos ativos.A aplicação deve retornar R$ 1.087.000,00
            acessarResultProcesso();
            acessarResultCliente();

            decimal soma = 0;

            var testeResultado = from consumirProcesso in processo
                                 join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                 where consumirProcesso.Status == "Ativo"
                                 select new
                                 {
                                     ValorProcesso = consumirProcesso.Valor
                                 };

            foreach (var item in testeResultado)
            {
                soma = item.ValorProcesso + soma;
                
            }
            Console.WriteLine("Questão 1 - Valor total de processos ativos R$: {0}", soma);
        }

        private void resposta2()
        {
            //2) Calcular a a média do valor dos processos no Rio de Janeiro para o Cliente "Empresa A". A aplicação deve retornar R$ 110.000,00.
            acessarResultProcesso();
            acessarResultCliente();

            decimal soma = 0;
            int contador = 0;
            decimal media = 0;
            var testeResultado = from consumirProcesso in processo
                                 join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                 where consumirCliente.Nome == "Empresa A" && consumirProcesso.Uf == "Rio de Janeiro"
                                 select new
                                 {
                                     ValorProcesso = consumirProcesso.Valor
                                 };

            foreach (var item in testeResultado)
            {
                soma = item.ValorProcesso + soma;
                contador++;
            }

            media = soma / contador;

            Console.WriteLine("Questão 2 - A média de valor é R$: {0}", media);
        }
        private void resposta3()
        {
            //3) Calcular o Número de processos com valor acima de R$ 100.000,00. A aplicação deve retornar 2.
            acessarResultProcesso();
            acessarResultCliente();
            int contador = 0;

            var testeResultado = from consumirProcesso in processo
                                 join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                 where consumirProcesso.Valor > 100000
                                 select new
                                 {
                                    consumirProcesso.Id
                                 };

            foreach (var item in testeResultado)
            {
                contador++;
            }
            Console.WriteLine("Questão 3 - O número de processos com o valor acima de R$: 100.000,00 é: {0}", contador);
        }


        private void resposta4()
        {
            //4) Obter a lista de Processos de Setembro de 2007. A aplicação deve retornar uma lista com somente o Processo “00010TRABAM”.
            acessarResultProcesso();
            acessarResultCliente();

            var testeResultado = from consumirProcesso in processo
                                 join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                 where DateTime.ParseExact(consumirProcesso.Criacao, "MM-dd-yyyy hh:mm:ss tt", CultureInfo.InvariantCulture).Month == 09 
                                 //&& DateTime.ParseExact(consumirProcesso.Criacao, "dd-MM-yyyy", CultureInfo.InvariantCulture).Year == 2007
                                 select new
                                 {
                                     NomeCliente = consumirCliente.Nome,
                                     CnpjCliente = consumirCliente.Cnpj,
                                     UfCliente = consumirCliente.Uf,
                                     ProcessoNumero = consumirProcesso.NumeroProcesso,
                                     ValorProcesso = consumirProcesso.Valor,
                                     UfProcesso = consumirProcesso.Uf,
                                     DataInicio = consumirProcesso.Criacao,
                                     StatusProcesso = consumirProcesso.Status
                                 };

            foreach (var item in testeResultado)
            {
                Console.WriteLine("Questão 4 - Processo {0} número {1} no {2} no valor de R$: {3}, iniciado em {4}",
                    item.StatusProcesso, item.ProcessoNumero, item.UfProcesso, item.ValorProcesso, item.DataInicio);
            }
        }
        private void resposta5()
        {
            //5) Obter a lista de processos no mesmo estado do cliente, para cada um dos clientes.
            //A aplicação deve retornar uma lista com os processos de número
            //“00001CIVELRJ”,”00004CIVELRJ” para o Cliente "Empresa A" e “00008CIVELSP”,”00009CIVELSP” para o o Cliente "Empresa B".
            acessarResultProcesso();
            acessarResultCliente();

            var testeResultado = (from consumirProcesso in processo
                                  join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                  where consumirCliente.Id == 1 && consumirProcesso.Uf == "Rio de Janeiro"
                                  select new
                                  {
                                      NomeCliente = consumirCliente.Nome,
                                      ProcessoNumero = consumirProcesso.NumeroProcesso
                                  })
                                 .Union
                                 (from consumirProcesso in processo
                                  join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                  where consumirCliente.Id == 2 && consumirProcesso.Uf == "São Paulo"
                                  select new
                                  {
                                      NomeCliente = consumirCliente.Nome,
                                      ProcessoNumero = consumirProcesso.NumeroProcesso
          
                                  });
            foreach (var item in testeResultado)
            {
                //Console.WriteLine("Questão 5 - Processo número {0} do cliente {1}", item.ProcessoNumero, item.NomeCliente);
            }
        }
        private void resposta6()
        {
            //6) Obter a lista de processos que contenham a sigla “TRAB”. A aplicação deve retornar uma lista com os processos “00003TRABMG” e “00010TRABAM”
            acessarResultProcesso();
            acessarResultCliente();

            var testeResultado = from consumirProcesso in processo
                                 join consumirCliente in cliente on consumirProcesso.ClienteID equals consumirCliente.Id
                                 where consumirProcesso.NumeroProcesso.Contains("TRAB")
                                 select new
                                 {
                                     NomeCliente = consumirCliente.Nome,
                                     CnpjCliente = consumirCliente.Cnpj,
                                     UfCliente = consumirCliente.Uf,
                                     ProcessoNumero = consumirProcesso.NumeroProcesso,
                                     ValorProcesso = consumirProcesso.Valor,
                                     UfProcesso = consumirProcesso.Uf,
                                     DataInicio = consumirProcesso.Criacao,
                                     StatusProcesso = consumirProcesso.Status
                                 };

            foreach (var item in testeResultado)
            {
                Console.WriteLine("Questão 6 - Processo {0} número {1} no {2} no valor de R$: {3}, iniciado em {4}",
                    item.StatusProcesso, item.ProcessoNumero, item.UfProcesso, item.ValorProcesso, item.DataInicio);
            }

            Console.ReadKey();
        }
    }
}
