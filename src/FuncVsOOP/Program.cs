// See https://aka.ms/new-console-template for more information
using BenchmarkDotNet.Running;
using FuncVsOOP;

Console.WriteLine("Starting!");


var summary = BenchmarkRunner.Run<ExampleService>();


Console.ReadLine();