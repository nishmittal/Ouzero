using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;

namespace SentimentAnalysis
{
    class Program
    {
        private IList<TwitterHandle> Handles;
        private string AccessToken = "3226815046-jOHYCioNDa0m3oLoukS35xY6DLsWQHbQRETZoPq";
        private string AccessTokenSecret = "XKUWVQgKEe2wD0wmLeCF5senwKqPAfcQQZEX5XYtNLQRs";
        private string ConsumerKey = "nysITef21Ph7H5gb3mQaCBYXL";
        private string ConsumerSecret = "x1ZAAXTxFeZcNN4yxQOC3sIESRTTtsJpwxKgsiBIcUnaGqH6Ap";

        static void Main(string[] args)
        {
            var prg = new Program();
            prg.Go();
            Console.ReadLine();
        }

        public Program()
        {
            Handles = new List<TwitterHandle>();
            Handles.Add(new TwitterHandle("@NetshockTech"));
            Handles.Add(new TwitterHandle("@techcrunch"));
            TwitterCredentials.SetCredentials(AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret);
        }

        private void Go()
        {
            TwitterDataSourcer tds = new TwitterDataSourcer( Handles );
            
        }
    }
}
