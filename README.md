# Mysql Entities

Mysql Entitiesis a C# library for dealing with Mysql in thread safe Entity Framework way.

## Installation

Include the Compiled ```MysqlEntities.dll``` in the working project and you are all set and ready to write the code.

## Usage

```csharp
// Include the library
using Mysql.Entities;

//create the classes - (Class name should be exact as the table name)
public class Table
{
   public long id {get;set;}
   ...
}

//Extract connection string
public static string connection_string = "<your mysql connection string>";

//create context object
IDataContext context = new DataContext(connection_string);

//To Insert in to table
await context.AddAsync(new class_name
{
     <property> = <value>,
});

//To read from table
List<class_name> lst = await context.SelectAsync<class>();

//To update the table data
//To Update the tuple from table
var obj = <object to delete>;;
obj.<property> = <value>;
...
await context.UpdateAsync(obj,"<property> = {0}", value);

//To delete from table
var obj = <object to delete>;
await context.DeleteAsync<class>(obj,"<property> = {0}", value);
```

## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

Please make sure to update tests as appropriate.

## dependencies
[MySql.Data.dll @6.9.12](https://www.nuget.org/packages/MySql.Data/6.9.12?_src=template)

## Author
[Pranil Tunga](https://pranilism.github.io/)

## License
[MIT](https://choosealicense.com/licenses/mit/)