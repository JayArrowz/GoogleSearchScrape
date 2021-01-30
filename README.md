# GoogleSearchScrape
An extendable Search Scraper to list SEO rankings

### Code Docs
* [All Docs](/GoogleSearchScrape/api)

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

## Contributing

Please read [CONTRIBUTING.md](https://gist.github.com/PurpleBooth/b24679402957c63ec426) for details on our code of conduct, and the process for submitting pull requests to us.

## Versioning

We use [SemVer](http://semver.org/) for versioning. For the versions available, see the [tags on this repository](https://github.com/JayArrowz/GoogleSearchScrape/tags). 

## Authors

* **JayArrowz** - [JayArrowz](https://github.com/JayArrowz)

See also the list of [contributors](https://github.com/JayArrowz/GoogleSearchScrape/contributors) who participated in this project.
