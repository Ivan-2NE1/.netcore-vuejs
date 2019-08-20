FROM ${registry:-registry.applicationx.net}/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM ${registry:-registry.applicationx.net}/vsand-sdk AS build
WORKDIR /src
COPY ["src/VSAND.Frontend/VSAND.Frontend.csproj", "src/VSAND.Frontend/"]
COPY ["src/VSAND.Services/VSAND.Services.csproj", "src/VSAND.Services/"]
COPY ["src/NJAM/NJAM.csproj", "src/NJAM/"]
COPY ["src/VSAND.Data/VSAND.Data.csproj", "src/VSAND.Data/"]
COPY ["src/VSAND.Common/VSAND.Common.csproj", "src/VSAND.Common/"]
COPY ["src/VSAND.Interfaces/VSAND.Interfaces.csproj", "src/VSAND.Interfaces/"]
RUN dotnet restore "src/VSAND.Frontend/VSAND.Frontend.csproj"
COPY . .
WORKDIR "/src/src/VSAND.Frontend"

# run npm install and gulp build here
RUN npm install

RUN gulp build
RUN dotnet build "VSAND.Frontend.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "VSAND.Frontend.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "VSAND.Frontend.dll"]