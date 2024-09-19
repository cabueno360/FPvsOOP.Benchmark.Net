using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncVsOOP.Tests.State
{
    public readonly record struct BankAccountFP(decimal Balance);

    public static class BankAccountExt
    {
        public static BankAccountFP Deposit(this BankAccountFP @this, decimal amount) =>
            new BankAccountFP(@this.Balance + amount); // New object, no mutation


        public static BankAccountFP Withdraw(this BankAccountFP @this, decimal amount) =>
          new BankAccountFP(@this.Balance - amount); // New object, no mutation
    }

}
