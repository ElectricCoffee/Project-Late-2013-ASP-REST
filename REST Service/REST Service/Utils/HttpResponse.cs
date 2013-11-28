using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace REST_Service.Utils
{
    public static class HttpResponse
    {
        public static void OK(this HttpResponseMessage message, string content)
        {
            message.StatusCode = HttpStatusCode.OK;
            message.Content = new StringContent(content);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        public static void Forbidden(this HttpResponseMessage response, string reason) {
            response.StatusCode = HttpStatusCode.Forbidden;
            response.Content = new StringContent("");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.ReasonPhrase = reason;
        }
    }
}