# Kaftar

Kaftar provides you with a structure that you need for designing your application in [CQRS](https://martinfowler.com/bliki/CQRS.html). Keep in mind that Kaftar only helps you with coming up with clear and well separated [Application Layer](). Kaftar does not provide any facilities for separting read from writes in your physical infrastructure layer.


# :moneybag: Advantages 

Using kaftar you can easily get the following for free.

1. Clear and well separated application layer which results in [Thin Controllers instead of Fat Controllers]().

# :electric_plug: Install

You can easily have Kaftar installed on your project using [Nuget](https://www.nuget.org/packages/KaftarCQRS/0.0.9).

```powershell
Install-Package KaftarCQRS
```

# :gun: Usage

Kaftar promotes a design style called [Command Query Responsibility Segregation (CQRS)](). Every type of web application could reap the benefits of CQRS regardless of complexity or requirements. CQRS brings clarity to the structure of your application layer and provide easy to follow framework for adding new usecases into the system.

In CQRS, each usecase maps to a _Command_ or _Query_.

## :pencil2: Commands

CQRS sees any usecase that is supposed to change the state of system and write to database as a **Command**.


## Queries

CQRS sees any usecase that wants to read data from database as a **Query**.
