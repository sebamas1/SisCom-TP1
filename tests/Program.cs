using TEST.models;
using System;
using System.Runtime.InteropServices;

namespace TEST
{

    public class Program
    {

        [DllImport("libtest.so", EntryPoint = "valoresCryptos")]
        static extern void valoresCryptos(string crypto1, string valor1, string crypto2, string valor2);

        public const string URLCrypto = "https://api.binance.com/api/v3/avgPrice";

        public static string[] cryptos = { "BTC", "ETH", "LTC", "BNB", "ADA", "SOL", "LUNA" };

        static async Task<decimal> getPrice(string ingreso)
        {
            /*
            ***********************************************Crypto*******************************************
            */
            string urlParametersCrypto = "?symbol=" + ingreso + "USDT";
            HttpClient client = new HttpClient();
            decimal valorCrypto = 0;
            //Respuesta
            HttpResponseMessage response = client.GetAsync(URLCrypto + urlParametersCrypto).Result;  //LLamada bloqueante. El programa espera que reciba la respuesta para continuar
            if (response.IsSuccessStatusCode)
            {
                // Parseamos la respuesta a nuestro objeto
                var dataObjects = response.Content.ReadAsAsync<DataObject>().Result; //La respuesta es un solo objeto JSON que se formatea a DataObject
                Console.WriteLine("U$D {0}", dataObjects.price.ToString().Replace(".", ","));
                valorCrypto = (dataObjects.price != null) ? decimal.Parse(dataObjects.price.ToString().Replace(".", ",")) : 0;
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            /*
            ***********************************************Dolar*******************************************
            */

            // Llamar a la cotización de dólar.
            string URLDolar = "https://www.dolarsi.com/api/api.php?type=valoresprincipales";
            decimal valorDolar = 0;
            //Respuesta
            HttpResponseMessage responseDolar = client.GetAsync(URLDolar).Result;  //LLamada bloqueante. El programa espera que reciba la respuesta para continuar
            if (responseDolar.IsSuccessStatusCode)
            {
                // Parseamos la respuesta a nuestro objeto
                List<Root> root = responseDolar.Content.ReadAsAsync<List<Root>>().Result; //La respuesta es un solo objeto JSON que se formatea a DataObject
                foreach (Root dataRoot in root)
                {
                    if (dataRoot.casa.nombre == "Dolar Blue")
                    {
                        Console.Write("AR$ {0}", dataRoot.casa.venta);
                        valorDolar = Decimal.Parse(dataRoot.casa.venta);
                    }
                }
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)responseDolar.StatusCode, responseDolar.ReasonPhrase);
            }
            //Cerrar las llamadas HttpClient ya que se instancia una única vez, sino tirar error.
            client.Dispose();
            Console.Write("\n-------------------------------\n");
            decimal resultado = (valorCrypto * valorDolar);
            string valorPesos = resultado.ToString("0.00");
            Console.WriteLine($"El valor de {ingreso} en AR$ es: ${valorPesos}");
            return valorCrypto;
        }

        public static string getName()
        {
            Console.Write("Ingresar la criptomoneda a buscar\n");
            string textoBienvenida = " ";
            for (int i = 0; i < cryptos.Length; i++)
            {
                textoBienvenida += cryptos[i] + "  ";
            };
            textoBienvenida += "\n";
            Console.Write(textoBienvenida);



            string? ingreso = Console.ReadLine();
            ingreso = (ingreso != null) ? ingreso.ToString().ToUpper() : "";

            return ingreso.ToString();
        }
        static void Main(string[] args)
        {

            string[] nombres = { "", "" };
            string[] valores = { "", "" };

            for (int i = 0; i < 2; i++)
            {
                nombres[i] = getName();
                if (cryptos.Contains(nombres[i]))
                {
                    
                    Task.Run(async () =>
                    {
                        decimal valor = await getPrice(nombres[i]);
                        valores[i] = valor.ToString();
                    }).GetAwaiter().GetResult();

                     valoresCryptos(nombres[0], valores[0], nombres[1], valores[1]);
                }
                else
                {
                    Console.WriteLine("No ingresó un código correcto");
                    i--;
                }
                
                Console.WriteLine("\n");
            }

           
             

        }
    }
}