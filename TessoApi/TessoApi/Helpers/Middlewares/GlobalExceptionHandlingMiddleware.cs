using System.Net;
using TessoApi.Exceptions;
using TessoApi.Models.Http;

namespace TessoApi.Helpers.Middlewares
{
    public class GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger = logger;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (CollectionNotFoundException exp) 
            {
                await HandleExceptionAsync(context, exp, HttpStatusCode.NotFound);
            }
            catch (DbContextException exp) 
            {
                await HandleExceptionAsync(context, exp);
            }
            catch (ProjectAlreadyExistException exp) 
            {
                await HandleExceptionAsync(context, exp, HttpStatusCode.BadRequest); 
            }
            catch (RegistrationFailedException exp) 
            {
                await HandleExceptionAsync(context, exp, HttpStatusCode.InternalServerError);
            }
            catch (UserAlreadyExistsException exp) 
            {
                await HandleExceptionAsync(context, exp, HttpStatusCode.Conflict);
            }
            catch (UserNotFoundException exp) 
            {
                await HandleExceptionAsync(context, exp);
            }
            catch (JwtTokenExtractionException exp) 
            {
                await HandleExceptionAsync(context, exp);
            }
            catch (JwtTokenGenerationException jtge)
            {
                await HandleExceptionAsync(context, jtge);
            }
            catch (ArgumentNullException ane)
            {
                await HandleExceptionAsync(context, ane, HttpStatusCode.BadRequest);
            }
            catch (UnauthorizedAccessException uae)
            {
                await HandleExceptionAsync(context, uae, HttpStatusCode.Unauthorized);
            }
            catch (NotImplementedException nie)
            {
                await HandleExceptionAsync(context, nie, HttpStatusCode.NotImplemented);
            }
            catch (KeyNotFoundException kne)
            {
                await HandleExceptionAsync(context, kne, HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        public Task HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            CustomHttpResponse<string> response = new CustomHttpResponse<string>
            {
                StatusCode = statusCode,
                Data = null,
                Error = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
