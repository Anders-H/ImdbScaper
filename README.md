# ImdbScaper

IMDb Scraper is a simple .NET 8 library for extracting a movie title, rating and year from a IMDb ID.

```
var repository = new Repository();
var result = repository.GetMovie(87332);
Console.WriteLine(result.ToString());
```

Result (from Sweden):

```
IMDb ID: 87332
OK: True
Scrape date: 2023-06-18
Regional title: Ghostbusters - Spökligan
Original title: Ghostbusters
Year: 1984
Rating: 7,8
Url: https://www.imdb.com/title/tt0087332/
Repository result status: FromSource
```