FROM mcr.microsoft.com/dotnet/sdk:5.0 as BUILD
WORKDIR /source

COPY *.sln .
COPY Directory.Build.props .

# Copy all the project files
# Base projects
COPY BLL.Base/*.csproj ./BLL.Base/
COPY Contracts.BLL.Base/*.csproj ./Contracts.BLL.Base/
COPY Contracts.DAL.Base/*.csproj ./Contracts.DAL.Base/
COPY Contracts.Domain.Base/*.csproj ./Contracts.Domain.Base/
COPY DAL.Base/*.csproj ./DAL.Base/
COPY DAL.Base.EF/*.csproj ./DAL.Base.EF/
COPY Domain.Base/*.csproj ./Domain.Base/
COPY Extensions.Base/*.csproj ./Extensions.Base/
COPY Helpers/*.csproj ./Helpers/
COPY Resources.Base/*.csproj ./Resources.Base/


#App projects
COPY BLL.App/*.csproj ./BLL.App/
COPY BLL.App.DTO/*.csproj ./BLL.App.DTO/
COPY Contracts.BLL.App/*.csproj ./Contracts.BLL.App/
COPY Contracts.DAL.App/*.csproj ./Contracts.DAL.App/
COPY DAL.App.DTO/*.csproj ./DAL.App.DTO/
COPY DAL.App.EF/*.csproj ./DAL.App.EF/
COPY Domain/*.csproj ./Domain/
COPY PublicApiDTO.v1/*.csproj ./PublicApiDTO.v1/
COPY Resources/*.csproj ./Resources/
COPY WebApp/*.csproj ./WebApp/

# Tests
COPY TestProject/TestProject.csproj ./TestProject/

#restore all the nuget packages
RUN dotnet restore
 
# copy over source code

# Copy all the project files
# Base projects
COPY BLL.Base/. ./BLL.Base/
COPY Contracts.BLL.Base/. ./Contracts.BLL.Base/
COPY Contracts.DAL.Base/. ./Contracts.DAL.Base/
COPY Contracts.Domain.Base/. ./Contracts.Domain.Base/
COPY DAL.Base/. ./DAL.Base/
COPY DAL.Base.EF/. ./DAL.Base.EF/
COPY Domain.Base/. ./Domain.Base/
COPY Extensions.Base/. ./Extensions.Base/
COPY Helpers/. ./Helpers/
COPY Resources.Base/. ./Resources.Base/

# App projects
COPY BLL.App/. ./BLL.App/
COPY BLL.App.DTO/. ./BLL.App.DTO/
COPY Contracts.BLL.App/. ./Contracts.BLL.App/
COPY Contracts.DAL.App/. ./Contracts.DAL.App/
COPY DAL.App.DTO/. ./DAL.App.DTO/
COPY DAL.App.EF/. ./DAL.App.EF/
COPY Domain/. ./Domain/
COPY PublicApiDTO.v1/. ./PublicApiDTO.v1/
COPY Resources/. ./Resources/
COPY WebApp/. ./WebApp/

# Tests
COPY TestProject/. ./TestProject

WORKDIR /source/WebApp

RUN dotnet publish -c Release -o out

RUN ls out -al

#create a new image from runtime and copy everything to app (no compilers)
FROM mcr.microsoft.com/dotnet/aspnet:5.0 as runtime



WORKDIR /app
COPY --from=BUILD /source/WebApp/out ./

ENTRYPOINT ["dotnet", "WebApp.dll"]


