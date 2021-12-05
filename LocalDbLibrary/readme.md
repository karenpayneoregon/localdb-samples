# About

Provides base code to create a SQL-Server localDb. If the connection string used does not work, see other connection strings [here](https://www.connectionstrings.com/sql-server/).

## Base operations

- Create a database under bin\debug\Data (calling application has a post-build event to ensure the data folder exists)
- Create a simple table
- Populate the table
- Display in a console app using Debug.WriteLine so output will be seen Visual Studio's `output window`.
  - Options
    - List
    - DataTable
- There is a method to check if the database needs to be created.

## Caveats

If the database is created in one folder than another folder before creating the database in the second folder first detach it else a runtime exception is raised, *database already exists*.

## Recommendations

Don't just use this code without taking time to understand the code which if there is a need to modify the code or there are issues you have at least base understanding of the code.
