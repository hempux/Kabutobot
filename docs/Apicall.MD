
### Classes

#### Objects from ninjas Api

When any method needs information about items from ninja (a device, organization etc) the request is sent to the [`SqliteEngine`](../src/Database/SqliteEngine.cs) class that checks if the data is available locally. If not a request is passed to [`NinjaApiV2`](../src/Ninja/NinjaApiv2.cs) which will call the ninja API to get the neccessary information. This reduces the need for API calls and the chance of hitting any ratelimiting limits NinjaOne imposes for their APIs.


### Fetching items

Generally when calling items you should use the database to look up objects since it will return objects if it is available in the local database, and if not it will fetch data from your ninjarmm instanace, save it to the sqlite database and then return the data.  
The DeviceModel/OrganizationModel classes that SqliteEngine returns has a One-to-One/Has-Many relationsships preconfigurured so that if you fetch a DeviceModel it will also return the organization name,and organizations will return known(cached) devices.

In cases where you need to fetch items directly from a controller use NinjaFetchAsync<T> with Organization for fetching organizations, or NodeWithDetailedReferences for a device.

#### Using sqlite (preferred)
  ```csharp
    SqliteEngine engine = new SqliteEngine();
    int nodeId = 123;
    DeviceModel device = await engine.GetDeviceById(nodeId);
```

##### Api endpoint Enums
To simplify using the ninja endpoints in methods the ones that are used are collected as constants in [`NinjaApiEnums.cs`](../src/Ninja/NinjaApiEnums.cs) so they can be used as the first param for `NinjaFetchAsync<T>`(like in the example below)
  
##### Fetching data directly
```csharp
    NinjaApiv2 api = new NinjaApiv2();
    int nodeId = 123;
    DeviceModel device = await api.NinjaFetchAsync<NodeWithDetailedReferences>(NinjaApiEndpoint.getDevice, nodeId);
```


### Updating models
The ninjaRMM API webhook stubs are generated from openAPI/swagger using [`OpenAPI-Generator`](https://github.com/OpenAPITools/openapi-generator#13---download-jar) with definitions from https://resources.ninjarmm.com/API/webhook.yaml with documentation available [here](https://eu.ninjarmm.com/apidocs/)

```bash
# Re-generate models from openAPI code
java -jar openapi-generator-cli.jar generate -i https://resources.ninjarmm.com/API/webhook.yaml -g aspnetcore -o ninjawebhook --additional-properties packageName=net.hempux.ninjawebhook,buildTarget=library,operationResultTask=true,OperationsAsync=true,sourceFolder=ninjawebhook```
```
After this copy over the following files to the `src/Ninja/WebhookClasses` folder
* CustomEnumConverter.cs
* DetailedActivity.cs
* Location.cs
* Maintenance.cs
* NodeReferences.cs
* NodeRole.cs
* NodeWithDetailedReferences.cs
* Organization.cs
* Policy.cs
