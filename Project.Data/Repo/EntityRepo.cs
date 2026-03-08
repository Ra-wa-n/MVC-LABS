using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.Data.Data;
using Project.Data.Model;

namespace Project.Data.Repo
{
    public class EntityRepo<T> : IEntityRepo<T> where T : class 
    {
        PContext context;
        DbSet<T> dataSet;
        public EntityRepo(PContext context)
        {
            this.context = context;
            dataSet = context.Set<T>();
        }
        public void Add(T entity)
        {
           dataSet.Add(entity);
     
        }

        public void Delete(int id)
        {
            var std = dataSet.Find(id);
            context.Remove(std);
           
        }

        public T Find(Func<T, bool> predicate, string nav = null)
        {
            if (string.IsNullOrEmpty(nav))
            {
                return dataSet.FirstOrDefault(predicate);
            }
            return dataSet.Include(nav).FirstOrDefault(predicate);
        }

        public List<T> FindAll(Func<T, bool> predicate, params string[] includes)
        {
            IQueryable<T> query = dataSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.Where(predicate).ToList();
        }
        public List<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = dataSet;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query.ToList();
        }

        public T GetById(int id, string nav = null)
        {
            if (string.IsNullOrEmpty(nav))
            {
                return dataSet.Find(id);
            }
            return dataSet.Include(nav).FirstOrDefault(x => EF.Property<int>(x, "Id") == id);
        }

        public void Update(T entity)
        {
            context.Update(entity);
          
        }
    }
    
    
}
