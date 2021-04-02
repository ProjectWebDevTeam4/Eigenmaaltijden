using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Eigenmaaltijden.wwwroot.includes
{
    public class MailManager
    {
        private static string CallMailChimpApi()
        {
            using(var http = new HttpClient())
            {   
                http.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("Basic", mailchimpapikey-us1);
                content = await http.GetStringAsync(@"https://us1.api.mailchimp.com/3.0/lists");
                Console.WriteLine(content);
            }
        }
    }
}
