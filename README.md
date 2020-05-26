# gqldotnet-testing-included
##Step 1:
create the solution file
dotnet new sln -n MyFancyApi -o .

##create a WebApi project
dotnet new webapi -n MyFancyApi.Service -o ./src/MyFancyApi.Service/

##add it to the solution
dotnet sln MyFancyApi.sln add src/MyFancyApi.Service/MyFancyApi.Service.csproj

##create UnitTest project
dotnet new xunit -n MyFancyApi.UnitTest -o ./tests/MyFancyApi.UnitTest/

##create ComponentTest project
dotnet new xunit -n MyFancyApi.ComponentTest -o ./tests/MyFancyApi.ComponentTest/

##create IntegrationTest project
dotnet new xunit -n MyFancyApi.IntegrationTest -o ./tests/MyFancyApi.IntegrationTest/

##create SystemTest project
dotnet new xunit -n MyFancyApi.SystemTest -o ./tests/MyFancyApi.SystemTest/