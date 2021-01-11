using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Fermezza.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Fermezza
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //First of all, read certificate from certs path and save a pfx there
            var privateKey = PEMHelpers.GetBytesFromPEM(File.ReadAllText("certs/privkey.pem"), "PRIVATE KEY");
            var cert = new X509Certificate2("certs/fullchain.pem");
            var rsa = RSA.Create();
            rsa.ImportPkcs8PrivateKey(privateKey, out _);
            cert = cert.CopyWithPrivateKey(rsa);
            var pfxCert = cert.Export(X509ContentType.Pfx, "superPwd");
            File.WriteAllBytes("certs/myPfx.pfx", pfxCert);

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
