using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Favourite;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts;

public interface IFavouriteService : IStatusGeneric
{
    Task<IEnumerable<FavouriteDto>> GetByUserAsync(Guid userId);
    Task<bool> IsFavouriteAsync(IsFavouriteRequestModel model);
    Task AddToFavouriteAsync(AddToFavouriteRequestModel model);
    Task RemoveFromFavouritesAsync(int id);
}
