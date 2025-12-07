FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /source

COPY *.sln .
COPY iva-grp7-backend/*.csproj ./iva-grp7-backend/
RUN dotnet restore

COPY iva-grp7-backend/. ./iva-grp7-backend/
WORKDIR /source/iva-grp7-backend
RUN dotnet publish -c release -o /app --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:7.0
WORKDIR /app
COPY --from=build /app ./
CMD ["dotnet","iva-grp7-backend.dll"]
