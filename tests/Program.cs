namespace TEST
{

    public class DataObject
    {
        public int? mins { get; set; }
        public string? price { get; set; }
    }

    public class Afluencia
    {
    }

    public class Observaciones
    {
    }

    public class Casa
    {
        public string? compra { get; set; }
        public string? venta { get; set; }
        public string? agencia { get; set; }
        public string? nombre { get; set; }
        public object? variacion { get; set; }
        public string? ventaCero { get; set; }
        public string? decimales { get; set; }
        public string? mejor_compra { get; set; }
        public string? mejor_venta { get; set; }
        public string? fecha { get; set; }
        public string? recorrido { get; set; }
        public Afluencia? afluencia { get; set; }
        public Observaciones? observaciones { get; set; }
    }

    public class Root
    {
        public Casa? casa { get; set; }
    }

    public class Program
    {
        public const string URLCrypto = "https://api.binance.com/api/v3/avgPrice";
 
        static void Main(string[] args)
        {
            string[] cryptos = { "BTC", "ETH", "LTC", "BNB", "ADA", "SOL", "LUNA" };

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
            if (cryptos.Contains(ingreso))
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
            }
            else
            {
                Console.WriteLine("No ingresó un código correcto");
            }
        }
    }
}