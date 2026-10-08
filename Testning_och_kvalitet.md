# Mina Tester

## Sammanfattning :

För att säkerställa att vår ai bokning fungerar som förväntat så har vi skrivit 7 enhetstester med xUnit och Moq.

Vi använder Moq för att simulera olika delar av systemet, t.ex service och endpoints. Vi kan testa bokningslogiken utan att behöva ansluta till postgres, eller göra riktiga anrop till OpenAI.

Enhetstesterna är bra för att upptäcka fel tidigt och säkerställa att funktioner som tidigare har fungerat fortfarande fungerar vid framtida implementeringar.

---

## Test 1–5 (AIBookingServiceTests)

### Sammanfattning:

Dessa tester är framförallt viktiga eftersom att vår AI kan tolka en användare fel och misstolka viktig information.

Vi vill därför säkerställa att vår backend kontrollerar informationen innan den används för att söka en ledig resurs.

I dessa tester så testar vi även ifall obligatoriska värden finns, att resurs typen är gilltig och att rätt information skickas vidare till AvailabilityService.

Vi använder Moq för att simulera vår AvailabilityService.

### Test 1 Bokning som saknar sluttid.

Om en bokning saknar en sluttid så säkerställer vi att en bokning inte genomförs och att vårat test returnerar null.

Eftersom vår AI kan missa information så säkerställer vi med det här testet att vår backend inte fortsätter med en ofullständig förfrågan.

I testet skickar vi vår bookingIntentdto där endTime är null .

Vi förväntar oss att FindAvailableResourseAsync() returnerar null vilket gör att inget bokningsförslag med tillgänglig resurs visas.

### Test 2 Om resurs typ är ogiltig.

I test 2 säkerställer vi att ResourceType är giltig, i vårt test skickar vi in värdet "Casper" som resurs typ.

Eftersom "Casper" inte finns i vår ResourceType Enum ska kontrollen med Enum.TryParse misslyckas.

Vi förväntar oss att metoden ska returnera null.

Detta testet finns eftersom AI skulle kunna returnera en resurstyp som inte finns i vårat bokningssystem.

### Test 3 Om ledig resurs finns

I Test 3 så testar vi vad som händer när en giltig bokningsförfrågan skickas och en ledig resurs finns.

Vi skapar ett BookingIntentDto där användaren vill boka ett vr headset mellan 9-12 .

Med Moq simulerar vi att AvailabilityService hittar en ledig resurs med id 40.

vi förväntar oss att resultatet inte är null och att vi får resursen med id 40.

Testet säkerställer att AIBookingService returnerar rätt resurs när Availability service hittar en.

### Test 4 Ingen ledig resurs finns tillgänglig

I test 4 kontrollerar vi vad som händer ifall det inte finns någon ledig resurs tillgänglig vid angiven tid.

Vi använder moq för att simulera att AvailabilityService inte hittar någon ledig resurs vid önskad tid och att den returnerar null.

Vi förväntar oss även att AIBookingService returnerar null

Detta test är viktigt eftersom vi inte vill visa bokningsförslag om resursen inte är tillgänglig. Vi testar bara hur AIBookingService hanterar svaret från AvailabilityService inte själva sökningen i databasen.

### Test 5 Kontroll av datum och tider

I Test 5 säkerställer vi att rätt information skickas från AIBookingservice till availabilityService.

Vi skapar en bokningsförfrågan mellan 9 och 12 de n 9 Oktober 2026.

Med Moq och Verify kontrollerar vi att GetFirstAvailableAsync anropas en gång med rätt resurstyp, start- och sluttid.

Detta är viktigt eftersom att vår AI tar emot datum och tider separat somDateOnly och TimeONly medan vår AvailabilityService tar emot DateTime.

Testet säkerställer att dessa värden kombineras rätt.

---

## Test 6-7 (AvailabilityServiceTests)

### Sammanfattning

I dessa tester fokuserar vi på vår AvailabilityService som ansvarar för att hitta en ledig resurs från användarens önskemål.

I dessa testerna använder vi Moq för att simulera våra repositories som Resource- och BookingRepository.

Dessa testerna är viktiga för att säkerställa att att systemet kan hantera både lediga och upptagna resurser.

### Test 6: Hitta den första lediga resursen

I test 6 testar vi metoden GetFirstAvailableAsync(), som ska returnera den första lediga resursen.

I vårt fall skapar vi två VR-headset med ID 40 och 41.

Med hjälp av Moq simulerar vi att resurs 40 är upptagen genom att låta IsResourceAvailableAsync() returnera false.

För resurs 41 returnerar samma metod true, vilket innebär att den är ledig.

Vi förväntar oss att vår AvailabilityService hoppar över resurs 40 och returnerar resurs 41.

Vi kontrollerar därför att resultatet inte är null och att ResourceId är 41.

Testet är viktigt eftersom systemet ska kunna fortsätta söka efter en ledig resurs även om den första är upptagen.

### Test 7: I det sista testet så testar vi vad som händer om alla resurser är bokade vid en specifik tid.

I det sista testet kontrollerar vi vad som händer om alla resurser av en viss typ är upptagna under den önskade tiden.

Precis som i föregående test skapar vi två VR-headset med ID 40 och 41.

Den här gången använder vi Moq för att simulera att båda resurserna är upptagna.

Vi förväntar oss att AvailabilityService kontrollerar båda resurserna och sedan returnerar null, eftersom ingen resurs är tillgänglig.

Med hjälp av Verify() kontrollerar vi också att båda resurserna faktiskt har kontrollerats.

Detta är viktigt eftersom systemet inte ska föreslå en resurs som redan är bokad.

---

 Du ska även reflektera över hur du arbetat för att projektet skall enkelt kunna vidareutvecklas med nya funktioner
 ### Framtids säkring

 Dett projektet började som ett grupparbete. Tidigt i utvecklingen kom vi fram till att skriva våra endpoints med hjälp av Dto'er. Detta har underlättat väldigt mycket under utvecklingen eftersom vi hela tiden har kontroll vilken information som visas, vilken information som skickas osv. Behövs det någon ändring så räcker det att ändra i DTO istället för att ändra entiteter och göra en ny migrering till databas. 

 Vi har även valt att separera våran backend i controllers, services, repositories och som sagt DTO'er. 

 Exempel på det är att AIBookingService använder IAvailabilityService istället för att vara beroende av klassen AvailabilityService. Det gör att vi enklare kan byta ut eller utveckla vår kod utan att behöva ändra all kod som använder den.
 Det gjorde även att vi kunde använda Moq i våra tester för att simulera våra beroenden. 

 Skulle jag vidareutveckla något i den här applikationen så skulle det vara att AI'n ställde följdfrågor för att göra bokningen enklare, just nu om den saknar t.ex sluttid så kommer den inte att returnera något, hade jag gjort en förbättring så hade det varit att den då frågade användaren efter en sluttid så att den fortfarande kan skapa ett bokningsförslag till användaren. 

 Snart kommer ju IoT implementeringen, och med den kommer vi att få SensorAPI. Tidigt i arbetet tog vi hänsyn till att det kommer att dyka upp i framtiden, så vi har förberett en Sensor klass med en Enum för sensor type. Våra resurser i sin tur har en ICollection av sensorer, vi skapade alltså ett 1-to-many förhållande mellan resurs och sensorer. 

### Säkerhet 

Eftersom vi använder OpenAI API så måste vi hantera vår api nyckel på ett säkert sätt. I den här applikationen så ligger api-nyckeln i en .env fil istället för att t.ex lägga den i program.cs. Filen är med i .gitignore så att den inte läggs upp på github av misstag. 

Eftersom att applikationen körs i Docker så skickas nyckeln till våran backend genom en miljövariabel. 

När applikationen körs i produktionsmiljö bör vi använda driftplattformens funktioner för att säkert hantera våra hemligheter istället för att förlita oss på en lokal env fil. 

Säkerhet kring AI: 

Vi låter inte våran AI skapa bokningar direkt. AI'n ansvarar bara för att tolka användarens önskemål och returnera strukturerad information kring bokningen. Därefter är det vår egna backend som kontrollerar att informationen är giltig och om resursen är tillgänglig. 

Användaren måste själv bekräfta bokningen innan en bokning skickas till vårt boknings api. Detta trots att vi har massa tester på vår AI så behöver AIns svar inte alltid vara rätt. 





