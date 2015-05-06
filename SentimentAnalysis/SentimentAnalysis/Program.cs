using System;
using System.Collections.Generic;

namespace SentimentAnalysis
{
    class Program
    {
        private IList<TwitterHandle> _handles;

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
        }

        private void Go()
        {
            //TwitterDataSourcer tds = new TwitterDataSourcer( _handles );
            
        }
    }
}
