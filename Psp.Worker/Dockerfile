FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app
EXPOSE 80

COPY ["Psp.Worker/*.csproj", "Psp.Worker/"]
COPY ["Psp.Shared.Domain/*.csproj", "Psp.Shared.Domain/"]
COPY ["Psp.Shared.Models/*.csproj", "Psp.Shared.Models/"]
COPY ["Psp.Shared.Services/*.csproj", "Psp.Shared.Services/"]

COPY . ./
RUN dotnet restore "Psp.Worker/Psp.Worker.csproj"
RUN dotnet publish "Psp.Worker/Psp.Worker.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "Psp.Worker.dll"]