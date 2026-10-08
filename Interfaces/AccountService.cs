using System;
using System.Collections.Generic;
using System.Linq;
using wt_lab2_2oop_vashamirskaya.Interfaces;
using wt_lab2_2oop_vashamirskaya.Models;

namespace wt_lab2_2oop_vashamirskaya.Services
{
    public class AccountService : IItemService<Account>
    {
        private readonly List<Account> _accounts = new();

        public int Count => _accounts.Count;

        // Перегрузка №1 — принимает готовый объект
        public void Add(Account item)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            _accounts.Add(item);
            Console.WriteLine($"Добавлен счёт {item.AccountNumber} (ID={item.Id}).");
        }

        // Перегрузка №2 — создаёт объект сама
        public void Add(string accountNumber, string owner, decimal balance, string currency)
        {
            int newId = _accounts.Any() ? _accounts.Max(a => a.Id) + 1 : 1;
            var account = new Account(newId, accountNumber, owner, balance, currency);
            Add(account);
        }

        public Account GetById(int id)
        {
            return _accounts.FirstOrDefault(a => a.Id == id);
        }

        public IEnumerable<Account> GetAll()
        {
            return _accounts;
        }

        // Демонстрация protected-доступа: меняем баланс через внутренний механизм
        public void IncreaseAllBalances(decimal percent)
        {
            foreach (var acc in _accounts)
            {
                decimal bonus = acc.Balance * (percent / 100);
                acc.Deposit(bonus);
            }
        }
    }
}