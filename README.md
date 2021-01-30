# GoogleSearchScrape
An extendable Search Scraper to list SEO rankings

### Code Docs
* [All Docs](https://jayarrowz.github.io/GoogleSearchScrape/api)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites
* [PostgresSQL](https://www.postgresql.org/download/)
* [Net5.0](https://dotnet.microsoft.com/download/dotnet/5.0)

### Installing
1. Go to [appsettings.json](https://github.com/JayArrowz/GoogleSearchScrape/blob/master/GoogleSearchScrape/appsettings.json) and ensure the ConnectionString to your database is correct
2. Go to your Terminal (Make sure its current directory is matching the root of this repo) or VS Console then type:
```
dotnet tool install -g dotnet-ef
dotnet build
dotnet ef database update --project GoogleSearchScrape
```

To Run in Terminal: 
```
dotnet run --project GoogleSearchScrape
```

## How To Use
1. Add a scrape request on the page with any specified strategy 'https://localhost:5001/requests'. The *search term* is what will be used when making the reqeust to the search engine, the max results will limit the results of this request. The *target URL* will determine the results saved by the application, if the URL returned by the search scraper **CONTAINS** the target URL (it doesn't need to be a URL) then the application will save the result, if not it will be discarded. The *repeat time* will determine when a request is scraped again.
![ImageTwo](https://i.imgur.com/0rwJDxI.png)

2. Go to the dashboard and wait for the graph to update 'https://localhost:5001/'
![ImageThree](https://i.imgur.com/UqVszq5.png)

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/JayArrowz/GoogleSearchScrape/tags). 

## Authors

* **JayArrowz** - [JayArrowz](https://github.com/JayArrowz)

See also the list of [contributors](https://github.com/JayArrowz/GoogleSearchScrape/contributors) who participated in this project.
