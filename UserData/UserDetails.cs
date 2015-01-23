using System.Collections.Generic;

namespace UserData
{
    public class UserDetails
    {
        private readonly MostRecentList<string> sageDataLocations = new MostRecentList<string>(20);        


        public IEnumerable<string> Sage50DataLocations
        {
            get { return sageDataLocations.GetMostRecentValues(); }
        }

        public void AddSage50DataLocation(string location)
        {
            sageDataLocations.AddUsage(location);
        }
    }
}
