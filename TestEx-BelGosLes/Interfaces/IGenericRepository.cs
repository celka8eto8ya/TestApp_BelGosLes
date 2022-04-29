using System.Collections.Generic;

namespace TestEx_BelGosLes.Interfaces
{
    public interface IGenericRepository<T>
        where T : class
    {
        IEnumerable<T> GetList();
        T GetById(int id);
        void Create(T item);
        T CreateEntity(T item);
        void Update(T item);
        void Delete(int id);
    }
}
