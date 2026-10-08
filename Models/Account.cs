using System;

namespace wt_lab2_2oop_vashamirskaya.Models
{
    public class Account : BaseEntity
    {
        public string AccountNumber { get; set; }
        public string OwnerName { get; set; }
        public string Currency { get; set; }
        public string AccountType { get; set; }

        private decimal _balance;

        public decimal Balance
        {
            get => _balance;
            private set
            {
                if (value < 0)
                    throw new ArgumentException("Баланс не может быть отрицательным.");
                _balance = value;
            }
        }

        public Account(int id, string accountNumber, string ownerName, decimal initialBalance, string currency)
            : base(id)
        {
            AccountNumber = accountNumber;
            OwnerName = ownerName;
            Currency = currency;
            AccountType = "Обычный";
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма пополнения должна быть больше нуля.");

            Balance += amount;
            Console.WriteLine($"[+] Счёт {AccountNumber}: пополнение {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма снятия должна быть больше нуля.");
            if (amount > Balance)
                throw new InvalidOperationException("Недостаточно средств на счёте.");

            Balance -= amount;
            Console.WriteLine($"[-] Счёт {AccountNumber}: снятие {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}");
        }

        protected void SetBalance(decimal newBalance)
        {
            _balance = newBalance;
        }

        public override void Save()
        {
            Console.WriteLine($"Счёт {AccountNumber} сохранён в базе данных.");
        }

        public override void PrintInfo()
        {
            base.PrintInfo();
            Console.WriteLine($"Номер счёта: {AccountNumber}, Владелец: {OwnerName}");
            Console.WriteLine($"Тип: {AccountType}, Баланс: {Balance:N2} {Currency}");
            Console.WriteLine("-----------------------------------------------------");
        }
    }
}