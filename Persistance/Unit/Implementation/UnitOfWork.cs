using MyAtelier.DAL.Context;
using MyAtelier.DAL.Repository;
using MyAtelier.DAL.Repository.Implementation;
using MyAtelier.DAL.Repository.Interfaces;
using MyAtelier.DAL.Unit.Interfaces;

namespace MyAtelier.DAL.Unit.Implementation;

public class UnitOfWork : IUnitOfWork
{
    private AppDbContext _context { get; set; }

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
        ClothingRepository = new ClothingRepository(context);
        MaterialRepository = new MaterialRepository(context);
        RepairingServiceRepository = new RepairingServiceRepository(context);
        ServiceAggregatorRepository = new ServiceAggregatorRepository(context);
        SewingServiceRepository = new SewingServiceRepository(context);
        OrderRepository = new OrderRepository(context);
        UserCodeRepository = new UserCodeRepository(context);
    }
    
    public IClothingRepository ClothingRepository { get; set; }
    public IMaterialRepository MaterialRepository { get; set; }
    public IRepairingServiceRepository RepairingServiceRepository { get; set; }
    public IServiceAggregatorRepository ServiceAggregatorRepository { get; set; }
    public ISewingServiceRepository SewingServiceRepository { get; set; }
    public IOrderRepository OrderRepository { get; set; }
    public IUserCodeRepository UserCodeRepository { get; set; }

    public int Complete()
    {
        return _context.SaveChanges();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}