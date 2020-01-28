FROM microsoft/aspnetcore:2.2 as build

ARG BUILDCONFIG=RELEASE
ARG VERSION=1.0.0


COPY  Automation.Service.csproj /build/

RUN dotnet restore ./build/Automation.Service.csproj

COPY . ./build/
WORKDIR /build/
RUN dotnet publish ./Automation.Service.csproj -c $BUILDCONFIG -o out /p:Version=$VERSION

FROM microsoft/aspnetcore:2.2-aspnetcore-runtime
WORKDIR /app

COPY --from=build /build/out .

ENTRYPOINT ["dotnet", "Automation.Service.dll"]
