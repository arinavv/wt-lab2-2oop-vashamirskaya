using System;
using wt_lab2_2oop_vashamirskaya.Interfaces;

namespace wt_lab2_2oop_vashamirskaya.Models
{
    public abstract class BaseEntity : IStorable
    {
        public int Id { get; protected set; }
        public DateTime CreatedAt { get; protected set; }

        protected BaseEntity(int id)
        {
            Id = id;
            CreatedAt = DateTime.Now;
        }

        public abstract void Save();

        public virtual void PrintInfo()
        {
            Console.WriteLine($"ID: {Id}, Создано: {CreatedAt:dd.MM.yyyy HH:mm:ss}");
        }
    }
}