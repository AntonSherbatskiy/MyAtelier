using Application.Models;
using Application.Usecases.Commands.Services.Repairing;
using Application.Usecases.Commands.Services.Sewing;
using ErrorOr;
using MyAtelier.DAL.Entities;

namespace Application.Services.Interfaces;

public interface IFavorService
{
    Task<ErrorOr<RepairingServiceModel>> AddRepairingServiceAsync(AddRepairingServiceCommand command);
    Task<ErrorOr<SewingServiceModel>> AddSewingServiceAsync(AddSewingServiceCommand command);
    Task<IEnumerable<RepairingServiceModel>> GetAllRepairingServicesAsync();
    Task<IEnumerable<SewingServiceModel>> GetAllSewingServicesAsync();
    Task<ErrorOr<IEnumerable<RepairingServiceModel>>> RemoveRepairingServiceAsync(RemoveRepairingServiceCommand command);
    Task<ErrorOr<IEnumerable<SewingServiceModel>>> RemoveSewingServiceAsync(RemoveSewingServiceCommand command);
    Task<ErrorOr<RepairingServiceModel>> GetRepairingServiceByIdAsync(int id);
    Task<ErrorOr<RepairingServiceModel>> GetRepairingServiceByServiceAggregatorIdAsync(int serviceAggregatorId);
    Task<ErrorOr<SewingServiceModel>> GetSewingServiceByIdAsync(int id);
    Task<ErrorOr<SewingServiceModel>> GetSewingServiceByServiceAggregatorIdAsync(int orderServiceAggregatorId);
    Task<IEnumerable<SewingServicesGroupedModel>> GetSewingServiceGroupsAsync();
    Task<IEnumerable<SewingServiceModel>> GetSewingServicesInGroupAsync(string clothingName);
}