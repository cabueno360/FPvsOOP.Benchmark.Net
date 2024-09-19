using FuncVsOOP.Tests.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FuncVsOOP.Tests
{
    public  class OOPRaceConditionTest
    {
        [Fact]
        public async Task OOP_MutableBankAccount_RaceConditionOccurs()
        {
            var account = new BankAccountOOP(1000m);
            int iterations = 1000;

            // Create a list of tasks that perform deposit and withdrawal operations
            var tasks = Enumerable.Range(0, iterations).Select(i =>
            {
                return Task.Run(() =>
                {
                    account.Deposit(1);   // Mutating shared state
                    account.Withdraw(1);  // Mutating shared state
                });
            });

            // Wait for all tasks to complete
            await Task.WhenAll(tasks);

            // The final balance should be incorrect due to race conditions
            Assert.NotEqual(1000m, account.Balance); // Race condition leads to an incorrect balance
        }

        [Fact]
        public async  Task FP_ImmutableBankAccount_NoRaceCondition()
        {
            var account = new BankAccountFP(1000m);
            int iterations = 1000;

            // Create a list of tasks where each task does a deposit and withdrawal operation
            var tasks = Enumerable.Range(0, iterations).Select(i =>
            {
                // Each operation returns a new immutable account
                return Task.Run(() => account.Deposit(1).Withdraw(1));
            });

            // Wait for all tasks to complete and retrieve the final result
            var accounts = await Task.WhenAll(tasks);

            // The final account balance after all operations
            var finalAccount = accounts.Last(); // Get the last account state



            Assert.Equal(1000m, finalAccount.Balance); // Immutability ensures correct balance
        }
    }
}
