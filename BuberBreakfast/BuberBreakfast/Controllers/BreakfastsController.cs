using BuberBreakFast.Contracts.Breakfast;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Models;


public class BreakfastController: ApiController
{
    private readonly IBreakfastService _breakfastService;

    public BreakfastController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost()]
public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.Create (
        request.Name,
        request.Description,
        request.StartDateTime,
        request.EndDateTime,
        request.Savory,
        request.Sweet);

        if(requestToBreakfastResult.IsError)
        {
          return Problem(requestToBreakfastResult.Errors);
        }

        var breakfast=requestToBreakfastResult.Value;
        ErrorOr<Created> CreateBreakfastResult = _breakfastService.CreateBreakfast(breakfast);
       
        return CreateBreakfastResult.Match(
        created=>CreatedAtGetBreakfast(breakfast),
        errors=>Problem(errors)
       );
    }

   

    [HttpGet("{id:guid}")]
public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
        return getBreakfastResult.Match(
        breakfast=>Ok(MapBreakfastResponse(breakfast)),
        errors=>Problem(errors));
    }


    

    [HttpPut("{id:guid}")]
public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
{
    ErrorOr<Breakfast> requestToBreakfastResult = Breakfast.From(id,request);

    if(requestToBreakfastResult.IsError)
    {
      return Problem(requestToBreakfastResult.Errors);
    }
    var breakfast=requestToBreakfastResult.Value;
  ErrorOr<UpsertBreakfast> upsertedBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);
   

    return upsertedBreakfastResult.Match(
      upserted=> upserted.isNewlyCreated ? CreatedAtGetBreakfast(breakfast):NoContent(),
      errors=>Problem(errors));
}

[HttpDelete("{id:guid}")]
public IActionResult DeleteBreakfast(Guid id)
{
    ErrorOr<Deleted> deletedBreakfastResult = _breakfastService.DeleteBreakfast(id);
    return deletedBreakfastResult.Match(
      deleted => NoContent(),
      errors =>Problem(errors)
    );
}

private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
        breakfast.Id,
        breakfast.Name,
        breakfast.Description,
        breakfast.StartDateTime,
        breakfast.EndDateTime,
        breakfast.LastModifiedDateTime,
        breakfast.Savory,
        breakfast.Sweet);
    }
 private IActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
          actionName: nameof(GetBreakfast),
          routeValues: new { id = breakfast.Id },
          value: MapBreakfastResponse(breakfast));
    }

}