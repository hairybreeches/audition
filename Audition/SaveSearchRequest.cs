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
        public HoursSearchWindow SearchWindow { get; private set; }
        public string Filename { get; private set; }

        public SaveSearchRequest(HoursSearchWindow searchWindow, string filename)
        {
            SearchWindow = searchWindow;
            Filename = filename;
        }
    }
}
