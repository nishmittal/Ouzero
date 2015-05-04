using System;
using System.Collections.Generic;

namespace SentimentAnalysis
{
    class Program
    {
        private IList<TwitterHandle> _handles;
        private string _accessToken = "3226815046-jOHYCioNDa0m3oLoukS35xY6DLsWQHbQRETZoPq";
        private string _accessTokenSecret = "XKUWVQgKEe2wD0wmLeCF5senwKqPAfcQQZEX5XYtNLQRs";
        private string _consumerKey = "nysITef21Ph7H5gb3mQaCBYXL";
        private string _consumerSecret = "x1ZAAXTxFeZcNN4yxQOC3sIESRTTtsJpwxKgsiBIcUnaGqH6Ap";

        static void Main()
        {
            var prg = new Program();
            prg.Go();
            Console.ReadLine();
        }

        public Program()
        {
            _handles = new List<TwitterHandle>();
            _handles.Add(new TwitterHandle("@NetshockTech"));
            _handles.Add(new TwitterHandle("@techcrunch"));
            //TwitterCredentials.SetCredentials(AccessToken, AccessTokenSecret, ConsumerKey, ConsumerSecret);
        }

        private void Go()
        {
            TwitterDataSourcer tds = new TwitterDataSourcer( _handles );
            
        }
    }
}
