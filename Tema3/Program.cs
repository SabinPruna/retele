using System;
using Tema3.Utils;

namespace Tema3 {
    public class Program {
        private static void Main() {

            NetworkHelper.GetMachineName();

            NetworkHelper.GetLocalIPAddress();

            NetworkHelper.GetIPAddressesByName(Environment.MachineName);

            NetworkHelper.WebPageToFile("https://www.google.ro/?gws_rd=cr&dcr=0&ei=DHW1Wr2mL4SrswH6vr_ADg");
        }
    }
}