using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace REST_Service.Utils
{
    public static class HttpResponse
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        public static void OK(this HttpResponseMessage message, string content)
        {
            message.StatusCode = HttpStatusCode.OK;
            message.Content = new StringContent(content);
            message.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="response"></param>
        /// <param name="reason"></param>
        public static void Forbidden(this HttpResponseMessage response, string reason) {
            response.StatusCode = HttpStatusCode.Forbidden;
            response.Content = new StringContent("");
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            response.ReasonPhrase = reason;
        }

        /// <summary>
        /// A try-catch that returns either an OK if successful or a Forbidden if failed
        /// </summary>
        /// <typeparam name="E">Any kind of Exception</typeparam>
        /// <param name="action">The code you want to try</param>
        /// <param name="success">A JSON string with the success message</param>
        /// <param name="failure">A string message saying why it failed</param>
        public static HttpResponseMessage Try<E>(
            Action action,
            string success = "{\"Response\":\"Success\"}",
            string failure = "Noget gik galt") 
            where E : Exception
        {
            var message = new HttpResponseMessage();
            try
            {
                action();
                message.OK(success);
            }
            catch (E ex)
            {
                Debug.WriteLine(ex.Message);
                message.Forbidden(failure);
            }

            return message;
        }
    }
}