
### Classes
#### Api endpoint Enums
To simplify using the ninja endpoints in methods they are collected as constants in [Ninja/NinjaApiEnums.cs](Ninja/NinjaApiEnums.cs) 

#### Response Models
The responses have models in [Ninja/ApiModels](Ninja/ApiModels) so that they can be deserialized into c# objets with named properties.


### Fetching items

Generally when calling items you should use the database to look up objects (for exmaple SqliteEngine.GetDeviceById(int id)), and *not* use NinjaFetchAsync<T> as that will not cache the response.
The Database method will first check if the device is already cached and return the database object, and if not will request it from the API.
