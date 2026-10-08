FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy ไฟล์ csproj และ restore dependencies
COPY ["RecipeApp.csproj", "./"]
RUN dotnet restore "RecipeApp.csproj"

# Copy โค้ดทั้งหมดและ Build
COPY . .
RUN dotnet publish "RecipeApp.csproj" -c Release -o /app/publish

# Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# กำหนด Port สำหรับ Render
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "RecipeApp.dll"]