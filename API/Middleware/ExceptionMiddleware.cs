using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        // RequestDelegate is what is coming up next in the pipeline
        // ILogger so we can logout our exception into our terminal, we dont want to swallow we want to display it into our terminal
        // IHostEnvironment to check what environment we are running, is it PROD or DEV
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        // this is happening in the context of HttpContext, when we add middleware we have access to Http request that is coming in
        public async Task InvokeAsync(HttpContext context)
        {
            // this is a place we use try and catch block
            try
            {
                // we try next(context)
                // take the context and pass to the next piece of middleware
                // this piece of middle ware will stay at the very top of our middleware and anything below this and any of them gets an exception
                // then it trhows the exception up and up up until it handles the exception and then because our exception middleware is at the very top
                // of that tree we are going to catch the exception
                await _next(context);
            }
            catch(Exception ex)
            {
                // first of we are going to log the error if we don't then our applicaiton will be silcence of what's happened
                _logger.LogError(ex, ex.Message);
                // we are going to write our excpetion into response
                context.Response.ContentType = "application/json";
                // specify the status code
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // create a response
                var response = _env.IsDevelopment() // check what environment we are running in
                    // create a new API exception (our own API exception)

                    //just incase if it is null then we use ?
                    ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
                    // if it is in production mode we don't go for many details
                    : new ApiException(context.Response.StatusCode, "Internal Server Error");

                // we send back this as JSON
                // by defaul we want our JSON file to be back in cama case
                // create some options
                // we need to serialze our response into a JSON response
                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var json = JsonSerializer.Serialize(response, options);
                await context.Response.WriteAsync(json);
            }
        }
    }
}
