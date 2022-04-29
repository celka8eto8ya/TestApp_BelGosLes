using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TestEx_BelGosLes.Interfaces;
using TestEx_BelGosLes.Data;

namespace TestEx_BelGosLes.Services
{

    public class SQLRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context;
        private readonly DbSet<T> _dbSet;

        public SQLRepository(ApplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public IEnumerable<T> GetList()
        {
            return _dbSet.AsNoTracking().ToList();
        }

        public T GetById(int id)
        {
            return _dbSet.Find(id);
        }

        public void Create(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }

        public T CreateEntity(T item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
            return item;
        }

        public void Update(T item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            T item = _dbSet.Find(id);
            if (item != null)
            {
                _dbSet.Remove(item);
                _context.SaveChanges();
            }
        }

    }
}

