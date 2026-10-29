# Individuell leverans – G-krav och testguide

Dokumentet beskriver den individuella versionen av Innovia Hub och vad som ska testas inför inlämningen. Lokal uppstart, tekniköversikt och lista över egna vidareutvecklingar finns i [README.md](README.md). Tidigare kontroller och automatiska tester dokumenteras i [Testning_och_kvalitet.md](Testning_och_kvalitet.md).

## Leverans och åtkomst

- Applikation: https://innovia-3-frontend-production.up.railway.app/
- API-basadress: https://innovia-3-production.up.railway.app/
- Källkod: https://github.com/C-friberg/Innoviahub-CF
- Separat IoT-projekt: https://github.com/C-friberg/Innovia-iot

Frontend, eget API och PostgreSQL körs som separata tjänster i Railway. Uppdateringar på `main` bygger och deployar frontend och API. API-hemligheter finns i serverns miljövariabler. Frontendens `VITE_API_URL` innehåller API:ts publika HTTPS-adress och sätts vid bygget. OpenAI-nyckeln ska aldrig läggas i frontend eller Git.

**Testkonto:** Admin@innoviahub.se, roll Admin, angivet av projektägaren. Lösenordet lämnas separat i inlämningen. Provlogga in inför leverans. Ett vanligt användarkonto räcker för bokning och AI. För administration och sensoröversikt behövs ett administratörskonto. Administratör kan registrera användare i applikationen.

## Hur systemet fungerar

### Bokningar och realtid i molnversionen

Frontend hämtar resurser och tillgängliga tider från vårt API. När en inloggad användare bokar kontrollerar backend tillgängligheten och sparar bokningen i PostgreSQL. Därefter skickar BookingHub händelsen `BookingsChanged`. Anslutna frontendklienter hämtar uppdaterat innehåll. På så sätt kan en annan användare se att tillgängligheten ändrats utan att ladda om sidan.

Kod: `backend/api/Controllers/BookingController.cs`, `backend/api/Hubs/BookingHub.cs`, `frontend/src/pages/LandingPage.tsx` och `frontend/src/components/ResourceStatus.tsx`.

### AI-bokning

Frontend skickar användarens text och JWT till `POST /api/ai/chat`. AIService anropar OpenAI Responses API och omvandlar önskemålet till resurstyp, datum och tider. AIBookingService söker en ledig resurs. Frontend visar förslaget och skapar bokningen först när användaren bekräftar det genom det vanliga boknings-API:t. AI:n skriver inte direkt till databasen.

AI-anrop kräver inloggning. Varje användare har fem försök per minut, även misslyckade försök räknas. Gränsen är en fast tidsperiod och lagras i minnet per API-instans; omstart återställer räknaren. Detta är inte ett hårt kostnadstak för OpenAI-kontot. Leverantörens begränsning ger 503 till frontend, medan vår egen anropsgräns ger 429 och `Retry-After`.

Kod: `backend/api/Controllers/AIController.cs`, `backend/api/Services/AIService.cs`, `backend/api/Services/AIBookingService.cs`, `backend/api/Services/AIRateLimit.cs` och `frontend/src/components/AIBooking.tsx`.

### IoT-flödet vid lokal körning

Enhetslistan hämtas av vår DeviceService från DeviceRegistry. Vårt API kompletterar listan med resurskopplingar från den egna sensortabellen. Frontend visar enheterna under rätt resurs på adminsidan. Kopplingen sker via enhetens GUID i `DeviceRegistryDeviceId`; DeviceRegistry behöver därför inte ha samma rumskoppling som vår plattform.

Mätvärden har en separat väg: Edge.Simulator skickar via MQTT till Ingest.Gateway, som sparar och publicerar vidare till Realtime.Hub. Frontend tar emot `measurementReceived` och matchar enhetens GUID för att uppdatera rätt kort. Den egna bokningshubben och IoT-hubben är två skilda anslutningar.

Kod i detta repo: `backend/api/Services/DeviceService.cs`, `backend/api/Controllers/DeviceController.cs`, `backend/api/Models/Sensor.cs`, `backend/api/Data/IoTSensorSeed.cs`, migrationen `LinkSensorsToResources` och `frontend/src/components/DeviceOverview.tsx`.

## Testa den deployade applikationen

Använd framtida datum och testbokningar som kan tas bort efteråt. Ett teststeg nedan är en instruktion, inte ett påstående att hela scenariot redan är manuellt verifierat efter senaste deploy.

| Test | Gör så här | Förväntat resultat |
|---|---|---|
| Inloggning | Logga in med det separat bifogade testkontot. | Användaren får tillgång till sina bokningar och AI-assistenten. |
| Manuell bokning | Välj resurstyp, framtida datum och ledig start-/sluttid. Genomför bokningen. | Bekräftelse visas och bokningen finns under Bokningar. |
| Avbokning | Ta bort din egen testbokning. | Bokningen försvinner och tiden blir tillgänglig igen. |
| Realtid | Öppna samma resurs/datum i två fönster. Boka eller avboka i det ena. | Det andra fönstrets tillgänglighet ändras utan manuell omladdning. |
| AI | Ange ett tydligt framtida datum, resurstyp och tider, exempelvis ”Jag vill boka ett mötesrum den 15 december 2026 klockan 10 till 11”. Anpassa datumet vid senare test. | AI visar ett bokningsförslag om en resurs finns ledig. Ingenting bokas före bekräftelse. |
| Bekräfta AI-förslag | Bekräfta förslaget och kontrollera bokningslistan. | En bokning skapas genom samma API som vid manuell bokning. |
| AI utan inloggning | Logga ut och försök använda AI-assistenten. | Meddelandet ”Logga in för att använda AI-assistenten.” visas. Ett direkt API-anrop utan token ger 401. |
| AI-gräns, valfri | Gör fler än fem AI-anrop från samma konto inom en minut. Observera att riktiga anrop kan kosta pengar. | Sjätte försöket avvisas med ett begripligt meddelande; API:t ger 429. Detta är också automatiskt testat med mockad AI. |
| IoT offline | Med IoT-servern avstängd, testa bokning, AI och bokningsrealtid. Öppna sedan sensoröversikten med administratör. | Kärnfunktionerna ska fungera. Sensorvyn får inte krascha resten av appen. Tydligare samlat offlinebesked återstår att implementera och verifiera. |

## Status mot G-kraven

| Krav | Underlag och aktuell status |
|---|---|
| Publicerad molnapplikation | Frontend och API är publicerade i Railway. Länkar finns ovan. |
| Fungerande AI | Användaren bekräftade fungerande AI med egen nyckel i molnversionen. Efterföljande deploy lade till inloggningskrav och anropsgräns. Ett nytt komplett AI-förslag och bekräftad bokning med testkontot bör ingå i slutkontrollen. |
| Bokningsflöde | Implementerat i eget API och frontend. Genomför manuell bokning och avbokning med inlämningens testkonto inför leverans. |
| Realtidskommunikation | Egen SignalR-hubb för bokningar finns i molnversionen. Kontrollera tvåfönstertestet ovan inför leverans; sensordata krävs inte för detta test. |
| Plattform fungerar med IoT offline | IoT-servern är inte driftsatt. Sensorvyn fångar hämtningsfel och visar anslutningsstatus, men samlat offlinebesked och slutlig verifiering av molnbeteendet återstår. |
| README med systembeskrivning, teknik, lokal start och egna funktioner | Beskrivs i rotens README. Denna fil kompletterar med testguide och kravstatus. |
| Tydliga testinstruktioner och åtkomst | Teststeg finns här. Testkontots e-post och roll anges ovan; lösenordet bifogas separat. Provlogga in före leverans. |

## Verifiering och kvarvarande inför inlämning

Den 28 oktober 2026 godkändes 16 backendtester, frontendens produktionsbygge och riktad ESLint-kontroll. HTTP-testerna använder isolerad server, signerade testtoken och mockad AI; de belastar inte OpenAI. De täcker bland annat 401 utan giltig token, fem tillåtna försök följt av 429, separata räknare per användare och CORS på felsvar.

Commit `28414ca` med AI-skyddet pushades till `main` och deployades. Moln-API:t verifierades returnera 401 utan inloggning och frontend visade rätt inloggningsmeddelande. Detta verifierar inte i sig hela bokningsflödet eller tvåfönstertestet.

Före inlämning återstår att:

1. Provlogga in med granskningens testkonto, och lämna lösenordet separat.
2. Implementera och verifiera tydligt offlinebesked i sensorvyn.
3. Genomföra bokning, avbokning, AI-bokning och SignalR-test i två fönster i den slutliga molnversionen.

Dokumentationen beskriver nuläget. Samtliga G-krav ska inte markeras som färdiga förrän de kvarvarande punkterna är genomförda.
