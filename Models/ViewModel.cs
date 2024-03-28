using System.Text.RegularExpressions;
using System;

namespace BeautySaloon.Models
{
    public class ViewModel
    {
        public IEnumerable<Service> Service { get; set; }
        public IEnumerable<Services> Services { get; set; }
    }
}
