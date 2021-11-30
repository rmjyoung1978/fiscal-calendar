IO_COMPOSE = -f docker-compose-io.yml

# store the UID and GID of the current user, so file permissions are kept intact
USER = $(shell id -u):$(shell id -g)
#
RUN_DOTNET = @docker-compose run --rm dotnet
RUN_UTIL = @docker-compose $(IO_COMPOSE) run --rm utils
RUN_ALPINE = @docker-compose $(IO_COMPOSE) run --rm alpine

clean: .env
	$(RUN_UTIL) rm -rf artifacts
	$(RUN_UTIL) rm -rf FiscalCalendar

build: .env
	$(RUN_DOTNET) make _buildLambda

shellBOTO: .env
	docker-compose run --entrypoint bash boto

.env:
	@echo "No .env file found. Create new .env using .env.template"
	$(RUN_ALPINE) cp .env.template .env

_buildLambda:
	dotnet restore ./FiscalCalendar.Api/FiscalCalendar.Api.csproj -s https://api.nuget.org/v3/index.json
	dotnet lambda package -pl ./FiscalCalendar.Api/ --configuration release --framework netcoreapp3.1 --output-package ./artifacts/deploy-api-package.zip
