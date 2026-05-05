FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ShoppingCart.Domain/ShoppingCart.Domain.csproj ShoppingCart.Domain/
COPY ShoppingCart.Application/ShoppingCart.Application.csproj ShoppingCart.Application/
COPY ShoppingCart.Infrastructure/ShoppingCart.Infrastructure.csproj ShoppingCart.Infrastructure/
COPY ShoppingCart.Api/ShoppingCart.Api.csproj ShoppingCart.Api/

RUN dotnet restore ShoppingCart.Api/ShoppingCart.Api.csproj

COPY . .
RUN dotnet publish ShoppingCart.Api/ShoppingCart.Api.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Development
ENV ConnectionStrings__ShoppingCart="Data Source=/app/Data/shopping-cart.db"

COPY --from=build /app/publish .

RUN mkdir -p /app/Data

EXPOSE 8080

ENTRYPOINT ["dotnet", "ShoppingCart.Api.dll"]
