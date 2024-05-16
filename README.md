# Lazy.Api 

### Lazy by the commitment of development it requires, but never by the speed & functionality it provides 😅

`Lazy.Api` is a RESTful api written using .NET, ideal for lazy developers. It requires minimal dedication for adding CRUD functionality for new domain entities.

### Do you want to try this?

Follow steps shown below, and you'll be all good.

- For generic CRUD endpoints:
  1. Define the new entity under `/Entities` directory, make sure it inherits the [BaseEntity](https://github.com/fatlummaliqi/Lazy.Api/blob/main/Lazy.Api/Entities/Base/BaseEntity.cs) type.
  2. If you want a well defined route name for this entity, you can set one by marking this entity with [EntityRouteAttribute](https://github.com/fatlummaliqi/Lazy.Api/blob/main/Lazy.Api/Infrastructure/Attributes/EntityRouteAttribute.cs) attribute and providing a route name.
  3. Add the EF Core migration for this entity and you should be all set.
 
- For specific endpoints:
  1. Follow the same first step as above
  2. In order to ignore this new entity from generic CRUD endpoints, mark it with the [IgnoreEntityResourceAttribute](https://github.com/fatlummaliqi/Lazy.Api/blob/main/Lazy.Api/Infrastructure/Attributes/IgnoreEntityResourceAttribute.cs) attribute.
  3. Don't forget to add the new EF Core migration required for the new entity.
  4. Last but not least define the entity resource endpoints by defining a class which implements the [IEndpointsGroup](https://github.com/fatlummaliqi/Lazy.Api/blob/main/Lazy.Api/Infrastructure/IEndpointsGroup.cs) interface, An example of specific resource endpoints is shown [here](https://github.com/fatlummaliqi/Lazy.Api/blob/main/Lazy.Api/Endpoints/Users.cs).
