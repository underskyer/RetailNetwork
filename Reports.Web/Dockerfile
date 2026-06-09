FROM mcr.microsoft/dotnet/aspnet/3.1 AS app  
WORKDIR /app  
COPY .   .
RUN dotnet restore  

EXPOSE 8001  

ENTRYPOINT ["dotnet", "Reports.Web"]