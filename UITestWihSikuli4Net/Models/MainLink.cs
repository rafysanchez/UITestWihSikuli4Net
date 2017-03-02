using System.Collections.Generic;

namespace UITestWihSikuli4Net.Models
{
    public class MainLink
    {
        public string Caption { get; set; }

        public string Url { get; set; }

        public List<MainLink> ChildLinks { get; set; }
    }
}
