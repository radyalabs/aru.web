#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Trisatech.MWorkforce.Job/Trisatech.MWorkforce.Job.csproj", "Trisatech.MWorkforce.Job/"]
RUN dotnet restore "Trisatech.MWorkforce.Job/Trisatech.MWorkforce.Job.csproj"
COPY . .
WORKDIR "/src/Trisatech.MWorkforce.Job"
RUN dotnet build "Trisatech.MWorkforce.Job.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Trisatech.MWorkforce.Job.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trisatech.MWorkforce.Job.dll"]