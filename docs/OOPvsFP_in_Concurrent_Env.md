
# Explanation of the Tests: OOP vs. FP in Concurrent Environments

In this project, we are comparing two different programming paradigms—**Object-Oriented Programming (OOP)** with mutable state and **Functional Programming (FP)** with immutable state—to demonstrate how they behave in a concurrent environment. Specifically, we are testing how each approach handles multiple threads performing operations on a bank account balance simultaneously.

## 1. OOP Mutable State Test: `OOP_MutableBankAccount_RaceConditionOccurs`

### **Code Snippet**

```csharp
public class OOPRaceConditionTest
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
}
```

### **Explanation**

#### **Initial Setup**

- **Account Creation**: We instantiate a mutable bank account with an initial balance of **1000**.
- **Iterations**: We set the number of iterations (and hence tasks) to **1000**.

#### **Task Creation**

- We create **1000 tasks** using `Enumerable.Range` and `Select`.
- Each task performs two operations on the **same** `account` instance:
  - **Deposit**: Adds 1 to the account balance.
  - **Withdraw**: Subtracts 1 from the account balance.
- The tasks are started using `Task.Run`, which schedules them to run asynchronously.

#### **Concurrency and Race Conditions**

- All tasks run **concurrently** and **mutate the shared `account.Balance`**.
- Since multiple threads are modifying the same shared variable without any synchronization mechanisms (like locks), **race conditions** occur.
- **Race Condition**: A situation where the system's substantive behavior is dependent on the sequence or timing of uncontrollable events (e.g., thread scheduling).

#### **Expected Result**

- Due to race conditions, the final balance is **likely not equal to the initial balance** of 1000.
- We use `Assert.NotEqual(1000m, account.Balance)` to verify that the balance has changed unexpectedly.



## Why the OOP Approach Failed

### **Shared Mutable State**

- The `Balance` property is **shared** among all tasks and is **mutable**.
- Multiple threads accessing and modifying `Balance` simultaneously leads to inconsistent states.

### **Lack of Thread Safety**

- The `Deposit` and `Withdraw` methods do not implement any synchronization mechanisms.
- Operations like `Balance += amount` are **non-atomic**; they consist of multiple steps (read-modify-write).

### **Non-Atomic Operations and Interleaving**

- **Non-Atomicity**: The read-modify-write sequence can be interrupted by other threads.

#### **Example Scenario**:

- Thread A reads `Balance` (e.g., 1000).
- Thread B reads `Balance` (also 1000).
- Thread A adds 1 and writes back 1001.
- Thread B adds 1 and writes back 1001 (overwriting Thread A's update).
- This results in a **lost update**, causing the final balance to be incorrect.

### **Conclusion**

- The OOP approach with mutable shared state is **prone to race conditions** in concurrent environments.
- Without proper synchronization (e.g., locks), **data integrity cannot be guaranteed**.




## 2. FP Immutable State Test: `FP_ImmutableBankAccount_NoRaceCondition`

### **Code Snippet**

```csharp
public class OOPRaceConditionTest
{
    [Fact]
    public async Task FP_ImmutableBankAccount_NoRaceCondition()
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
```

### **Explanation**

#### **Initial Setup**

- **Account Creation**: We instantiate an immutable bank account (`BankAccountFP`) with an initial balance of **1000**.
- **Iterations**: We set the number of iterations (tasks) to **1000**.

#### **Task Creation**

- We create **1000 tasks** using `Enumerable.Range` and `Select`.
- Each task performs operations on the `account` but does not mutate it:
  - **Deposit**: Returns a new `BankAccountFP` instance with the balance increased by 1.
  - **Withdraw**: Returns another new `BankAccountFP` instance with the balance decreased by 1.
- Each task operates on its own chain of immutable instances.

#### **Concurrency Without Race Conditions**

- Since `BankAccountFP` is immutable, **no shared state is modified**.
- Each task works independently, and there's **no interference** between tasks.
- **Immutability** ensures that once an instance is created, its state cannot change.

#### **Result Collection**

- We use `await Task.WhenAll(tasks)` to wait for all tasks to complete.
- The results are collected in the `accounts` array.
- We retrieve the `finalAccount` from `accounts.Last()`, although any account in the array would have the same balance due to immutability.

#### **Expected Result**

- The final balance remains **1000**, as each deposit and withdrawal cancel each other out.
- We use `Assert.Equal(1000m, finalAccount.Balance)` to verify the balance is correct.

### **Why the FP Approach Didn't Fail**

#### **Immutability**

- `BankAccountFP` is a **readonly record struct**, meaning it is inherently immutable.
- Operations return **new instances** rather than modifying existing ones.

#### **No Shared Mutable State**

- There is **no shared state** being modified across threads.
- Each task operates on its **own copy** of the data.

#### **Thread Safety**

- Immutable objects are **thread-safe** by nature because their state cannot change after creation.
- This eliminates the need for synchronization mechanisms like locks.

#### **Functional Programming Principles**

- Emphasizes **pure functions** and **statelessness**.
- Side effects are minimized, reducing the potential for bugs in concurrent environments.

#### **Conclusion**

- The FP approach with immutable state **avoids race conditions**.
- It provides a safer and more predictable behavior in concurrent scenarios.



## Additional Details

### **Bank Account Implementations**

#### **Mutable Bank Account (OOP)**

```csharp
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
```

- **Characteristics**:
  - **Mutable**: The `Balance` can be changed after the object is created.
  - **Shared State**: Multiple threads can access and modify `Balance`.

#### **Immutable Bank Account (FP)**

```csharp
public readonly record struct BankAccountFP(decimal Balance);

public static class BankAccountExt
{
    public static BankAccountFP Deposit(this BankAccountFP @this, decimal amount) =>
        new BankAccountFP(@this.Balance + amount); // New object, no mutation

    public static BankAccountFP Withdraw(this BankAccountFP @this, decimal amount) =>
        new BankAccountFP(@this.Balance - amount); // New object, no mutation
}
```

- **Characteristics**:
  - **Immutable**: Once created, the `Balance` cannot be changed.
  - **Pure Functions**: Methods return new instances without side effects.



## Summary and Conclusions

### **Why the OOP Test Fails**

- **Race Conditions**: Occur due to concurrent modifications of shared mutable state.
- **Non-Atomic Operations**: Read-modify-write sequences are interrupted by other threads.
- **Lack of Synchronization**: No locks or other mechanisms to ensure thread safety.
- **Result**: Final balance is inconsistent and often incorrect.

### **Why the FP Test Succeeds**

- **Immutability**: No shared state is modified; each operation works on a separate instance.
- **Thread Safety**: Immutable objects are inherently thread-safe.
- **No Side Effects**: Operations do not affect other threads or shared data.
- **Result**: Final balance remains consistent and correct.

---

## Recommendations

- **Prefer Immutability**: In concurrent applications, use immutable data structures to avoid race conditions.
- **Functional Programming Practices**: Embrace pure functions and avoid shared mutable state.
- **Thread Safety in OOP**: If using mutable objects, ensure thread safety through synchronization mechanisms like locks (though this can impact performance).
- **Code Readability and Maintenance**: Immutable and functional approaches often lead to code that is easier to reason about and maintain.

---

## Practical Implications

- **Performance Considerations**: While immutable objects can lead to more allocations (due to creating new instances), the trade-off is safer concurrent code without the overhead of locks.
- **Scalability**: Applications using immutable data structures can scale better in multi-threaded environments.
- **Debugging and Testing**: Immutability reduces the complexity of debugging concurrent code, as there are fewer states and interactions to consider.

---

## Conclusion

This comparison demonstrates that:

- **Mutable shared state** in OOP can lead to **race conditions** and **data inconsistencies** in concurrent environments.
- **Immutable data structures** and **functional programming** techniques provide a robust solution to concurrency issues by eliminating shared mutable state.
- Choosing the right programming paradigm and data structures is crucial for building reliable, concurrent applications.

By understanding and applying these principles, developers can write safer, more efficient, and maintainable code in multi-threaded applications.

