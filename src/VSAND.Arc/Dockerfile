FROM ${registry:-registry.applicationx.net}/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM ${registry:-registry.applicationx.net}/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/VSAND.Arc/VSAND.Arc.csproj", "src/VSAND.Arc/"]
COPY ["src/VSAND.Data/VSAND.Data.csproj", "src/VSAND.Data/"]
COPY ["src/VSAND.Common/VSAND.Common.csproj", "src/VSAND.Common/"]
RUN dotnet restore "src/VSAND.Arc/VSAND.Arc.csproj"
COPY . .
WORKDIR "/src/src/VSAND.Arc"
RUN dotnet build "VSAND.Arc.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VSAND.Arc.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VSAND.Arc.dll"]