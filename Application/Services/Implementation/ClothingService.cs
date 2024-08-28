using Application.ErrorModels;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Clothing;
using ErrorOr;
using MapsterMapper;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Unit.Interfaces;

namespace Application.Services.Implementation;

public class ClothingService : IClothingService
{
    private IUnitOfWork _unit { get; set; }
    private IMapper _mapper { get; set; }

    public ClothingService(IUnitOfWork unit, IMapper mapper)
    {
        _unit = unit;
        _mapper = mapper;
    }
    
    public async Task<IEnumerable<ClothingModel>> GetAllClothingAsync()
    {
        var clothing = await _unit.ClothingRepository.GetAsync();
        var mapped = _mapper.Map<IEnumerable<ClothingModel>>(clothing);

        return mapped;
    }

    public async Task<ErrorOr<ClothingModel>> AddClothingAsync(AddClothingCommand addClothingCommand)
    {
        var existedClothing = await _unit.ClothingRepository.GetClothingByNameAsync(addClothingCommand.Name);

        if (existedClothing != null)
        {
            return Errors.Clothing.ClothingAlreadyExists;
        }

        var clothing = _mapper.Map<Clothing>(addClothingCommand);
        
        await _unit.ClothingRepository.AddAsync(clothing);
        _unit.Complete();
        
        return _mapper.Map<ClothingModel>(clothing);
    }

    public async Task<ErrorOr<IEnumerable<ClothingModel>>> RemoveClothingByIdAsync(int id)
    {
        var clothing = await _unit.ClothingRepository.GetClothingByIdAsync(id);

        if (clothing == null)
        {
            return Errors.Clothing.IncorrectClothing;
        }

        if (clothing.RepairingServices.Count > 0 || clothing.SewingServices.Count > 0)
        {
            return Errors.Clothing.CannotRemoveActiveClothing;
        }

        await _unit.ClothingRepository.RemoveAsync(id);
        _unit.Complete();

        return (dynamic)_mapper.Map<IEnumerable<ClothingModel>>(await _unit.ClothingRepository.GetAsync());
    }
}