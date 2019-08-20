FROM ${registry:-registry.applicationx.net}/dotnet/core/sdk:2.2-alpine3.9 AS build
WORKDIR /src
COPY ["src/VSAND.Test/VSAND.Test.csproj", "src/VSAND.Test/"]
COPY ["src/VSAND.Common/VSAND.Common.csproj", "src/VSAND.Common/"]
COPY ["src/VSAND.Data/VSAND.Data.csproj", "src/VSAND.Data/"]
COPY ["src/VSAND.Services/VSAND.Services.csproj", "src/VSAND.Services/"]
RUN dotnet restore "src/VSAND.Test/VSAND.Test.csproj"
COPY . .

FROM build AS test
WORKDIR "/src/src/VSAND.Test"
RUN dotnet test -p:CollectCoverage=true
