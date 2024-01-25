using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseProject.Service.Exceptions
{
    public class BookShopException : Exception
    {
        public int Code { get; set; }
        public BookShopException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
