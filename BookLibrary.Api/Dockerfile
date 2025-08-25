FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ../BookLibrary.sln .
COPY BookLibrary.Api/BookLibrary.Api.csproj BookLibrary.Api/
COPY BookLibrary.Domain/BookLibrary.Domain.csproj BookLibrary.Domain/
RUN dotnet restore "BookLibrary.Api/BookLibrary.Api.csproj"

COPY . .
WORKDIR "/src/BookLibrary.Api"
RUN dotnet build "BookLibrary.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookLibrary.Api.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookLibrary.Api.dll"]