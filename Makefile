.PHONY: build-api
build-api:
	docker build -t mirana-api:latest -f Dockerfile.api .

.PHONY: build-admin
build-admin:
	docker build -t mirana-admin:latest -f Dockerfile.admin .

.PHONY: build-react
build-react:
	docker build -t mirana-react:latest -f Dockerfile.react .

.PHONY: add-mgn
add-mgn:
	dotnet ef migrations add Initialize -s ./src/miranaSolution.API/ -p ./src/miranaSolution.Data/

# DB:=Server=localhost;Database=MrnDataBase;User Id=sa;Password=myStrongPassword123;Encrypt=True;TrustServerCertificate=True
DB:=Server=rds;Database=MrnDataBase;User Id=admin;Password=admin123;Encrypt=True;TrustServerCertificate=True

.PHONY: update-db
update-db:
	dotnet ef database update -s ./src/miranaSolution.API/ -p ./src/miranaSolution.Data/ --connection "${DB}"