# build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
# a .sln está uma pasta acima do contexto atual
RUN dotnet restore ../FSI.AccessAuthentication.sln
RUN dotnet publish FSI.AccessAuthentication.Api.csproj -c Release -o /app/publish /p:UseAppHost=false

# runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
ENTRYPOINT ["dotnet","FSI.AccessAuthentication.Api.dll"]
