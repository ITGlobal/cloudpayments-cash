# cloudpayments-cash
C# client for cloudpayments cash api

[![Build status](https://ci.appveyor.com/api/projects/status/1rrw11v9sw3cftj8/branch/master?svg=true)](https://ci.appveyor.com/project/itgloballlc/cloudpayments-cash/branch/master)

## Usage
Get CashApi object with default implementation
```csharp
using Microsoft.Extensions.Logging;

...

CashApi.GetDefault(new CashSettings { ... }, loggerFactory)
```

Use `Test = true` param in development enviroment
```csharp
CashApi.GetDefault(new CashSettings { ..., Test = true }, loggerFactory)
```

If project use dependency injection, CashApi can register it services in container.
```csharp
public IServiceProvider ConfigureServices(IServiceCollection services)
{
    services.AddCloudPaymentCash(new CashSettings { ... })
    ...
}
```

## Create cash vaucher
```csharp
var api = CashApi.GetDefault(new CashSettings { ... }, loggerFactory)
await api.Receipt(new ReceiptContract { ... }, 123, token)
``` 
Second argument (`123`) is optional and it is used to make requests idempotent.

See [cloudpayments api docs](https://cloudpayments.ru/docs/api/kassa) for more details about `ReceiptContract`.  

## CashSettings description

| Name      | Description                                                            | Default                      |
| --------- | ---------------------------------------------------------------------- | ---------------------------- |
| PublicId  | Public ID from CloudPaynets site settings                              |                              |
| ApiSecret | ApiSecret from CloudPaynets site settings                              |                              |
| Endpoint  | Api endpoint                                                           | https://api.cloudpayments.ru |
| Inn       | Organization INN number. Optional. INN can be set in `ReceiptContract` |                              |
| Test      | Test mode                                                              | false                        |


