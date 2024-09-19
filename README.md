
# FPvsOOP.Benchmark.Net

## Overview

This project benchmarks two approaches for handling errors in asynchronous methods: one using exceptions and another using a custom `Result<T>` type for functional-style error handling. The goal is to compare the performance and memory usage of each approach and highlight the trade-offs.

## Benchmark Results Summary

We benchmarked two methods:

1. **`AsyncMethodWithException`**: This method throws an `ArgumentException` on every call.
2. **`AsyncMethodWithCustomResult`**: This method returns a custom `Result<T>` object, encapsulating either a successful result or an error without throwing exceptions.

| Method                       | Mean       | Error       | StdDev      | Gen0   | Allocated |
|------------------------------|------------|-------------|-------------|--------|-----------|
| `AsyncMethodWithException`    | 12,275.37 ns | 952.969 ns  | 2,794.892 ns | 0.1373 | 1160 B    |
| `AsyncMethodWithCustomResult` | 61.14 ns   | 2.043 ns    | 5.928 ns    | 0.0382 | 320 B     |

### Analysis

- **`AsyncMethodWithException`**: 
   - **Performance**: Average execution time is 12,275.37 ns.
   - **Memory Allocation**: Allocates 1160 bytes per call.
   - **Overhead**: Exception handling is expensive in .NET, contributing to the slower execution and higher memory usage.
  
- **`AsyncMethodWithCustomResult`**: 
   - **Performance**: Average execution time is only 61.14 ns, a significant improvement over the exception-based approach.
   - **Memory Allocation**: Allocates just 320 bytes per call, demonstrating much better memory efficiency.

### Conclusion

- The **`AsyncMethodWithCustomResult`** is far more efficient both in terms of execution time and memory consumption compared to **`AsyncMethodWithException`**.
- Using exceptions for flow control can significantly impact performance in .NET, especially when exceptions occur frequently.
- **Functional-style** error handling using `Result<T>` is a better alternative for scenarios where performance and resource management are critical.

## Functional.DotNet Package

This project utilizes the **[Functional.DotNet](https://www.nuget.org/packages/Functional.DotNet/)** library for functional-style error handling through the `Result<T>` monad. The `Result<T>` structure is used to encapsulate both successful values and exceptions in a more controlled and efficient manner.

- **GitHub**: [https://github.com/cabueno360/Functional.DotNet](https://github.com/cabueno360/Functional.DotNet)
- **NuGet**: [Functional.DotNet on NuGet](https://www.nuget.org/packages/Functional.DotNet/)

## How to Run the Benchmark

To run the benchmark, clone the project and run the following command from the root directory:

```bash
dotnet run -c Release
```

The benchmarks are performed using the **[BenchmarkDotNet](https://benchmarkdotnet.org/)** library, which will generate detailed performance reports.

## Future Improvements

- Explore additional functional programming techniques to further reduce overhead and improve performance consistency.
