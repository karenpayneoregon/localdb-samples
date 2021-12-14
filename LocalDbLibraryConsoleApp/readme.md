# About

Provide code to create a LocalDb using code in LocalDbLibrary.

Coding done in Visual Studio 2019, C# 9, .NET Core 5 Framework.

## Notes

- Do not simply run the code, instead walkthrough the code to get an idea of what is going one.
- Do not rely on `LocalDb` for production work 

# Create simple database with one table
There are two methods

- Version1 which creates a new database, one table and populates the table only if the database does not exists.
- Version2 which creates the database each time the method is called

Both methods will display records in Visual Studio's output window rather directly in the console window via Debug.WriteLine.

Post build event ensures the database folder exists.

```
if not exist $(TargetDir)\Data mkdir $(TargetDir)\Data
```

# Create NorthWind database with tables

This code sample will attempt to drop the database, if the database exists it's dropped else an exception is thrown and caught which is cheaper than checking to see if the database exists first.

Next the database is created followed by populating tables with data.

Last step, perform a read on two tables.