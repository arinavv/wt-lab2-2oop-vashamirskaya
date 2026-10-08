using System.Collections.Generic;
using wt_lab2_2oop_vashamirskaya.Models;

namespace wt_lab2_2oop_vashamirskaya.Interfaces
{
    public interface IItemService<T> where T : BaseEntity
    {
        void Add(T item);
        T GetById(int id);
        IEnumerable<T> GetAll();
        int Count { get; }
    }
}