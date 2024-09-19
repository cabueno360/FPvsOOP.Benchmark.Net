using BenchmarkDotNet.Attributes;
using FuncVsOOP.Tests.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncVsOOP.Benchmark
{
    [MemoryDiagnoser]
    public class BankAccountBenchmark
    {
        private BankAccountOOP accountOOP;
        private BankAccountFP accountFP;

        [Params(100, 1000, 10000)] // Test with different numbers of iterations
        public int Iterations;

        [GlobalSetup]
        public void Setup()
        {
            accountOOP = new BankAccountOOP(1000m);
            accountFP = new BankAccountFP(1000m);
        }

        [Benchmark]
        public async Task MutableBankAccountRaceCondition()
        {
            var tasks = Enumerable.Range(0, Iterations).Select(i =>
            {
                return Task.Run(() =>
                {
                    accountOOP.Deposit(1);  // Mutating shared state
                    accountOOP.Withdraw(1); // Mutating shared state
                });
            });

            await Task.WhenAll(tasks);
        }

        [Benchmark]
        public async Task ImmutableBankAccountNoRaceCondition()
        {
            var tasks = Enumerable.Range(0, Iterations).Select(i =>
            {
                return Task.Run(() =>
                {
                    return accountFP.Deposit(1).Withdraw(1); // Immutable objects, no mutation
                });
            });

            await Task.WhenAll(tasks);
        }
    }

}
