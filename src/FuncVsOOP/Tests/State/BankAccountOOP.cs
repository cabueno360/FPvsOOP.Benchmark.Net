using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncVsOOP.Tests.State
{
    public class BankAccountOOP
    {
        public decimal Balance { get; private set; }

        public BankAccountOOP(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        public void Deposit(decimal amount)
        {
            Balance += amount; // Mutating shared state
        }

        public void Withdraw(decimal amount)
        {
            Balance -= amount; // Mutating shared state
        }
    }
}
