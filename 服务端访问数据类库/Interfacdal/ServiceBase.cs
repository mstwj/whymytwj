using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using 服务端访问数据类库.Interface;

namespace 服务端访问数据类库.Interfacdal
{
    public class ServiceBase :IServiceBase
    {
        protected EFCoreContext Context { get; private set; }

        public ServiceBase()
        {
            //这里我是自己NEW的，不需要系统给我NEW了..
            Context = new EFCoreContext();
        }
        public void Commit()
        {
            Context.SaveChanges();
        }

        public void Delete<T>(int Id) where T : class
        {
            T t = Find<T>(Id);//也可以附加
            if (t == null) throw new Exception("t is null");
            Context.Set<T>().Remove(t);
            Commit();
        }

        public void Delete<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");
            Context.Set<T>().Attach(t);
            Context.Set<T>().Remove(t);
            Commit();
        }

        public void Delete<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                Context.Set<T>().Attach(t);
            }
            Context.Set<T>().RemoveRange(tList);
            Commit();
        }

        public T Find<T>(int id) where T : class
        {
            return Context.Set<T>().Find(id);
        }

        public T Insert<T>(T t) where T : class
        {
            Context.Set<T>().Add(t);
            Commit();
            return t;
        }

        public IEnumerable<T> Insert<T>(IEnumerable<T> tList) where T : class
        {
            Context.Set<T>().AddRange(tList);
            Commit();//写在这里  就不需要单独commit  不写就需要 
            return tList;
        }

        public IQueryable<T> Query<T>(Expression<Func<T, bool>> funcWhere) where T : class
        {
            return Context.Set<T>().Where(funcWhere);
        }

        public void Update<T>(T t) where T : class
        {
            if (t == null) throw new Exception("t is null");

            Context.Set<T>().Attach(t);//将数据附加到上下文，支持实体修改和新实体，重置为UnChanged
            Context.Entry(t).State = EntityState.Modified;
            Commit();
        }

        public void Update<T>(IEnumerable<T> tList) where T : class
        {
            foreach (var t in tList)
            {
                Context.Set<T>().Attach(t);
                Context.Entry(t).State = EntityState.Modified;
            }
            Commit();
        }
        public virtual void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }
    }

}
