using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace wordmeister_api.Dtos.General
{
    public class General
    {
        public class ResponseResult
        {
            public bool Error { get; set; }
            public object Data { get; set; }
            public string ErrorMessage { get; set; }
        }
    }

    public enum HttpResponseCode
    {
        /// <summary>
        /// Response to a successful GET, PUT, PATCH or DELETE. Can also be used for a POST that doesn't result in a creation.
        /// </summary>
        OK = 200,
        /// <summary>
        /// This means that client-side input fails validation.
        /// </summary>
        BADREQUEST = 400,
        /// <summary>
        /// This means the user isn’t not authorized to access a resource. It usually returns when the user isn’t authenticated.
        /// </summary>
        UNAUTHORIZED = 401,
        /// <summary>
        /// This means the user is authenticated, but it’s not allowed to access a resource.
        /// </summary>
        FORBIDDEN = 403,
        /// <summary>
        ///  This indicates that a resource is not found
        /// </summary>
        NOTFOUND = 404,
        /// <summary>
        /// This is a generic server error. It probably shouldn’t be thrown explicitly.
        /// </summary>
        INTERNALSERVERERROR = 500,
        /// <summary>
        ///  This indicates an invalid response from an upstream server.
        /// </summary>
        BADGATEWAY = 502,
    }
}
