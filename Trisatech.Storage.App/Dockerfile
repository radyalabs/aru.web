#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Trisatech.Storage.App/Trisatech.Storage.App.csproj", "Trisatech.Storage.App/"]
RUN dotnet restore "Trisatech.Storage.App/Trisatech.Storage.App.csproj"
COPY . .
WORKDIR "/src/Trisatech.Storage.App"
RUN dotnet build "Trisatech.Storage.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Trisatech.Storage.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trisatech.Storage.App.dll"]