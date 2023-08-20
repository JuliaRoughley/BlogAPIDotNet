# BlogAPIDotNet
A .NET port of the BlogAPI Python project. This is an API web backend for the frontend app found in: https://github.com/JuliaRoughley/Masterblog-API/tree/main/frontend.

1. Build BlogAPI (.NET Core with SQL LocalDB - best run in Visual Studio 2022) and run dotnet ef migrations add InitialCreate
2. Launch the Masterblog-API/frontend web application (Python, Flask - best run in VS Code)
3. Run the frontend app i.e. in http://127.0.0.1:5001
4. Run the BlogAPI app
5. In the frontend, configure the API URL i.e. https://localhost:7074/api and you should be away!
