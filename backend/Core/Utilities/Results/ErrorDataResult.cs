using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data, string message) : base(data, false, message)
        {
        }

        public ErrorDataResult(T data) : base(data, false)
        {
        }

        public ErrorDataResult(string message) : base(default, false, message)
        {
        }

        public ErrorDataResult() : base(default, false)
        {
        }

        public ErrorDataResult(string message, string errorCode)
            : base(default, false, message, errorCode)
        {
        }


        public ErrorDataResult(T data, string message, string errorCode)
            : base(data, false, message, errorCode)
        {
        }


        public ErrorDataResult(T data, string errorCode, bool isErrorCodeOnly=true)
            : base(data, false, errorCode, isErrorCodeOnly)
        {
        }

    }
}
