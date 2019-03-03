# Kaftar

Kaftar is borned out of an experience I had in developing a large scale organizational software. It arms you with the structure that you need for designing your application in [Command Query Responsibility Segregation (CQRS)](https://martinfowler.com/bliki/CQRS.html) style. Every type of web application could reap the benefits of **Kaftar** regardless of complexity or requirements. It brings clarity to the structure of your application layer and provides an easy to follow framework for adding new usecases into the system. (It does not provide any facilities for separating read from writes in your physical infrastructure layer.)

`CQRS` is a way for writing the *Application Layer* in a very well-organized way. In this approach, any request from users is either a `Command` or a `Query` which gets executed by their corresponding `CommandHandler` or `QueryHandler` respectively. CQRS defines _Command_ and _Query_ as follows.

* **Command**: represents a user request that has a side effect by changing the system's state. Requests which lead to any of the _Update_, _Create_, _Delete_ operations on system's data, are considered as a command.

* **Query**: represents a user request that has no side effect on system's state and only wants to _Read_ data. 

In Kaftar, `CommandHanlder` and `QueryHandler` serve as a template for implementing different aspects of a usecase such as _Authorization_, _Validation_, _Handling Request_, _On Failure_ and _On Success_. Each of these aspects, implement in their own dedicated method. So, it forces developers to think about different concerns separately and have them in their own place instead of writing all different concerns in one method or in inconsistent ways. 

Kaftar's structure results in a highly _Readable_, _Maintainable_ and _Consistent_ system design. As a result, when somebody wants to add or modify a feature in system, they exactly know what the steps are for implementing a new feature or where they need to look for if they want to modify something.


# :moneybag: Advantages 

Using kaftar you can easily get the following for free.

1. Clear and well separated application layer which results in [Thin Controllers instead of Fat Controllers]().

# :electric_plug: Install

You can easily have Kaftar installed in your project using [Nuget](https://www.nuget.org/packages/KaftarCQRS/0.0.9).

```powershell
Install-Package KaftarCQRS
```

# :gun: Usage

In CQRS, each usecase maps to a _Command_ or _Query_.

## :pencil2: Commands

To implement a usecase that is seen as a Command, we need to take 2 steps

1. Create a _Command_ class. **Command** is a simple POCO inherited from **CqrsCommand** that encompasses user's request.
2. Create a _CommandHandler_ class to execute the _Command_.

Let's say we want to implement a usecase for updating the user's address. First, we create a Command to define the form of the request that can be issued by client.

```C#
public class UpdateUserAddressCommand : CqrsCommand
{
    public string NewAddress {get; set;}
}
```

We do not need to define a field such as `UserId`,`IpAddress`,`IssueDateTime` because CqrsCommand object has this fields already and fills them with appropriate values automatically.

Next, we need to implement the logic for handling this command. In CQRS, commands are handled by **CommandHandlers**. In other words, for each _Command_, there is a corresponding _CommandHandler_. 

In Kaftar, all database operations in a Command Handler happen in a single atomic transaction. Kaftar's CommandHandelrs force you to put _Authorization_, _Validation_, _Handle_, _Failure_, _Success_ logic in their own place and separately. So, there would be a consistent way all over the project for implementing different parts of a usecase. This template for implementing usecases helps both analysts and developers to think more vividly about needs of the project.

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

As you can see, we do not need to `Save` the results inside the `Handle` method. The idea here is that the developer should not be concerned about anything except doing the business logic. They should forget about persistence concern. 'DataContext' is a wrapper over `EntityFramework` which has all methods except `SaveChanges` and `SaveChangesAsync`. We believe removing these `Save` methods helps developers to think more in terms of domain. `SaveChangesAsync` will be called automatically after executing `Handle` by Kaftar.

## Authorization
In Asp.Net, authorization is done using `Roles`. However, there are cases that we need more specific `Policies` for checking for authorization. In `Authorization` we check whether a user is allowed to execute the requested operation.  It is a compelete different concern from `Validation`. For example, according to our usecase a policy for authotization coulde be like _if a user has an ongoing order we do not want to allow them to change their address_. 
Kaftar executes `ActivityAuthorizationIsConfirmed` method as the first method to make sure the request is authorized.

```C#
public class UpdateUserAddressCommandHandler : CommandHandler<UpdateUserAddressCommand, CqrsCommandResult>
{
    protected override Task Handle(UpdateUserAddressCommand command)
    {
        var user = DataContext.Set<User>().Single(q => q.Id == command.UserId);
        user.Address = command.NewAddress;
    }   

    protected override bool ActivityAuthorizationIsConfirmed(UpdateUserAddressCommand command)
    {
        var hasOnGoingOrder = SetDataContext.Set<UserOrder>().Any(q => q.UserId == command.UserId && !q.HasDelivered);
        return !hasOnGoingOrder;
    }
}
```

## Validation

Almost always, we need to do a validation before perfoming the operation requested by user. Kaftar's Command Hanlders have a method named `PreExecutionValidation` to put the validation logic there. This method gets executed just before the `Handle` method and after a call to `ActivityAuthorizationIsConfirmed`.
Return an instance of `CqrsCommandResult` with anything other than 0 as `errorCode` will be considered as a failed validation. Using `OkResult()` is a shortcut to returning a successful `CqrsCommandResult` with `errorCode=0`.

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

Let's say you want to get notified when a failure happens inside the `Handle` method or Kaftar can't execute `SaveChangesAsync` successfully to persist the changesyou made in `Handle` method. Kaftar wants you to implement such a requirement in a method called `OnFailure`. So, `Handle` method would be super clean and only about the actual business logic. Moreover, when somebody looks at your code, they can easily figure out where to find what. 

Inside `OnFailure` method, you have access to the `exception` that has happend.

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

## :books: Queries

CQRS sees any usecase that wants to read data from database as a **Query**. It takes three steps to fully implement a Query.

1. **Query**: A POCO object inherited from **CqrsQuery** which represents the form of a request issued by client.
2. **QueryResult**: A POCO object for defining the shape of the desired result to return to user.
3. **QueryHandler**: Implements the required logic for gathering data from datastore and returning it back to user. It is inherited from `QueryHandler`.


Let's say we have a usecase that asks for returning orders of a user filtered by status and date. First, we define a `Query` called `GetUserOrdersQuery` as follows.

```C#

public class GetUserOrdersQuery : CqrsQuery
{
    public DateTime? FromDateTime { get; set; }

    public DateTime? ToDateTime { get; set; }

    public OrderStatus? OrderStatus { get; set; }
}
```

We do not need to define a field such as `UserId`,`IpAddress`,`IssueDateTime` because CqrsQuery object has this fields already and fills them with appropriate values automatically.

Next, we define the desired `QueryResult` that we want to return to user.

```C#

public class GetUserOrdersQueryResult
{
    
    public Order[] Orders {get;set;}
   
    public class Order 
    {
        public DateTime DateTime { get; set; }

        public String Address { get; set; }

        public String ProductName { get; set; }

        public long ProductId { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }
}
```

In QueryResults, we avoid using domain types or reusing other QueryResults. This make each Query completely separate from other Queries and consequently we can change each of them without being worried about side-effects. Here, we define a custom `Order` as a DTO that we want to return to user.



```C#

public class GetUserOrderQueryHandler:QueryHandler<GetUserOrdersQuery, GetUserOrdersQueryResult>
{
    protected override Task<GetUserOrdersQueryResult> GetResult(GetUserOrdersQuery query)
    {
        var orders  = DataContext.Set<Order>().Where(q => q.UserId == query.UserId
        && (q.DateTime >= query.FromDateTime && q.DateTime <= query.ToDateTime))
            .ToArray();

        var result = new GetUserOrdersQueryResult()
        {
            Orders = orders.Select(q => new GetUserOrdersQueryResult.Order()
            {
                Address = q.Address,
                ProductId = q.ProductId,
                ProductName = q.ProductName,
                DateTime = q.DateTime.Value,
                OrderStatus = q.OrderStatus,
            }).ToArray()
         };
    }
}

```
