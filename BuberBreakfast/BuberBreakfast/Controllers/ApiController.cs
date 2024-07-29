using System.Security.Cryptography;
using ErrorOr;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

[ApiController]
[Route("breakfasts")]
public class  ApiController: ControllerBase
{
    protected IActionResult Problem(List<Error> errors)
    {
        if(errors.All(e=>e.Type == ErrorType.Validation))
        {
            var modelStateDictinary=new ModelStateDictionary();
    foreach(var error in errors)
    {
        modelStateDictinary.AddModelError(error.Code,error.Description);
    }

            return ValidationProblem(modelStateDictinary);
        }

        if(errors.Any(e=>e.Type==ErrorType.Unexpected))
        {
            return Problem();
        }

        var FirstError=errors[0];
        var statusCode = FirstError.Type switch
        {
            ErrorType.NotFound=> StatusCodes.Status404NotFound,
            ErrorType.Validation=> StatusCodes.Status400BadRequest,
            ErrorType.Conflict=> StatusCodes.Status409Conflict,
            _ =>StatusCodes.Status500InternalServerError

         };
         return Problem(statusCode:statusCode, title:FirstError.Description);
       
    }
}