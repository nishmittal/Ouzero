using System;
using System.Collections.Generic;
using System.Linq;
using Tweetinvi.Core.Interfaces;
using Tweetinvi.Logic;

namespace SentimentAnalysis
{
    public static class ListsScorer
    {
        public static void Score(List<string> listUrls)
        {
            if(!listUrls.Any())
            {
                throw new ArgumentException("List was empty.");
            }

            // identify list and get users from it
            // score the damn list
        }

        public static IList<TwitterList> TwitterLists { get; set; }
    }
}
