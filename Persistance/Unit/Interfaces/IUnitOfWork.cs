using MyAtelier.DAL.Repository.Interfaces;

namespace MyAtelier.DAL.Unit.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IClothingRepository ClothingRepository { get; set; }
    IMaterialRepository MaterialRepository { get; set; }
    IRepairingServiceRepository RepairingServiceRepository { get; set; }
    IServiceAggregatorRepository ServiceAggregatorRepository { get; set; }
    ISewingServiceRepository SewingServiceRepository { get; set; }
    IOrderRepository OrderRepository { get; set; }
    IUserCodeRepository UserCodeRepository { get; set; }

    int Complete();
}