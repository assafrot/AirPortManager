using Microsoft.EntityFrameworkCore;
using DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(DbContext context)
        {
            _context = context;
        }
        protected readonly DbContext _context;


        public void Add(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public T Get(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public void Remove(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
