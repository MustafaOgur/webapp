using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Results
{
    public class Result : IResult
    {
        public Result(bool success, string message) : this(success)
        {
            Message = message;
        }
        public Result(bool success)
        {
            Success = success;
        }

        public Result(bool success, string message, string errorCode)
            : this(success, message)
        {
            ErrorCode = errorCode;
        }


        public Result(bool success, string errorCode, bool isErrorCodeOnly)
            : this(success)
        {
            ErrorCode = errorCode;
        }

        public bool Success {  get; }

        public string Message { get; }

        public string ErrorCode { get; }
    }
}
