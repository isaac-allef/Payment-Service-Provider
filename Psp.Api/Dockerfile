FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

COPY ["Psp.Api/*.csproj", "Psp.Api/"]
COPY ["Psp.Shared.Domain/*.csproj", "Psp.Shared.Domain/"]
COPY ["Psp.Shared.Models/*.csproj", "Psp.Shared.Models/"]
COPY ["Psp.Shared.Services/*.csproj", "Psp.Shared.Services/"]

COPY . ./
RUN dotnet restore "Psp.Api/Psp.Api.csproj"
RUN dotnet publish "Psp.Api/Psp.Api.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Psp.Api.dll"]