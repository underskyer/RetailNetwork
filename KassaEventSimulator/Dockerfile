FROM mcr.microsoft/dotnet/aspnet/2.0 AS app  
WORKDIR /app  
COPY . .
RUN dotnet restore  
EXPOSE 8001  
ENTRYPOINT ["dotnet", "KassaEventSimulator"]