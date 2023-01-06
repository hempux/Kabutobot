
### Classes
#### Api endpoint Enums
To simplify using the ninja endpoints in methods they are collected as constants in [src/Ninja/NinjaApiEnums.cs](src/Ninja/NinjaApiEnums.cs) 

#### Objects from ninjas Api

When any method needs information about items from ninja (a device, organization etc) the request is sent to the [`SqliteEngine`](src/Database/SqliteEngine.cs) class that checks if the data is available locally. If not a request is passed to [`NinjaApiV2`](src/Ninja/NinjaApiv2.cs) which will call the ninja API to get the neccessary information. This reduces the need for API calls and the chance of hitting any ratelimiting limits NinjaOne imposes for their APIs.


### Fetching items

Generally when calling items you should use the database to look up objects since it will return objects if it is available in the local database, and if not it will fetch data from your ninjarmm instanace, save it to the sqlite database and then return the data.   
In cases where you need to fetch items directly from a controller use NinjaFetchAsync<T> with Organization for fetching organizations, or NodeWithDetailedReferences for a device.

##### Using sqlite (preferred)
  ```csharp
    engine = new SqliteEngine();
    int nodeId = 123;
    DeviceModel device = await engine.GetDeviceById(nodeId);
```
  
#### Fetching data directly
```csharp
    NinjaApiv2 api = new NinjaApiv2();
    int nodeId = 123;
    DeviceModel device = await api.NinjaFetchAsync<NodeWithDetailedReferences>(NinjaApiEndpoint.getDevice, nodeId);
```
  
