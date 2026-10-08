using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using wt_lab2_2oop_vashamirskaya.Interfaces;
using wt_lab2_2oop_vashamirskaya.Models;
using wt_lab2_2oop_vashamirskaya.Services;

namespace wt_lab2_2oop_vashamirskaya
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=====================================================");
                Console.WriteLine("  Лабораторная работа №2. Банковская система");
                Console.WriteLine("=====================================================");
                Console.WriteLine("  1  — Вводное задание (меню)");
                Console.WriteLine("  2  — Блок 1, Уровень 1 (класс Account)");
                Console.WriteLine("  3  — Блок 1, Уровень 2 (наследование, полиморфизм)");
                Console.WriteLine("  4  — Блок 1, Уровень 3 (IItemService<T>, перегрузка)");
                Console.WriteLine("  5  — Блок 2, Уровень 1 (List<T>, foreach)");
                Console.WriteLine("  6  — Блок 2, Уровень 2 (Dictionary, TryGetValue)");
                Console.WriteLine("  7  — Блок 2, Уровень 3 (LINQ)");
                Console.WriteLine("  8  — Блок 3, Уровень 1 (async/await)");
                Console.WriteLine("  9  — Блок 3, Уровень 2 (Task<List<T>>, try-catch)");
                Console.WriteLine(" 10  — Блок 3, Уровень 3 (Task.WhenAll, CancellationToken)");
                Console.WriteLine("  0  — Выход");
                Console.WriteLine("=====================================================");
                Console.Write("Ваш выбор: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowMenu(); break;
                    case "2": Block1_Level1(); break;
                    case "3": Block1_Level2(); break;
                    case "4": Block1_Level3(); break;
                    case "5": Block2_Level1(); break;
                    case "6": Block2_Level2(); break;
                    case "7": Block2_Level3(); break;
                    case "8": await Block3_Level1(); break;
                    case "9": await Block3_Level2(); break;
                    case "10": await Block3_Level3(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный выбор."); break;
                }

                Console.WriteLine("\nНажмите Enter, чтобы вернуться в меню...");
                Console.ReadLine();
            }
        }

        // ---------- Вводное задание ----------
        static void ShowMenu()
        {
            Console.WriteLine("Добро пожаловать в систему Банковского учёта!");
            Console.WriteLine("Выберите действие:");
            Console.WriteLine("  1 - Показать все счета");
            Console.WriteLine("  2 - Открыть новый счёт");
            Console.WriteLine("  3 - Пополнить счёт");
            Console.WriteLine("  4 - Снять средства");
            Console.WriteLine("  5 - Найти счёт по номеру");
            Console.WriteLine("  0 - Выход");
        }

        // ---------- Блок 1, Уровень 1 ----------
        static void Block1_Level1()
        {
            Console.WriteLine("=== Блок 1, Уровень 1: класс Account ===\n");

            // Объект через конструктор
            Account account1 = new Account(1, "BY12ALFA...1234", "Арина Вашамирская", 1500.75m, "BYN");

            // Объект через конструктор + инициализатор объектов (изменяем только то,
            // что разрешено менять снаружи — Currency)
            Account account2 = new Account(2, "BY12ALFA...5678", "Иван Петров", 320.50m, "BYN")
            {
                Currency = "USD"
            };

            account1.PrintInfo();
            account2.PrintInfo();

            Console.WriteLine($"Суммарный баланс: {account1.Balance + account2.Balance:N2}");
        }

        // ---------- Блок 1, Уровень 2 ----------
        static void Block1_Level2()
        {
            Console.WriteLine("=== Блок 1, Уровень 2: Наследование и полиморфизм ===\n");

            List<BaseEntity> accounts = new List<BaseEntity>
            {
                new Account(1, "BY12ALFA...1234", "Арина Вашамирская", 1500.75m, "BYN"),
                new SavingsAccount(2, "BY12ALFA...5678", "Иван Петров", 5000m, "BYN", 5.5),
                new CreditAccount(3, "BY12ALFA...9012", "Ольга Сидорова", 200m, "BYN", 1000m)
            };

            foreach (var account in accounts)
            {
                account.PrintInfo();
                account.Save();
            }

            Console.WriteLine("\n=== Операции по счёту ===");
            var acc = (Account)accounts[0];
            acc.Deposit(200);
            try { acc.Withdraw(5000); }
            catch (InvalidOperationException ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
        }

        // ---------- Блок 1, Уровень 3 ----------
        static void Block1_Level3()
        {
            Console.WriteLine("=== Блок 1, Уровень 3: IItemService<T>, перегрузка ===\n");

            var accountService = new AccountService();

            accountService.Add("BY12ALFA...1234", "Арина Вашемирская", 1500.75m, "BYN");
            accountService.Add("BY12ALFA...5678", "Иван Петров", 3200m, "BYN");
            accountService.Add(new SavingsAccount(3, "BY12ALFA...9012", "Ольга Сидорова", 8000m, "BYN", 5.5));

            IItemService<Account> service = accountService;

            Console.WriteLine($"\nВсего счетов в системе: {service.Count}\n");

            foreach (var acc in service.GetAll())
                acc.PrintInfo();

            Console.WriteLine("\n=== Поиск по ID ===");
            var found = service.GetById(2);
            if (found != null) found.PrintInfo();
            else Console.WriteLine("Счёт с таким ID не найден.");

            Console.WriteLine("\n=== Начисление бонусов (protected-доступ через сервис) ===");
            accountService.IncreaseAllBalances(1);
        }

        // ---------- Блок 2, Уровень 1 ----------
        static void Block2_Level1()
        {
            Console.WriteLine("=== Блок 2, Уровень 1: List<T> и foreach ===\n");

            List<Account> accounts = new List<Account>
            {
                new Account(1, "BY12ALFA...0001", "Арина Вашемирская", 1500.75m, "BYN"),
                new Account(2, "BY12ALFA...0002", "Иван Петров", 320.50m, "USD"),
                new SavingsAccount(3, "BY12ALFA...0003", "Ольга Сидорова", 8000m, "BYN", 5.5),
                new SavingsAccount(4, "BY12ALFA...0004", "Дмитрий Кузнецов", 12000m, "BYN", 7.0),
                new CreditAccount(5, "BY12ALFA...0005", "Елена Морозова", 200m, "BYN", 1000m),
                new CreditAccount(6, "BY12ALFA...0006", "Сергей Волков", 150m, "BYN", 2000m),
                new Account(7, "BY12ALFA...0007", "Наталья Смирнова", 5000m, "EUR")
            };

            int index = 1;
            foreach (var acc in accounts)
            {
                Console.WriteLine($"--- Счёт #{index++} ---");
                acc.PrintInfo();
            }

            Console.WriteLine($"Всего счетов: {accounts.Count}");
        }

        // ---------- Блок 2, Уровень 2 ----------
        static void Block2_Level2()
        {
            Console.WriteLine("=== Блок 2, Уровень 2: Dictionary и TryGetValue ===\n");

            List<Account> accounts = new List<Account>
            {
                new Account(1, "BY12ALFA...0001", "Арина Вашемирская", 1500.75m, "BYN"),
                new Account(2, "BY12ALFA...0002", "Иван Петров", 320.50m, "USD"),
                new SavingsAccount(3, "BY12ALFA...0003", "Ольга Сидорова", 8000m, "BYN", 5.5),
                new SavingsAccount(4, "BY12ALFA...0004", "Дмитрий Кузнецов", 12000m, "BYN", 7.0),
                new CreditAccount(5, "BY12ALFA...0005", "Елена Морозова", 200m, "BYN", 1000m),
                new CreditAccount(6, "BY12ALFA...0006", "Сергей Волков", 150m, "BYN", 2000m),
                new Account(7, "BY12ALFA...0007", "Наталья Смирнова", 5000m, "EUR")
            };

            Dictionary<int, Account> accountDict = new Dictionary<int, Account>();
            foreach (var acc in accounts) accountDict[acc.Id] = acc;

            Console.WriteLine($"Словарь создан. Записей: {accountDict.Count}\n");

            int[] searchIds = { 2, 5, 99 };
            foreach (int id in searchIds)
            {
                Console.WriteLine($"--- Поиск Id = {id} ---");
                if (accountDict.TryGetValue(id, out Account found))
                    found.PrintInfo();
                else
                    Console.WriteLine($"Счёт с Id={id} не найден.\n");
            }
        }

        // ---------- Блок 2, Уровень 3 ----------
        static void Block2_Level3()
        {
            Console.WriteLine("=== Блок 2, Уровень 3: LINQ ===\n");

            List<Account> accounts = new List<Account>
            {
                new Account(1, "BY12ALFA...0001", "Арина Вашемирская", 1500.75m, "BYN"),
                new Account(2, "BY12ALFA...0002", "Иван Петров", 320.50m, "USD"),
                new SavingsAccount(3, "BY12ALFA...0003", "Ольга Сидорова", 8000m, "BYN", 5.5),
                new SavingsAccount(4, "BY12ALFA...0004", "Дмитрий Кузнецов", 12000m, "BYN", 7.0),
                new CreditAccount(5, "BY12ALFA...0005", "Елена Морозова", 200m, "BYN", 1000m),
                new CreditAccount(6, "BY12ALFA...0006", "Сергей Волков", 150m, "BYN", 2000m),
                new Account(7, "BY12ALFA...0007", "Наталья Смирнова", 5000m, "EUR")
            };

            Console.WriteLine("=== 1. Счета с балансом > 1000 (по убыванию) ===");
            var rich = accounts.Where(a => a.Balance > 1000)
                               .OrderByDescending(a => a.Balance)
                               .Select(a => new { a.OwnerName, a.Balance, a.Currency })
                               .ToList();
            foreach (var a in rich)
                Console.WriteLine($"{a.OwnerName} — {a.Balance:N2} {a.Currency}");

            Console.WriteLine("\n=== 2. Агрегаты ===");
            Console.WriteLine($"Всего:              {accounts.Count}");
            Console.WriteLine($"Суммарный баланс:   {accounts.Sum(a => a.Balance):N2}");
            Console.WriteLine($"Средний баланс:     {accounts.Average(a => a.Balance):N2}");
            Console.WriteLine($"Минимум:            {accounts.Min(a => a.Balance):N2}");
            Console.WriteLine($"Максимум:           {accounts.Max(a => a.Balance):N2}");

            Console.WriteLine("\n=== 3. Группировка по типу счёта ===");
            foreach (var g in accounts.GroupBy(a => a.AccountType))
            {
                Console.WriteLine($"\nТип: {g.Key}");
                Console.WriteLine($"  Количество:       {g.Count()}");
                Console.WriteLine($"  Суммарный баланс: {g.Sum(a => a.Balance):N2}");
                Console.WriteLine($"  Средний баланс:   {g.Average(a => a.Balance):N2}");
            }

            Console.WriteLine("\n=== 4. Группировка по валюте ===");
            foreach (var g in accounts.GroupBy(a => a.Currency))
                Console.WriteLine($"Валюта {g.Key}: {g.Count()} счёт(ов), сумма: {g.Sum(a => a.Balance):N2}");
        }

        // ---------- Блок 3, Уровень 1 ----------
        static async Task Block3_Level1()
        {
            Console.WriteLine("=== Блок 3, Уровень 1: async/await ===\n");

            Console.WriteLine("Запуск обработки транзакций...\n");
            await ProcessTransactionsAsync();
        }

        static async Task ProcessTransactionsAsync()
        {
            Console.WriteLine("Шаг 1: Загрузка данных...");
            await Task.Delay(1000);
            Console.WriteLine("Шаг 2: Проверка балансов...");
            await Task.Delay(1000);
            Console.WriteLine("Шаг 3: Расчёт процентов...");
            await Task.Delay(1000);
            Console.WriteLine("Шаг 4: Сохранение результатов...");
            await Task.Delay(1000);
            Console.WriteLine("Все транзакции обработаны.");
        }

        // ---------- Блок 3, Уровень 2 ----------
        static async Task Block3_Level2()
        {
            Console.WriteLine("=== Блок 3, Уровень 2: Task<List<T>> + try-catch ===\n");

            List<Account> accounts = await GetAccountsFromDbAsync();
            Console.WriteLine($"\nПолучено счетов: {accounts.Count}\n");
            foreach (var acc in accounts) acc.PrintInfo();
        }

        static async Task<List<Account>> GetAccountsFromDbAsync()
        {
            try
            {
                Console.WriteLine("[БД] Устанавливаем соединение...");
                await Task.Delay(1000);
                Console.WriteLine("[БД] Выполняем SQL-запрос...");
                await Task.Delay(500);

                return new List<Account>
                {
                    new Account(1, "BY12ALFA...0001", "Арина Вашемирская", 1500.75m, "BYN"),
                    new Account(2, "BY12ALFA...0002", "Иван Петров", 320.50m, "USD"),
                    new SavingsAccount(3, "BY12ALFA...0003", "Ольга Сидорова", 8000m, "BYN", 5.5),
                    new CreditAccount(4, "BY12ALFA...0004", "Сергей Волков", 150m, "BYN", 2000m)
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ОШИБКА] {ex.Message}");
                return new List<Account>();
            }
            finally
            {
                Console.WriteLine("[БД] Соединение закрыто.");
            }
        }

        // ---------- Блок 3, Уровень 3 ----------
        static async Task Block3_Level3()
        {
            Console.WriteLine("=== Блок 3, Уровень 3: Task.WhenAll + CancellationToken ===\n");

            Console.WriteLine("--- Часть 1: параллельная загрузка ---\n");
            Task<string> t1 = LoadFromSourceAsync("Банк-партнёр №1", 1500);
            Task<string> t2 = LoadFromSourceAsync("Банк-партнёр №2", 2000);
            Task<string> t3 = LoadFromSourceAsync("Банк-партнёр №3", 1000);
            string[] results = await Task.WhenAll(t1, t2, t3);
            foreach (var r in results) Console.WriteLine("  " + r);

            Console.WriteLine("\n--- Часть 2: отмена операций ---\n");
            var cts = new CancellationTokenSource();
            CancellationToken token = cts.Token;

            var long1 = ProcessPaymentsAsync("ПС A", token);
            var long2 = ProcessPaymentsAsync("ПС B", token);
            var long3 = ProcessPaymentsAsync("ПС C", token);

            cts.CancelAfter(2500);

            try
            {
                await Task.WhenAll(long1, long2, long3);
                Console.WriteLine("\nВсе платежи обработаны.");
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("\n[!] Операции отменены (тайм-аут).");
            }
            finally
            {
                cts.Dispose();
            }
        }

        static async Task<string> LoadFromSourceAsync(string sourceName, int delayMs)
        {
            Console.WriteLine($"[{sourceName}] Запрос отправлен...");
            await Task.Delay(delayMs);
            Console.WriteLine($"[{sourceName}] Данные получены.");
            return $"{sourceName}: 42 записи";
        }

        static async Task ProcessPaymentsAsync(string systemName, CancellationToken token)
        {
            for (int i = 1; i <= 10; i++)
            {
                token.ThrowIfCancellationRequested();
                await Task.Delay(500, token);
                Console.WriteLine($"[{systemName}] Обработан платёж #{i}");
            }
        }
    }
}