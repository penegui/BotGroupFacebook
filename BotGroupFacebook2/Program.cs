using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;



namespace BotGroupFacebook2
{
    class Program
    {
        static void Main(string[] args)
        {
            Request();
        }

        public static void Request()
        {
            string to_id = "0000000000000000"; // Contêm a ID do Grupo do Facebook, onde será publicado.          
            string dadosPost = ""; // Aqui vai os dados Pots capturados no request feito pela proxy... e no lugar do compo to_id deve ser colocado a variavel "to_id" instanciada acima.

            var dados = Encoding.UTF8.GetBytes(dadosPost);

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.facebook.com/api/graphql/");

            //pode confirgurar a proxi para ver o susuntado mais facilmente.
            //var proxyObject = new WebProxy("http://127.0.0.1:8080");
            //request.Proxy = proxyObject;

            request.Headers = Headers();

            request.Method = "POST";
            request.ContentLength = dados.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.125 Safari/537.36";
            //request.Credentials = CredentialCache.DefaultCredentials;



            using (var stream = request.GetRequestStream())
            {
                stream.Write(dados, 0, dados.Length);
                stream.Close();
            }           

            ServicePointManager.ServerCertificateValidationCallback = ((sender, certificate, chain, sslPolicyErrors) => true);

            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            var streamDados = response.GetResponseStream();
            StreamReader reader = new StreamReader(streamDados);
            object objResponse = reader.ReadToEnd();            
            Console.WriteLine(objResponse);
        }

        public static WebHeaderCollection Headers()
        {
            var headers = new WebHeaderCollection();

            headers["Connection"] = "close";
            //headers["Viewport-Width"] = "1164";
            headers["Accept"] = "*/*";
            headers["Origin"] = "https://www.facebook.com";
            headers["Sec-Fetch-Site"] = "same-origin";
            headers["Sec-Fetch-Mode"] = "cors";
            headers["Sec-Fetch-Dest"] = "empty";
            headers["Referer"] = "https://www.facebook.com";
            headers["Accept-Encoding"] = "gzip, deflate";
            headers["Accept-Language"] = "pt-BR,pt;q=0.9,en-US;q=0.8,en;q=0.7";
            headers["Cookie"] = ""; // deve ser adicionado todos os cookies capiturados pela proxy

            return headers;
        }

    }
}
