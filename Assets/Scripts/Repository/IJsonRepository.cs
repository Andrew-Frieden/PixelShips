using System.Collections.Generic;
using Models;

namespace Repository
{
    public interface IJsonRepository<T>
    {
        IEnumerable<T> LoadData();
    }
}