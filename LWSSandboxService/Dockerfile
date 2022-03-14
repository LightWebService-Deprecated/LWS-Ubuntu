FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["LWSSandboxService/LWSSandboxService.csproj", "LWSSandboxService/"]
RUN dotnet restore "LWSSandboxService/LWSSandboxService.csproj"
COPY . .
WORKDIR "/src/LWSSandboxService"
RUN dotnet build "LWSSandboxService.csproj" -c Release -o /app/build
RUN dotnet publish "LWSSandboxService.csproj" -c Release -o /app/publish
WORKDIR /app/publish
ENTRYPOINT ["dotnet", "LWSSandboxService.dll"]