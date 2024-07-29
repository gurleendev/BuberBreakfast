
using ErrorOr;
using Models;

public interface IBreakfastService{


    ErrorOr<Created> CreateBreakfast(Breakfast request);
    ErrorOr<Deleted> DeleteBreakfast(Guid id);
    ErrorOr<Breakfast> GetBreakfast(Guid id);
 ErrorOr<UpsertBreakfast> UpsertBreakfast(Breakfast breakfast);
}