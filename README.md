# Innovia Hub

Innovia Hub är ett bokningssystem för resurser som skrivbord, mötesrum, VR-headset och AI-server. Projektet har en frontend och ett ASP.NET Core Web API med PostgreSQL. Systemet innehåller även en AI-assistent som tolkar bokningsönskemål med OpenAI och föreslår tillgängliga resurser.

Applikationen körs med **Docker Compose**, vilket innebär att frontend, backend och databas kan startas tillsammans.

## 1. Klona projektet

Öppna en terminal och kör:

```bash
git clone https://github.com/C-friberg/Innoviahub-CF.git
cd innoviahub-cf
git switch main
```

## 2. Konfigurera miljövariabler

I projektets rot finns filen `.env.example`. Skapa en kopia av den och döp kopian till `.env`.

**Windows (PowerShell):**

```powershell
Copy-Item .env.example .env
```

**macOS/Linux:**

```bash
cp .env.example .env
```

Öppna `.env` och fyll i värdena:

```dotenv
JWT_KEY=replace-with-jwt-key
POSTGRES_DB=innoviahub
POSTGRES_USER=postgres
POSTGRES_PASSWORD=replace-with-postgres-password
OPENAI_API_KEY=replace-with-valid-OPENAI_API_KEY
```

| Variabel            | Beskrivning                                                                   |
| ------------------- | ----------------------------------------------------------------------------- |
| `JWT_KEY`           | Hemlig nyckel för signering av JWT-token. Använd en lång, slumpmässig nyckel. |
| `POSTGRES_DB`       | Namnet på PostgreSQL-databasen.                                               |
| `POSTGRES_USER`     | Användarnamn för PostgreSQL.                                                  |
| `POSTGRES_PASSWORD` | Lösenord för PostgreSQL.                                                      |
| `OPENAI_API_KEY`    | Giltig OpenAI API-nyckel för AI-assistenten.                                  |

## 3. Starta projektet med Docker

Kontrollera att Docker Desktop körs. Öppna sedan en terminal i projektets rot, där `docker-compose.yml` ligger, och kör:

```bash
docker compose up --build -d
```

Docker bygger de images som behövs och startar tjänsterna i bakgrunden. Första starten kan ta några minuter.

Kontrollera att containrarna körs:

```bash
docker compose ps
```

## 4. Öppna applikationen

Med den tidigare använda portkonfigurationen nås tjänsterna här:

| Tjänst      | Adress                |
| ----------- | --------------------- |
| Frontend    | http://localhost:3000 |
| Backend API | http://localhost:5197 |

**Obs:** Kontrollera portarna under `ports:` i `docker-compose.yml` om adresserna ovan inte fungerar. Exponerade portar kan ha ändrats.

För att använda AI-assistenten behöver backend ha tillgång till en giltig `OPENAI_API_KEY`.

## 5. Stoppa eller starta om projektet

**Stoppa och ta bort containrarna:**

```bash
docker compose down
```

**Starta igen:**

```bash
docker compose up -d
```

**Bygg om efter kodändringar:**

```bash
docker compose up --build -d
```

**Varning:** Kör inte `docker compose down -v` om du vill behålla databasens data. Flaggan `-v` tar även bort Compose-volymer.

## 6. Uppdatera projektet

Om nya ändringar har lagts till på `main`:

```bash
git switch main
git pull origin main
docker compose up --build -d
```

## 7. Enhetstester (valfritt)

Projektet innehåller enhetstester för AI-bokningslogiken med **xUnit** och **Moq**. Om du har .NET SDK installerat kan testerna köras från testprojektets mapp med:

```bash
dotnet test
```
