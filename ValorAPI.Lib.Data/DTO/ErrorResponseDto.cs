using System;
using System.Collections.Generic;
using System.Text;

namespace ValorAPI.Lib.Data.DTO
{
    public class ErrorResponseDto
    {
        public ErrorStatusInnerDto status;
    }

    public class ErrorStatusInnerDto
    {
        public int status_code;
        public string message;
    }
}
