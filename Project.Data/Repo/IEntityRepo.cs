using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Project.Data.Repo
{
    public interface IEntityRepo<T>
    {
        List<T> GetAll(params Expression<Func<T, object>>[] includes);
        T GetById(int id,string nav=null);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        List<T> FindAll(Func<T, bool> predicate, params string[] includes);
        T Find(Func<T, bool> predicate, string nav = null);
    }
}
