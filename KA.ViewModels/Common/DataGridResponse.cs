using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Common
{
    public class DataGridResponse<T>
    {
        public List<T> Items { get; set; }
        public int TotalItem { get; set; }

    }
}