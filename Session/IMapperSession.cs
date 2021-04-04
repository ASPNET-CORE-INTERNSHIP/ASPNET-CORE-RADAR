using ASPNETAOP.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNETAOP.Session
{
    public interface IMapperSession
    {
        void BeginTransaction();
        Task Commit();
        Task Rollback();
        void CloseTransaction();
        Task Save(Receiver entity);
        Task Delete(Receiver entity);
        IQueryable<Receiver> receivers { get; }
    }

}
