using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scheta
{
    
    class Program
    {
        public struct BankAccount
        {
            public UInt32 number;
            public DateTime dateOpen;
            public Fio fio;
            public Double balance;
        }

        public struct Fio
        {
            public String sName;
            public String fName;
            public String tName;
        }

        static void Main(string[] args)
        {
            BankAccount[] accounts = new BankAccount[0];
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Создание нового аккаунта...");
            OpenSchet(ref accounts);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Создание второго нового аккаунта...");
            OpenSchet(ref accounts);

            Console.ForegroundColor = ConsoleColor.White;
            while (true)
            {
                Bankrot(accounts);
                uint num = ControlUInt($"Выберите номер счета ({GetAllNumbers(accounts)})");
                for (int i=0; i < accounts.Length; i++)
                {
                    if (accounts[i].number == num && accounts[i].balance > 0)
                    {
                        InfoSchet(accounts[i]);
                        Console.WriteLine("\nВыберите опцию:\n1) Пополнение счета\n2) Снятие со счета\n3) Начисление процентов\n4) Перевод\n5) Создание аккаунта");
                        string choice = Console.ReadLine();
                        switch (choice)
                        {
                            case "1": ImportSchet(ref accounts[i]);break;
                            case "2": ExportSchet(ref accounts[i]); break;
                            case "3": EzhemesCapital(ref accounts[i]);break;
                            case "4": Transfer(ref accounts[i], accounts);break;
                            case "5": OpenSchet(ref accounts);break;
                            default:break;
                        }
                        
                    }
                }
            }
        }

        public static void OpenSchet (ref BankAccount[] account)
        {
            Array.Resize(ref account, account.Length + 1);
            account[account.Length - 1].number = GenerateNumber(account);
            Console.WriteLine($"Номер вашего счета: {account[account.Length - 1].number}");
            account[account.Length - 1].dateOpen = DateTime.Now.Date;
            Console.WriteLine("Введите имя владельца счета");
            account[account.Length - 1].fio.sName = Console.ReadLine();
            Console.WriteLine("Введите фамилию владельца счета");
            account[account.Length - 1].fio.fName = Console.ReadLine();
            Console.WriteLine("Введите отчество владельца счета");
            account[account.Length - 1].fio.tName = Console.ReadLine();
            account[account.Length - 1].balance = ControlBalance("Введите баланс");
        }

        public static void Bankrot (BankAccount[] accounts)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].balance <= 0)
                {
                    Console.WriteLine($"{accounts[i].number} - этот номер счёта удален по причине банкротства");
                    for (int j = i; j < accounts.Length - 2; j++)
                    {
                        accounts[j] = accounts[j + 1];
                        Array.Resize(ref accounts, accounts.Length - 1);
                    }
                }
            }
        }

        public static uint GenerateNumber (BankAccount[] account)
        {
            string genKey = (account.Length - 1).ToString();
            Random ran = new Random();
            uint key;
            do
            {
                for (int j = 0; j < 3; j++)
                {
                    genKey += ran.Next(0, 10).ToString();
                }
                key = Convert.ToUInt32(genKey);
                for (int j = 0; j < account.Length; j++)
                {
                    if (account[j].number == key)
                        key = 0;
                }
            }
            while (key == 0);
            return key;
        }

        public static BankAccount ExportSchet (ref BankAccount account)
        {
            double input;
            input = ControlBalance("Введите сумму для снятия");
            if (input <= account.balance)
            {
                account.balance -= input;
            }
            else
            {
                Console.WriteLine("Введенная сумма больше баланса на счету");
            }
            return (account);
        }

        public static void InfoSchet (BankAccount account)
        {
            Console.WriteLine($"Номер счета: {account.number} " +
                $"\nФИО: {account.fio.sName} {account.fio.fName} " +
                $"{account.fio.tName}\nДата открытия:{account.dateOpen.Date} " +
                $"\nБаланс: {account.balance.ToString("F2")}");
        }

        public static string GetAllNumbers (BankAccount[] account)
        {
            string numbers = "";
            for (int i = 0; i < account.Length; i++)
            {
                numbers += account[i].number.ToString() + ", ";
            }
            numbers = numbers.Substring(0, numbers.Length - 2);
            return numbers;
        }

        public static BankAccount ImportSchet(ref BankAccount account)
        {
            double input;
            input = ControlBalance("Введите сумму для пополнения");
            account.balance += input;
            return account;
        }

        public static BankAccount EzhemesCapital(ref BankAccount account)
        {
            uint mes;
            mes = ControlUInt("Введите количество месяцев");
            account.balance = account.balance * Math.Pow((1.0 + 4.7 / 1200.00), mes);
            return account;
        }

        public static void Transfer(ref BankAccount account1, BankAccount[] accounts)
        {
            uint schetNum = ControlUInt($"Введите номер счета ({GetAllNumbers(accounts)})");
            for (int i=0; i < accounts.Length; i++)
            {
                if (accounts[i].number == schetNum)
                {
                    double input;
                    input = ControlBalance($"Введите сумму для перевода пользователю {accounts[i].fio.sName} {accounts[i].fio.fName} {accounts[i].fio.tName}");
                    if (input <= account1.balance && input > 0)
                    {
                        account1.balance -= input;
                        accounts[i].balance += input;
                        Console.WriteLine("Операция выполнена");
                        return;
                    }
                    else
                        Console.WriteLine("Операция отклонена");
                }
            }
            Console.WriteLine("Операция отклонена");
        }

        static uint ControlUInt(string mess)
        {
            uint rezult; string temp;
            do
            {
                Console.WriteLine(mess);
                temp = Console.ReadLine();
            }
            while (!uint.TryParse(temp, out rezult));
            return (rezult);
        }

        static double ControlBalance(string mess)
        {
            double rezult; string temp;
            do
            {
                do
                {
                        Console.WriteLine(mess);
                        temp = Console.ReadLine();
                }
                while (!double.TryParse(temp, out rezult));
            }
            while (rezult < 0);
            rezult = Math.Round(rezult, 2);
            return (rezult);
        }

    }
}
