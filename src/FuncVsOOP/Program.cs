// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using FuncVsOOP;
using FuncVsOOP.Benchmark;

Console.WriteLine("Starting!");


var summary = BenchmarkRunner.Run(new[]
       {
            typeof(ExceptionVsResultBenchmark),
            typeof(BankAccountBenchmark)
        });


Console.ReadLine();