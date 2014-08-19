using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Audition
{
    public class SaveSearchRequest
    {
        public SearchWindow SearchWindow { get; private set; }
        public string Filename { get; private set; }

        public SaveSearchRequest(SearchWindow searchWindow, string filename)
        {
            SearchWindow = searchWindow;
            Filename = filename;
        }
    }
}
