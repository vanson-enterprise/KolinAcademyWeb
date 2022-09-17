using KA.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA.ViewModels.Common
{
    public  class ResponseDto
    {
        public ResponseStatus Status { get; set; }
        public string? Message { get; set; }
    }
}
