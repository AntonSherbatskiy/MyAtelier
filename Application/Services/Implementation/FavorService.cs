using Application.ErrorModels;
using Application.Models;
using Application.Services.Interfaces;
using Application.Usecases.Commands.Services.Repairing;
using Application.Usecases.Commands.Services.Sewing;
using ErrorOr;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using MyAtelier.DAL.Context;
using MyAtelier.DAL.Entities;
using MyAtelier.DAL.Repository.Implementation;
using MyAtelier.DAL.Repository.Interfaces;
using MyAtelier.DAL.Unit.Interfaces;

namespace Application.Services.Implementation;

public class FavorService : IFavorService
{
    private IMapper _mapper { get; set; }
    private IUnitOfWork _unit { get; set; }
    private AppDbContext _context { get; set; }
    
    public FavorService(IMapper mapper, IUnitOfWork unit, AppDbContext context)
    {
        _mapper = mapper;
        _unit = unit;
        _context = context;
    }

    public async Task<ErrorOr<RepairingServiceModel>> AddRepairingServiceAsync(AddRepairingServiceCommand command)
    {
        var existedService =
            await _unit.RepairingServiceRepository.GetRepairingServiceByClothingNameAsync(command.ClothingName);

        if (existedService != null)
        {
            return Errors.Service.ServiceAlreadyExists;
        }

        var clothing = await _unit.ClothingRepository.GetClothingByNameAsync(command.ClothingName);

        if (clothing == null)
        {
            return Errors.Clothing.IncorrectClothing;
        }

        if (command.EstimatedDays < 0)
        {
            return Errors.Service.IncorrectEstimatedDays;
        }

        if (command.Price <= 0)
        {
            return Errors.Service.IncorrectPrice;
        }

        var repairingService = _mapper.Map<RepairingService>((clothing.Id, command));

        await AggregateService(_unit.RepairingServiceRepository, repairingService, "Repairing");

        return _mapper.Map<RepairingServiceModel>(repairingService);
    }

    public async Task<ErrorOr<SewingServiceModel>> AddSewingServiceAsync(AddSewingServiceCommand command)
    {
        var clothing = await _unit.ClothingRepository.GetClothingByNameAsync(command.ClothingName);

        if (clothing == null)
        {
            return Errors.Clothing.IncorrectClothing;
        }

        var material = await _unit.MaterialRepository.GetMaterialByNameAsync(command.MaterialName);

        if (material == null)
        {
            return Errors.Material.IncorrectMaterial;
        }

        var existedSewingService = await _unit.SewingServiceRepository.GetAsync(
            s => s.ClothingSize == command.ClothingSize &&
                 s.Clothing.Name == command.ClothingName &&
                 s.Material.Name == command.MaterialName);

        if (existedSewingService != null)
        {
            return Errors.Service.ServiceAlreadyExists;
        }

        var sewingService = _mapper.Map<SewingService>((clothing.Id, material.Id, command));

        await AggregateService(_unit.SewingServiceRepository, sewingService, "Sewing");

        return _mapper.Map<SewingServiceModel>(sewingService);
    }

    public async Task<IEnumerable<RepairingServiceModel>> GetAllRepairingServicesAsync()
    {
        var services = await 
                _unit.RepairingServiceRepository.GetIncludedAllAsync();

        return _mapper.Map<IEnumerable<RepairingServiceModel>>(services);
    }

    public async Task<IEnumerable<SewingServiceModel>> GetAllSewingServicesAsync()
    {
        var services = await _unit.SewingServiceRepository.GetIncludedAllAsync();
        return _mapper.Map<IEnumerable<SewingServiceModel>>(services);
    }

    public async Task<ErrorOr<IEnumerable<RepairingServiceModel>>> RemoveRepairingServiceAsync(RemoveRepairingServiceCommand command)
    {
        var service = await _unit.RepairingServiceRepository.GetRepairingServiceByIdAsync(command.Id);

        if (service == null)
        {
            return Errors.Service.IncorrectService;
        }

        var orders = await _unit.OrderRepository.GetOrdersByServiceIdAsync(command.Id, "Repairing");

        if (orders.Any(o => o.Status == "Process"))
        {
            return Errors.Service.CannotRemoveActiveService;
        }

        await _unit.RepairingServiceRepository.RemoveAsync(service.Id);
        _unit.Complete();

        var repairingServices = await _unit.RepairingServiceRepository.GetIncludedAllAsync();

        return (dynamic)_mapper.Map<IEnumerable<RepairingServiceModel>>(repairingServices);
    }

    public async Task<ErrorOr<IEnumerable<SewingServiceModel>>> RemoveSewingServiceAsync(RemoveSewingServiceCommand command)
    {
        var service = await _unit.SewingServiceRepository.GetSewingServiceByIdAsync(command.Id);

        if (service == null)
        {
            return Errors.Service.IncorrectService;
        }

        var orders = await _unit.OrderRepository.GetOrdersByServiceIdAsync(command.Id, "Sewing");

        if (orders.Any(o => o.Status == "Process"))
        {
            return Errors.Service.CannotRemoveActiveService;
        }

        await _unit.SewingServiceRepository.RemoveAsync(service.Id);
        _unit.Complete();

        return (dynamic)_mapper.Map<IEnumerable<SewingServiceModel>>(await _unit.SewingServiceRepository.GetIncludedAllAsync());
    }

    public async Task<ErrorOr<RepairingServiceModel>> GetRepairingServiceByIdAsync(int id)
    {
        var repairingService = await _unit.RepairingServiceRepository.GetRepairingServiceByIdAsync(id);

        if (repairingService == null)
        {
            return Errors.Service.IncorrectService;
        }
        
        return _mapper.Map<RepairingServiceModel>(repairingService);
    }
    
    public async Task<ErrorOr<SewingServiceModel>> GetSewingServiceByIdAsync(int id)
    {
        var sewingService = await _unit.SewingServiceRepository.GetSewingServiceByIdAsync(id);

        if (sewingService == null)
        {
            return Errors.Service.IncorrectService;
        }

        return _mapper.Map<SewingServiceModel>(sewingService);
    }

    public async Task<ErrorOr<SewingServiceModel>> GetSewingServiceByServiceAggregatorIdAsync(int serviceAggregatorId)
    {
        var sewingService = await _unit.ServiceAggregatorRepository.GetSewingServiceByIdAsync(serviceAggregatorId);
        
        if (sewingService == null)
        {
            return Errors.Service.IncorrectService;
        }

        return _mapper.Map<SewingServiceModel>(sewingService);
    }

    public async Task<IEnumerable<SewingServicesGroupedModel>> GetSewingServiceGroupsAsync()
    {
        var services = await _unit.SewingServiceRepository.GetIncludedAllAsync();
        var groups = services.GroupBy(s => s.Clothing.Name).Select(g =>
            new SewingServicesGroupedModel()
            {
                ClothingName = g.Key,
                SewingServiceModels = g.Select(s => _mapper.Map<SewingServiceModel>(s)).ToList()
            });

        return groups;
    }

    public async Task<IEnumerable<SewingServiceModel>> GetSewingServicesInGroupAsync(string clothingName)
    {
        var services = await _unit.SewingServiceRepository.GetIncludedAllAsync();
        return _mapper.Map<IEnumerable<SewingServiceModel>>(services.Where(s => s.Clothing.Name == clothingName));
    }

    public async Task<ErrorOr<RepairingServiceModel>> GetRepairingServiceByServiceAggregatorIdAsync(int serviceAggregatorId)
    {
        var repairingService = await _unit.ServiceAggregatorRepository.GetRepairingServiceByIdAsync(serviceAggregatorId);

        if (repairingService == null)
        {
            return Errors.Service.IncorrectService;
        }

        return _mapper.Map<RepairingServiceModel>(repairingService);
    }

    private async Task AggregateService<T>(IRepository<int, T> repository, T service, string serviceType) 
        where T : Identity
    {
        await repository.AddAsync(service);
        _unit.Complete();

        var aggregator = new ServiceAggregator()
        {
            ServiceType = serviceType
        };
        
        if (service is SewingService)
        {
            aggregator.SewingServiceId = service.Id;
        }
        else
        {
            aggregator.RepairingServiceId = service.Id;
        }
        
        await _unit.ServiceAggregatorRepository.AddAsync(aggregator);
        
        _unit.Complete();
    }
}