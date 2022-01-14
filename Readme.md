# Pokespeare
![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![C#](https://img.shields.io/badge/c%23-%23239120.svg?style=for-the-badge&logo=c-sharp&logoColor=white)
![React](https://img.shields.io/badge/React-20232A?style=for-the-badge&logo=react&logoColor=white)
![TypeScript](https://img.shields.io/badge/TypeScript-007ACC?style=for-the-badge&logo=typescript&logoColor=white)
![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)

Ever wondered what would it look like if Shakespeare wrote descriptions for Pokémon? Wounder no more. With this web app you can search for a Pokémon by name and get a Shakespeare’s style description. Pokémon data is from pokeapi.co, and translations are powered by funtranslations.com
Build with .NET 6 minimal APIs.

![Screenshot](https://github.com/Vake93/Pokespeare/blob/master/screenshots/screenshot.jpeg?raw=true)

## How To Run
The entire project can be build and executed as a docker container. The only prerequisite is to have docker installed. Following commands will build both the .NET 6 web service and React web app and create a ready to run docker image.

Open a terminal window at the root of the repo and run

```
docker build -t pokespeare -f .\Dockerfile .
```

When the docker image is built run the following command to start it

```
docker run --name pokespeare -p 5000:80 -d pokespeare
```

And open [localhost:5000](http://localhost:5000/) in a browser

## Project Structure 
- pokespear.data - Contains a console application to convert CSV files from [pokeapi.co](https://pokeapi.co/) to JSON format
- pokespeare.api - Contains the web service.
- pokespeare.api.test - Unit and Integration tests for web service
- pokespeare.web - React web app project and unit tests for React components

## Attributions
- PokeAPI - For the Pokémon data [Link](https://github.com/PokeAPI/pokeapi)
- Fun Translations - For translating Pokémon descriptions [Link](https://funtranslations.com/)
