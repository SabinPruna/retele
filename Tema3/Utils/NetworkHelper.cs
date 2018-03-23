using System;
using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Tema3.Utils {
    public class NetworkHelper {
        public static void GetMachineName() {
            Console.WriteLine($"Machine Name: {Environment.MachineName}");
        }

        public static string GetLocalIPAddress() {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList) {
                if (ip.AddressFamily == AddressFamily.InterNetwork ||
                    ip.AddressFamily == AddressFamily.InterNetworkV6) {
                    return ip.ToString();
                }
            }

            return "No address found!";
        }

        public static void GetIPAddressesByName(string computerName) {
            IPHostEntry host = Dns.GetHostEntry(computerName);
            foreach (IPAddress ip in host.AddressList) {
                Console.WriteLine(ip.ToString());
            }
        }

        public static void WebPageToFile(string webpageUrl) {
            WebClient client = new WebClient();
            string content = client.DownloadString(webpageUrl);

            client.DownloadFile(webpageUrl, @"E:\DirectWebPageDownload");

            File.WriteAllText(@"E:\contentForTema3", content);
        }
    }
}