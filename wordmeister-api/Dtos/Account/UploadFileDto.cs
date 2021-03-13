using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static wordmeister_api.Helpers.Enums;

namespace wordmeister_api.Dtos.Account
{
    public class UploadFileDto
    {
       public class Request
        {
            public IFormFile File { get; set; }
            public UploadFileType Type { get; set; }
            public string Description { get; set; }
        }
    }
}
