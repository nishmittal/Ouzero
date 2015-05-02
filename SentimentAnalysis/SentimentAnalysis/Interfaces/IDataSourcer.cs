using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SentimentAnalysis.Interfaces
{
    /// <summary>
    /// Interface for implementations which will source data from social media sites.
    /// </summary>
    interface IDataSourcer
    {
        void GetData();

        int ComputeScore();
    }
}
