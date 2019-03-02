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

CQRS sees any usecase that is supposed to change the state of system and write to database as a **Command**. So, for any request that is going to come from client and is supposed to write to database, at first we need to create a _Command_ object. **Command** is simply a POCO which consists of just fields. **A command is a poco object encompasses user's request**.

Let's say we want to implement a usecase for updating the user's address. First, we create a Command to define the form of request that can submit by client.

```C#
public class UpdateUserAddressCommand : CqrsCommand
{
    public string NewAddress {get; set;}
}
```

We do not need to define a field such as `UserId` because CqrsCommand object has this field already and fills it with the id of user who has submitted the command.

Next, we need to implement a the logic for handling this command. In CQRS, commands are handled by **CommandHandlers**. In other words, for each _Command_, there is a corresponding _CommandHandler_. In Kaftar, all database operations in a Command Handler happen in a single transaction automatically.

The implementation of a command handler is as follows.

```C#
public class UpdateUserAddressCommandHandler : CommandHandler<UpdateUserAddressCommand, CqrsCommandResult>
{
    protected override Task Handle(UpdateUserAddressCommand command)
    {
        var user = DataContext.Set<User>().Single(q => q.Id == command.UserId);
        user.Address = command.UserAddress;
    }   
}
```

As you can see, we do not need to `Save` the results inside the `Handle` method. The idea here is that the developer should not be concerned about anything except doing the business logic and forgetting about the database. 'DataContext' is a wrapper over `EntityFramework` which has all methods except `SaveChanges` and `SaveChangesAsync`. We believe removing these `Save` methods helps developers to think more in terms of domain. `SaveChangesAsync` will be called automatically after executing `Handle` by Kaftar.

## Autorization

## Validation


```C#
public class UpdateUserAddressCommandHandler : CommandHandler<UpdateUserAddressCommand, CqrsCommandResult>
{
    protected override Task Handle(UpdateUserAddressCommand command)
    {
        var user = DataContext.Set<User>().Single(q => q.Id == command.UserId);
        user.Address = command.NewAddress;
    }   

    protected override Task<CqrsCommandResult> PreExecutionValidation(UpdateUserAddressCommand command)
    {
        if (string.IsNullOrEmpty(command.NewAddress))
        {
            return new CqrsCommandResult(100, "New Address Shouldn't be null", command);
        }

        return OkResult();
    }
}
```

## In Case of Failure

Let's say you want to get notified when a failure happens inside the `Handle` method or Kaftar can't execute `SaveChangesAsync` successfully to persist the change you made in `Handle`. Kaftar asks you to implement such a requirement in a method called `OnFailure`. So, `Handle` method would be super clean and only about the actual business logic. Moreover, when somebody looks at your code, they can easily figure out where to find what. Inside the exception method, you have access to the `exception` that has happend.

```C#
public class UpdateUserAddressCommandHandler : CommandHandler<UpdateUserAddressCommand, CqrsCommandResult>
{
    protected override Task Handle(UpdateUserAddressCommand command)
    {
        var user = DataContext.Set<User>().Single(q => q.Id == command.UserId);
        user.Address = command.NewAddress;
    }   
    
    protected override Task OnFail(Exception exception, UpdateUserAddressCommand command, CqrsCommandResult commandResult)
    {
        await SendEmail("support@kaftar",exception)
    }
}
```

## In Case of Success

Let's say we want to send an email to the user after successfully changing the user's address. Since Kaftar promotes separation of concerns, it advises you to implement this requirement inside a method called `OnSucess` instead of having it inside the `Handle` method. `OnSuccess` is executed by Kaftar, if `Handle` is executed successfully.


```C#
public class UpdateUserAddressCommandHandler : CommandHandler<UpdateUserAddressCommand, CqrsCommandResult>
{
    protected override Task Handle(UpdateUserAddressCommand command)
    {
        var user = DataContext.Set<User>().Single(q => q.Id == command.UserId);
        user.Address = command.NewAddress;
    }   
    
    protected override Task OnSucess(UpdateUserAddressCommand command, CqrsCommandResult commandResult)
    {
        await SendEmail(command.UserId);
    }
}
```

Next, we define an action method for handling this command.

```C#
public IActionResult UpdateUserAddress(UpdateUserAddressCommand command)
{
   
}

```



## :books: Queries

CQRS sees any usecase that wants to read data from database as a **Query**.
