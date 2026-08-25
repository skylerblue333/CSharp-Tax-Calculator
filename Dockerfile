FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY CSharp-Tax-Calculator.csproj ./
RUN dotnet restore CSharp-Tax-Calculator.csproj
COPY Program.cs TaxCalculator.cs ./
RUN dotnet publish CSharp-Tax-Calculator.csproj -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
COPY --from=build /app ./
USER app
EXPOSE 8080
ENTRYPOINT ["dotnet", "CSharp-Tax-Calculator.dll"]
