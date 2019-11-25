using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Runpath.Gallery.Domain.Exceptions;
using System;
using System.Net;
using System.Threading.Tasks;
using NonActionAttribute = System.Web.Http.NonActionAttribute;

namespace Runpath.Gallery.WebApi.Controllers
{
    public class AppBaseController : Controller
    {
        protected readonly ILogger Logger;

        protected AppBaseController(ILogger logger)
        {
            Logger = logger;
        }

        protected ObjectResult InternalServerError(string message)
        {
            return InternalServerError(StringToResult(message));
        }

        protected ObjectResult InternalServerError(object message)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, message);
        }

        [NonAction]
        public new ActionResult Ok()
        {
            return base.Ok();
        }

        [NonAction]
        public ActionResult<T> Ok<T>(T value)
        {
            return base.Ok(value);
        }

        private static object StringToResult(string message)
        {
            return new { Message = message };
        }

        protected async Task<ActionResult> Execute(string messageOnException, Func<ActionResult> action)
        {
            return await Execute(messageOnException, () => Task.FromResult(action()));
        }

        protected async Task<ActionResult> Execute(string messageOnException, Func<Task<ActionResult>> action)
        {
            return await Execute(ex => messageOnException, action);
        }

        protected async Task<ActionResult<T>> Execute<T>(string messageOnException, Func<ActionResult<T>> action)
        {
            return await Execute(messageOnException, () => Task.FromResult(action()));
        }

        protected async Task<ActionResult<T>> Execute<T>(string messageOnException, Func<Task<ActionResult<T>>> action)
        {
            return await Execute(ex => messageOnException, action);
        }

        protected async Task<ActionResult> Execute(Func<Exception, string> messageOnExceptionFunc, Func<Task<ActionResult>> action)
        {
            try
            {
                return await action();
            }
            catch (DataValidationException ex)
            {
                return BadRequest(new { validationErrors = ex.Message });
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                var message = messageOnExceptionFunc(ex);
                return InternalServerError(message);
            }
        }

        protected async Task<ActionResult<T>> Execute<T>(Func<Exception, string> messageOnExceptionFunc, Func<Task<ActionResult<T>>> action)
        {
            try
            {
                return await action();
            }
            catch (DataValidationException ex)
            {
                return BadRequest(new { validationErrors = ex.Message });
            }
            catch (ResourceNotFoundException)
            {
                return NotFound();
            }
            catch (UnauthorizedException)
            {
                return Unauthorized();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                var message = messageOnExceptionFunc(ex);
                return InternalServerError(message);
            }
        }
    }
}