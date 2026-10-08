using System;

namespace wt_lab2_2oop_vashamirskaya.Interfaces
{
    public interface IStorable
    {
        int Id { get; }
        DateTime CreatedAt { get; }
        void Save();
    }
}