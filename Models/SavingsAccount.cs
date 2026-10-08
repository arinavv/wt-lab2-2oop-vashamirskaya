using System;

namespace wt_lab2_2oop_vashamirskaya.Models
{
    public class SavingsAccount : Account
    {
        public double InterestRate { get; set; }

        public SavingsAccount(int id, string accountNumber, string ownerName,
                              decimal initialBalance, string currency, double interestRate)
            : base(id, accountNumber, ownerName, initialBalance, currency)
        {
            InterestRate = interestRate;
            AccountType = "Сберегательный";
        }

        public void AccrueInterest()
        {
            decimal interest = Balance * (decimal)(InterestRate / 100);
            Deposit(interest);
            Console.WriteLine($"Начислены проценты ({InterestRate}%): {interest:N2} {Currency}");
        }

        public override void Save()
        {
            Console.WriteLine($"Сберегательный счёт {AccountNumber} сохранён.");
        }
    }
}