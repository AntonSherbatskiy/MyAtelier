using Application.Models;
using Application.Usecases.Commands.Material;
using ErrorOr;

namespace Application.Services.Interfaces;

public interface IMaterialService
{
    Task<IEnumerable<MaterialModel>> GetAllMaterialsAsync();
    Task<ErrorOr<MaterialModel>> AddMaterialAsync(AddMaterialCommand addMaterialCommand);
    Task<ErrorOr<MaterialModel>> UpdateMaterialAsync(UpdateMaterialCommand command);
    Task<ErrorOr<MaterialModel>> GetMaterialByIdAsync(int id);
    Task<ErrorOr<IEnumerable<MaterialModel>>> RemoveMaterialByIdAsync(int id);
}