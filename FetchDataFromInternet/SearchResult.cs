using System;
using System.Collections.Generic;
using System.Text;

namespace FetchDataFromInternet
{
    internal class SearchResult
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public int total { get; set; }
        public List<int> objectIDs { get; set; }
    }
}
