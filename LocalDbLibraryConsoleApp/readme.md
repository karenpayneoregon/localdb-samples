# About

Provide code to create a LocalDb using code in LocalDbLibrary.

There are two methods

- Version1 which creates a new database, one table and populates the table only if the database does not exists.
- Version2 which creates the database each time the method is called

Both methods will display records in Visual Studio's output window rather directly in the console window via Debug.WriteLine.

Post build event ensures the database folder exists.

```
if not exist $(TargetDir)\Data mkdir $(TargetDir)\Data
```