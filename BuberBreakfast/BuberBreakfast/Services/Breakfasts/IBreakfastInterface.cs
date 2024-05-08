using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using ErrorOr;

namespace BuberBreakfast.Services.Breakfasts;

public interface IBreakfastService
{
    void CreateBreakfast(Breakfast breakfast);
    void DeleteBreakfast(Guid id);
    ErrorOr<Breakfast> GetBreakfast(Guid id);
    void UpdateBreakfast(Breakfast breakfast);
}