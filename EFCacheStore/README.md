# IgniteEFCacheStore
Example for Apache Ignite.NET and EF

- install ignite in your machine or VM (virtual machine) https://apacheignite.readme.io/docs/getting-started
   tested with V 2.7

- open this project with Visual Studio (I used vs 2017)

- optional: modify the ignite config file 
   I copied the default file provided by Ignite into this project

- run the Program.cs (F5)
   a console (cmd) with the ignite logo should be displayed
   note: it creates the database file (see SqlServerCe for more info)

- optional: use the command $IGNITE_HOME$\bin\ignitevisorcmd.bat to view the topology 

// more information and examples 
//   https://apacheignite-net.readme.io/docs/entity-framework-second-level-cache
//   https://github.com/apache/ignite
//   I didn't try this, but it seems good https://github.com/apache/ignite/blob/master/modules/platforms/dotnet/Apache.Ignite.EntityFramework.Tests/EntityFrameworkCacheTest.cs

push disclamer: the previous version of this example worked with version ignite 1.7 I just updated it (nuget) to 2.7
