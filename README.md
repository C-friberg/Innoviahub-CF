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

Med aktuell Compose-konfiguration nås tjänsterna här:

| Tjänst      | Adress                |
| ----------- | --------------------- |
| Frontend    | http://localhost:3000 |
| Backend API | http://localhost:5197 |

**Obs:** Kontrollera portarna under `ports:` i `docker-compose.yml` om adresserna ovan inte fungerar. Exponerade portar kan ha ändrats.

Backend kräver miljövariabeln `OPENAI_API_KEY` redan vid uppstart. En giltig nyckel behövs för att använda AI-assistenten.

## IoT: starta hela lösningen

Sensoröversikten kräver även det separata projektet **innovia-iot** och **.NET 8 SDK**. Docker Compose i detta repo startar eget API, frontend och egen databas. IoT-projektets Compose startar endast IoT-databasen, Mosquitto och Redis; IoT-tjänsterna och simulatorn körs separat med `dotnet run`.

Använd en gren/version som innehåller IoT-integrationen i båda projekten. Kommandona nedan utgår från befintlig demodata. Byt sökväg i `cd` till respektive projekt på din dator.

### 1. Starta IoT-infrastrukturen

Starta Docker Desktop. Kör från **innovia-iot**:

```powershell
cd <sökväg-till-innovia-iot>
docker compose -f deploy/docker-compose.yml up -d
docker compose -f deploy/docker-compose.yml ps
```

### 2. Starta IoT-tjänsterna i ordning

Kör varje kommando i **en egen terminal från innovia-iot**. Låt tjänsten starta klart innan nästa startas. Behåll terminalerna öppna.

```powershell
# Terminal 1: enhetsregistret
dotnet run --project src/DeviceRegistry.Api/DeviceRegistry.Api.csproj
```

```powershell
# Terminal 2: realtidshubben måste köras före Ingest
dotnet run --project src/Realtime.Hub/Realtime.Hub.csproj
```

```powershell
# Terminal 3: mottagning, lagring och vidarepublicering
dotnet run --project src/Ingest.Gateway/Ingest.Gateway.csproj
```

```powershell
# Terminal 4: publicerar mockdata för dev-101 till dev-113
dotnet run --project src/Edge.Simulator/Edge.Simulator.csproj
```

Portal.Adapter behövs inte för vår sensoröversikt. Om du vill använda dess endpoints för lagrade mätningar startas den separat:

```powershell
dotnet run --project src/Portal.Adapter/Portal.Adapter.csproj
```

Rules.Engine behövs inte för demonstrationen.

### 3. Starta vår egen portal och vårt API

Kör från **innoviahub-cf**, med `.env` konfigurerad enligt ovan:

```powershell
cd <sökväg-till-innoviahub-cf>
docker compose up -d --build
docker compose ps
```

API:t applicerar EF-migrationer och skapar demo-resurskopplingarna vid uppstart. Kör inte även `dotnet run` för vårt API på samma port. Swagger i vårt API är avstängt i Production, vilket är miljön som Compose använder.

### 4. Kontrollera resultatet

Öppna http://localhost:3000, logga in med ett administratörskonto och välj **Admin-vy → Sensoröversikt**. Där ska 13 enheter visas under mötesrum 20–23 och AIServer 30. Vänta ungefär tio sekunder på nya mätvärden. Översikten hämtar inte historiska värden vid sidladdning.

Kontrollera enhetslistan via vårt API från PowerShell:

```powershell
Invoke-RestMethod http://localhost:5197/api/devices |
    Format-Table serial, model, resourceId, resourceType
```

DeviceRegistry Swagger finns på http://localhost:5101/swagger. Ingest Swagger finns på http://localhost:5102/swagger. För att kontrollera lagring används `GET /ingest/debug/device/{deviceId}` med **enhetens GUID**, inte serienumret eller tenantens GUID.

### Konfiguration och portar

| Del | Adress eller port |
|---|---|
| Frontend | http://localhost:3000 |
| Eget API | http://localhost:5197 |
| Egen PostgreSQL från datorn | localhost:5433 |
| Egen PostgreSQL från API-containern | postgres:5432 |
| DeviceRegistry.Api | http://localhost:5101 |
| Ingest.Gateway | http://localhost:5102 |
| Realtime.Hub | http://localhost:5103/hub/telemetry |
| Portal.Adapter, valfri | http://localhost:5104 |
| IoT PostgreSQL från datorn | localhost:5432 |
| MQTT/Mosquitto | localhost:1883 |

I egen `docker-compose.yml` skickas följande inställningar till API-containern:

```yaml
DeviceRegistry__BaseUrl: http://host.docker.internal:5101/
DeviceRegistry__TenantId: 3fa85f64-5717-4562-b3fc-2c963f66afa6
```

`host.docker.internal` låter API-containern nå DeviceRegistry på datorn i vår Docker Desktop-miljö. Vid annan Docker-miljö behöver åtkomsten till värddatorn konfigureras. `localhost` inne i containern pekar på containern själv.

`appsettings.Development.json` anger i stället `http://localhost:5101/` för direkt lokal körning av eget API. Den filen används inte i Compose-miljön Production. Vid direkt körning måste även egen databasanslutning anpassas till datorns port 5433 och nödvändiga miljövariabler anges; `dotnet run` läser inte automatiskt `.env`.

Frontend ansluter direkt till hubben på localhost:5103 och tenantgruppen `innovia`. Adresserna är avsedda för demo på samma dator. Portalen på en annan dator eller en publik driftsättning kräver andra adresser och motsvarande HTTPS/CORS-konfiguration.

### Demodata på en ny installation

IoT-databasen måste innehålla tenant `innovia` och enheterna `dev-101`–`dev-113`; de registreras inte av simulatorn eller av vårt Compose-projekt. Använd DeviceRegistry API för registrering och verifiera tenantens GUID mot Compose-inställningen.

`backend/api/Data/IoTSensorSeed.cs` innehåller GUID-kopplingar för vårt befintliga demoregister. Om enheterna skapas på nytt med andra GUID:er behöver seedningen och en ny EF-migration anpassas för att resurskopplingarna ska matcha. Ändra inte en redan applicerad migration som enda åtgärd.

### Vanliga startproblem

- **Ingen realtidsanslutning:** kontrollera att Realtime.Hub körs på 5103 och att CORS tillåter frontend på localhost:3000. Starta hubben före Ingest.
- **Ansluten men inga värden:** kontrollera simulatorns och Ingests terminaler samt att MQTT är igång och rätt serienummer finns i registret.
- **GET /api/devices ger 502:** kontrollera DeviceRegistry och API-containerns DeviceRegistry-adress. Läs `docker compose logs --tail 30 api`.
- **Port 5432 upptagen:** IoT-databasen använder 5432. Vår egen databas ska publiceras på 5433, som i Compose-filen.
- **OPENAI_API_KEY IS MISSING:** kontrollera `.env` och att nyckeln skickas till API-containern. Nuvarande API kräver variabeln redan vid uppstart, även för sensorfunktionerna.
- **Kodändringar syns inte:** bygg om med `docker compose up -d --build`. IoT-processer behöver startas om efter ändringar i deras kod.

Stoppa .NET-processerna med `Ctrl+C` i deras terminaler. Stoppa respektive projekts containrar med `docker compose stop` eller, från innovia-iot, `docker compose -f deploy/docker-compose.yml stop`. Dessa kommandon tar inte bort datavolymer.


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
