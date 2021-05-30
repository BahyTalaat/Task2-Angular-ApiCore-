using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Repositories;

namespace BL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        int Commit();

        AccountRepositry Account { get; }

    }
}
