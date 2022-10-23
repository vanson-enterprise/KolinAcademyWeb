using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.DataProvider.Entities
{
    public class OfflineCourseStartDate
    {
        public int Id { get; set; }
        public int OfflineCourseId { get; set; }
        public string Place { get; set; }
        public DateTime StartTime { get; set; }
    }
}