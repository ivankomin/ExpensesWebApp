# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj and restore
COPY ["ExpensesWebApp/ExpensesWebApp.csproj", "ExpensesWebApp/"]
RUN dotnet restore "ExpensesWebApp/ExpensesWebApp.csproj"

# Copy the full project and build
COPY . .
WORKDIR "/src/ExpensesWebApp"
RUN dotnet publish -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "ExpensesWebApp.dll"]
