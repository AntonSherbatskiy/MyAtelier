using System.Collections;
using Application.ErrorModels;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Material;
using ErrorOr;
using MapsterMapper;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Unit.Interfaces;

namespace Application.Services.Implementation;

public class MaterialService : IMaterialService
{
    private IMapper _mapper { get; set; }
    private IUnitOfWork _unit { get; set; }

    public MaterialService(IMapper mapper, IUnitOfWork unit)
    {
        _mapper = mapper;
        _unit = unit;
    }

    public async Task<IEnumerable<MaterialModel>> GetAllMaterialsAsync()
    {
        var materials = await _unit.MaterialRepository.GetAsync();

        return _mapper.Map<IEnumerable<MaterialModel>>(materials);
    }

    public async Task<ErrorOr<MaterialModel>> AddMaterialAsync(AddMaterialCommand addMaterialCommand)
    {
        var existedMaterial = await _unit.MaterialRepository.GetMaterialByNameAsync(addMaterialCommand.MaterialName);

        if (existedMaterial != null)
        {
            return Errors.Material.MaterialAlreadyExists;
        }
        
        var material = _mapper.Map<Material>(addMaterialCommand);
        
        await _unit.MaterialRepository.AddAsync(material);
        _unit.Complete();
        
        return _mapper.Map<MaterialModel>(material);
    }

    public async Task<ErrorOr<MaterialModel>> UpdateMaterialAsync(UpdateMaterialCommand command)
    {
        if (command.Quantity < 0)
        {
            return Errors.Material.IncorrectQuantity;
        }
        
        var material = await _unit.MaterialRepository.GetMaterialByIdAsync(command.Id);

        if (material == null)
        {
            return Errors.Material.IncorrectMaterial;
        }

        material.Quantity = command.Quantity;
        await _unit.MaterialRepository.UpdateAsync(material.Id, material);

        _unit.Complete();
        return _mapper.Map<MaterialModel>(material);
    }

    public async Task<ErrorOr<MaterialModel>> GetMaterialByIdAsync(int id)
    {
        var material = await _unit.MaterialRepository.GetMaterialByIdAsync(id);

        if (material == null)
        {
            return Errors.Material.IncorrectMaterial;
        }

        return _mapper.Map<MaterialModel>(material);
    }

    public async Task<ErrorOr<IEnumerable<MaterialModel>>> RemoveMaterialByIdAsync(int id)
    {
        var material = await _unit.MaterialRepository.GetMaterialByIdAsync(id);
        var services = await _unit.SewingServiceRepository.GetMultipleAsync(s => s.MaterialId == id);

        if (material == null)
        {
            return Errors.Material.IncorrectMaterial;
        }

        if (services != null && services.Any())
        {
            return Errors.Material.CannotRemoveActiveMaterial;
        }

        await _unit.MaterialRepository.RemoveAsync(id);
        _unit.Complete();

        return (dynamic)_mapper.Map<IEnumerable<MaterialModel>>(await _unit.MaterialRepository.GetAsync());
    }
}