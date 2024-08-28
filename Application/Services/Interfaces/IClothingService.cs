using Application.Models;
using Application.Usecases.Commands.Clothing;
using ErrorOr;
using MyAtelier.DAL.Unit.Interfaces;

namespace Application.Services.Interfaces;

public interface IClothingService
{
    Task<IEnumerable<ClothingModel>> GetAllClothingAsync();
    Task<ErrorOr<ClothingModel>> AddClothingAsync(AddClothingCommand addClothingCommand);
    Task<ErrorOr<IEnumerable<ClothingModel>>> RemoveClothingByIdAsync(int id);
}