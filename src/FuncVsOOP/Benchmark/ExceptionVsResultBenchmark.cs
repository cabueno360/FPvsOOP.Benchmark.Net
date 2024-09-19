using BenchmarkDotNet.Attributes;
using Functional.DotNet.Monad;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FuncVsOOP
{
    [MemoryDiagnoser]
    public class ExceptionVsResultBenchmark
    {
        private async Task<string> AsyncMethodThatThrows(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentException("Invalid argument provided.");


            return await Task.FromResult("Valid");
        }

        private async Task<Result<string>> AsyncMethodWithResult(string name)
        {
            if (string.IsNullOrEmpty(name))
                return Result.Error<string>("Invalid argument provided.");


            return await Task.FromResult(Result.Success("Valid"));
        }


        [Benchmark]
        public async Task<IResult> AsyncMethodWithException()
        {
            try
            {
                await AsyncMethodThatThrows(string.Empty);
                return Results.Ok("Success");
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        }

        [Benchmark]
        public async Task<IResult> AsyncMethodWithCustomResult()
        {
            var result = await AsyncMethodWithResult(string.Empty);
            return result.Match(
                OnSuccess: (result) => Results.Ok("Sucess"),
                OnFailure: (error) => Results.BadRequest(error.Message))!;
        }
    }
}
