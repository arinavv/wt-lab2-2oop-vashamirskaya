using System;

namespace wt_lab2_2oop_vashamirskaya.Models
{
    public class CreditAccount : Account
    {
        public decimal CreditLimit { get; set; }

        public CreditAccount(int id, string accountNumber, string ownerName,
                             decimal initialBalance, string currency, decimal creditLimit)
            : base(id, accountNumber, ownerName, initialBalance, currency)
        {
            CreditLimit = creditLimit;
            AccountType = "Кредитный";
        }

        public void UseCredit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Сумма кредита должна быть положительной.");
            if (Balance - amount < -CreditLimit)
                throw new InvalidOperationException("Превышен кредитный лимит.");

            SetBalance(Balance - amount);
            Console.WriteLine($"Кредит использован на {amount:N2} {Currency}. Баланс: {Balance:N2} {Currency}");
        }

        public override void Save()
        {
            Console.WriteLine($"Кредитный счёт {AccountNumber} сохранён.");
        }
    }
}