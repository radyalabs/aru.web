#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Trisatech.MWorkforce.Api/Trisatech.MWorkforce.Api.csproj", "Trisatech.MWorkforce.Api/"]
COPY ["Trisatech.AspNet.Common/Trisatech.AspNet.Common.csproj", "Trisatech.AspNet.Common/"]
COPY ["Trisatech.MWorkforce.Business/Trisatech.MWorkforce.Business.csproj", "Trisatech.MWorkforce.Business/"]
COPY ["Trisatech.MWorkforce.Domain/Trisatech.MWorkforce.Domain.csproj", "Trisatech.MWorkforce.Domain/"]
COPY ["Trisatech.MWorkforce.Infrastructure/Trisatech.MWorkforce.Infrastructure.csproj", "Trisatech.MWorkforce.Infrastructure/"]
COPY ["Trisatech.MWorkforce.Persistence.PostgreSQL/Trisatech.MWorkforce.Persistence.PostgreSQL.csproj", "Trisatech.MWorkforce.Persistence.PostgreSQL/"]
RUN dotnet restore "Trisatech.MWorkforce.Api/Trisatech.MWorkforce.Api.csproj"
COPY . .
WORKDIR "/src/Trisatech.MWorkforce.Api"
RUN dotnet build "Trisatech.MWorkforce.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Trisatech.MWorkforce.Api.csproj" -c Release -o /app/publish

FROM base AS final

# Install the agent
RUN apt-get update && apt-get install -y wget ca-certificates gnupg \
&& echo 'deb http://apt.newrelic.com/debian/ newrelic non-free' | tee /etc/apt/sources.list.d/newrelic.list \
&& wget https://download.newrelic.com/548C16BF.gpg \
&& apt-key add 548C16BF.gpg \
&& apt-get update \
&& apt-get install -y newrelic-netcore20-agent \
&& rm -rf /var/lib/apt/lists/*

# Enable the agent
ENV CORECLR_ENABLE_PROFILING=1 \
CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
CORECLR_NEWRELIC_HOME=/usr/local/newrelic-netcore20-agent \
CORECLR_PROFILER_PATH=/usr/local/newrelic-netcore20-agent/libNewRelicProfiler.so \
NEW_RELIC_LICENSE_KEY={KEY} \
NEW_RELIC_APP_NAME=mworkforce-api

WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Trisatech.MWorkforce.Api.dll"]