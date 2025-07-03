using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using OnlineCourse.Application.Dtos;
using OnlineCourse.Application.Models.Favourite;
using OnlineCourse.Application.RepositoryContracts;
using OnlineCourse.Application.Validators.Review;
using OnlineCourse.Domain.Entities;
using StatusGeneric;

namespace OnlineCourse.Application.ServiceContracts.Implementations;

public class FavouriteService(
    IBaseRepositroy<Favourite> favouriteRepository,
    IBaseRepositroy<User> userRepository,
    IBaseRepositroy<Course> courseRepository,
    IMapper mapper,
    IValidator<AddToFavouriteRequestModel> createValidator,
    IValidator<IsFavouriteRequestModel> isFavouriteValidator) : StatusGenericHandler, IFavouriteService
{
    private readonly IBaseRepositroy<Favourite> _favouriteRepository = favouriteRepository;
    private readonly IBaseRepositroy<User> _userRepository = userRepository;
    private readonly IBaseRepositroy<Course> _courseRepository = courseRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IValidator<AddToFavouriteRequestModel> _addToValidator = createValidator;
    private readonly IValidator<IsFavouriteRequestModel> _isFavouriteValidator = isFavouriteValidator;
    public async Task AddToFavouriteAsync(AddToFavouriteRequestModel model)
    {
        var validatorResult = await _addToValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }

        bool result = await IsFavouriteAsync(model);

        if (result)
        {
            AddError($"Course with id: {model.CourseId} is already added");
            return;
        }

        Favourite? newFavourite = _mapper.Map<Favourite>(model);

        await _favouriteRepository.AddAsync(newFavourite);
        await _favouriteRepository.SaveChangesAsync();
    }

    public async Task<IEnumerable<FavouriteDto>> GetByUserAsync(Guid userId)
    {
        User? user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
        {
            AddError($"User with id: {userId}");
            return Enumerable.Empty<FavouriteDto>();
        }

        var userFavourites = await _favouriteRepository.GetAll()
            .Where(x => x.UserID == user.Id)
            .ToListAsync();

        return _mapper.Map<List<FavouriteDto>>(userFavourites);
    }

    public async Task<bool> IsFavouriteAsync(IsFavouriteRequestModel model)
    {
        var validatorResult = await _isFavouriteValidator.ValidateAsync(model);
        if (!validatorResult.IsValid)
        {
            foreach (var error in validatorResult.Errors)
            {
                AddError($"Validation error: {error.ErrorMessage}");
            }
        }
        User? user = await _userRepository.GetByIdAsync(model.UserId);
        if (user is null)
        {
            AddError($"User with id: {model.UserId}");
            return false;
        }

        Course? course = await _courseRepository.GetByIdAsync(model.CourseId);

        if (course is null)
        {
            AddError($"Course with id: {model.CourseId} is not found");
            return false;
        }

        return await _favouriteRepository.GetAll()
            .AnyAsync(x => x.UserID == user.Id && x.CourseId == course.Id);


    }

    public async Task RemoveFromFavouritesAsync(int id)
    {
        Favourite? favourite = await _favouriteRepository.GetByIdAsync(id);
        if (favourite is null)
        {
            AddError($"Favourite with id: {id} is not found");
            return;
        }

        await _favouriteRepository.DeleteAsync(favourite);
        await _favouriteRepository.SaveChangesAsync();
    }
}
