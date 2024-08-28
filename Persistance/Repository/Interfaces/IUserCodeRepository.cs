using MyAtelier.DAL.Entities;

namespace MyAtelier.DAL.Repository.Interfaces;

public interface IUserCodeRepository : IRepository<int, UserCode>
{
    Task<UserCode?> GetCodeByEmail(string registerCommandEmail);
    void RemoveCodesByEmail(string userEmail);
}