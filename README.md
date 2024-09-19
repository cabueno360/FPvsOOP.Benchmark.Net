
# FPvsOOP.Benchmark.Net

## Overview

**FPvsOOP.Benchmark.Net** is a project designed to compare **Functional Programming (FP)** and **Object-Oriented Programming (OOP)** approaches in .NET. The goal is to demonstrate the strengths and weaknesses of each approach in various scenarios, helping developers understand when to use each one.

This project currently includes benchmarks that cover two primary scenarios:

1. [**Exception vs. Result**](https://github.com/cabueno360/FPvsOOP.Benchmark.Net/blob/main/docs/ExceptionVsResult.md): A comparison between handling errors via exceptions (OOP) and result types (FP).
2. [**Concurrency in Shared State**](https://github.com/cabueno360/FPvsOOP.Benchmark.Net/blob/main/docs/OOPvsFP_in_Concurrent_Env.md): A benchmark that illustrates race conditions in shared mutable state (OOP) and shows how immutability (FP) prevents such issues.

## How to Use

Feel free to explore the existing benchmarks and run them to see the results. The project uses [BenchmarkDotNet](https://benchmarkdotnet.org/) to measure and compare the performance of different programming paradigms.

### Running the Benchmarks

To run the benchmarks, execute the following command in the project directory:

```bash
dotnet run -c Release
```

## Contributing

We encourage developers to add their own tests and benchmarks that compare FP and OOP in other scenarios. Some ideas include:

- Error handling
- State management
- Data processing
- Parallelism and threading

If you have a new test or benchmark that you'd like to contribute, feel free to submit a **Pull Request (PR)**.

## Current Benchmarks

- **Exception vs Result**: A comparison of error handling techniques.
- **Concurrency in Shared State**: A comparison between shared mutable state in OOP and immutable data structures in FP.

## License

This project is open-source, and contributions are welcome! Please create issues or pull requests if you'd like to collaborate.
