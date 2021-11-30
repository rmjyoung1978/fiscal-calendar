SHELL := /bin/bash
#.SILENT:

ifeq "$(BUILDKITE)" "true"
$(info Running with BuildKite)
    BUILD = $(BUILD_NUMBER)
    BRANCH = $(GIT_BRANCH)
else
$(info Running locally - no BuildKite)
    BUILD ="0.0.0"
    BRANCH = "local"
endif

info "Build Number: $(BUILD_NUMBER)"
info "Git Branch: $(BRANCH)"

# build vars, default if not set
IO_COMPOSE = -f docker-compose-io.yml

# terraform configuration
TF_ARTIFACT = $(ENV).$(AWS_DEFAULT_REGION).plan
TF_VARS := -var 'region=$(AWS_DEFAULT_REGION)' \
           -var 'environment=$(ENV)' \
		   -var-file=config/$(REGION)/$(ENV).$(AWS_DEFAULT_REGION).tfvars \

#export TF_LOG = TRACE

# docker-compose calls
RUN_OKTA = @docker-compose run --rm authenicate
RUN_ALPINE = @docker-compose $(IO_COMPOSE) run --rm alpine
RUN_TF = docker-compose run --rm terraform

.PHONY: auth
auth: .env
	@echo "Authenticating with Okta..."
	$(RUN_OKTA) awscli sts get-caller-identity

.PHONY: test
test: .env
	$(RUN_TF) init -input=false -backend=false
	$(RUN_TF) validate 

.PHONY: build
build: .env _init
	$(RUN_TF) refresh $(TF_VARS)
	$(RUN_TF) plan $(TF_VARS) -out $(TF_ARTIFACT)

.PHONY: deploy
deploy: .env _init
	$(RUN_TF) apply $(TF_ARTIFACT)

.PHONY: delete
delete: .env _init 
	$(RUN_TF) destroy -auto-approve $(TF_VARS) 

# Internal Targets
.PHONY: .env
.env: 
	@echo "No .env file found. Create new .env using .env.template"
	$(RUN_ALPINE) cp .env.template .env

.PHONY: shellTF
shellTF: .env
	docker-compose run --entrypoint bash terraform

.PHONY: _init
_init:
	$(RUN_TF) init -backend-config=config/$(REGION)/$(ENV).$(AWS_DEFAULT_REGION).backend
