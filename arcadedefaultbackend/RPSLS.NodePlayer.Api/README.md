## RPSLS NodePlayer Default Backend

### Build
```
docker build -t danielmeixner/arcadedefaultbackend .
```

### Run in docker
```
docker run -p 8080:8080 -e PICK_STRATEGY=RANDOM -e PORT=8080 danielmeixner/arcadedefaultbackend
```

### Access via Postman
Post e.g. 
```
{
    "challengerId":"daniel",
    "matchId":"42"
}
```
to http://localhost:8080/pick
