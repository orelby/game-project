FROM microsoft/dotnet:sdk AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy the rest and built .NET
COPY . ./
RUN dotnet publish -c Release -o out

########## Build Angular ##########
######### Moved to .csproj ########
# # Remove the Angular folder for rebuild
# RUN rm -rf ./ClientApp

# FROM node:alpine
# WORKDIR ./ClientApp

# # Copy package*.json and install npm deps
# COPY ./ClientApp/package*.json ./
# RUN npm install
# ENV NPM_PATH="./node_modules/.bin"

# # Copy the rest of the Angular folder and build
# COPY ./ClientApp ./
# RUN ${NPM_PATH}/ng build --prod --aot

# Build runtime image
FROM microsoft/dotnet:aspnetcore-runtime
WORKDIR /app
COPY --from=build-env /app/out .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet GameProject.dll