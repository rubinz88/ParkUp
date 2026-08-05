# ParkUp API leírás

Alap URL Docker használatakor: `http://localhost:8002`

Az API JSON formátumot használ. A sikeres létrehozó műveletek `201 Created`, a sikeres lekérdezések `200 OK`, a sikeres törlés vagy cancel művelet pedig `204 No Content` választ ad.

## Requesterek

### Összes requester lekérése

```http
GET /api/requesters
```

Válasz:

```json
[
  {
    "id": 1,
    "name": "Teszt Elek",
    "email": "teszt@example.com"
  }
]
```

### Új requester létrehozása

```http
POST /api/requesters
Content-Type: application/json
```

Kérés törzse:

```json
{
  "name": "Teszt Elek",
  "email": "teszt@example.com"
}
```

Hibák: `400 Bad Request` hibás adatoknál, `409 Conflict` már használt email címnél.

### Requester modosítása

```http
PATCH /api/requesters/{id}
Content-Type: application/json
```

A kérésben csak a módosítandó mezőket kell megadni:

```json
{
  "name": "Új Név"
}
```

### Requester törlése

```http
DELETE /api/requesters/{id}
```

Sikeres törlés: `204 No Content`. Foglalással rendelkező requester nem törölhető, ilyenkor `409 Conflict` válasz érkezik.

### Requester eligibility beallitasa

```http
PUT /api/requesters/{id}/eligibility
Content-Type: application/json
```

Disabled jogosultsag beallitasa (`eligibilityTypeId: 1`):

```json
{
  "eligibilityTypeId": 1
}
```

Az endpoint idempotens: ugyanaz a jogosultsag tobbszori beallitasa nem hoz letre duplikalt rekordot.

## Parking spotok

```http
GET    /api/parkingspot
GET    /api/parkingspot/{id}
POST   /api/parkingspot
PUT    /api/parkingspot/{id}
PATCH  /api/parkingspot/{id}
DELETE /api/parkingspot/{id}
```

A parking spot létrehozásának törzse:

```json
{
  "id": 0,
  "parkingSpotName": "A-101",
  "buildingId": 1,
  "floorNumber": 1,
  "price": 12.5,
  "eligibilityTypeId": null
}
```

## Foglalások

### Foglalás létrehozása

```http
POST /api/reservations
Content-Type: application/json
```

Kérés törzse:

```json
{
  "parkingSpotId": 1,
  "requesterId": 10,
  "startingDate": "2026-08-10T10:00:00Z",
  "endingDate": "2026-08-10T12:00:00Z"
}
```

A rendszer ellenőrzi a dátumtartományt, a kapcsolódó rekordokat, az eligibility jogosultságot és az időbeli ütközéseket.

Lehetséges hibák: `400 Bad Request`, `403 Forbidden`, `404 Not Found`, `409 Conflict`.

### Egy foglalás lekérése

```http
GET /api/reservations/{id}
```

### Egy parking spot foglalásainak lekérése

```http
GET /api/parkingspots/{parkingSpotId}/reservations
```

### Foglalás lemondása

```http
DELETE /api/reservations/{id}
```

Ez a művelet nem törli fizikailag a rekordot, hanem `Cancelled` státuszra állítja.

## Swagger

Fejlesztői környezetben a Swagger UI itt érhető el:

```text
http://localhost:8002/swagger
```
