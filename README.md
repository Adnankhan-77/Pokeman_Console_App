# Stuller_Console_App_Latest

## Overview
This console application determines a given Pokemon's effectiveness against other Pokemon types using the PokeAPI. It retrieves type relations to determine strengths and weaknesses based on attack and defense effectiveness.

## Features
- Accepts a Pokemon name as user input
- Fetches type effectiveness from PokeAPI
- Displays strengths and weaknesses against other types
- Handles exceptions and empty responses gracefully

## Prerequisites
- .NET 9 SDK
- Internet connection (to fetch data from PokeAPI)

## Installation
1. Clone this repository:
   ```sh
   git clone https://github.com/Adnankhan-77/Pokeman_Console_App.git
   ```
2. Navigate to the project directory:
   ```sh
   cd Pokeman_Console_App/Stuller_Console_App_Latest
   ```
3. Restore dependencies:
   ```sh
   dotnet restore
   ```

## Running the Application
Execute the following command in the project directory:
```sh
dotnet run
```

## Usage
1. After running the application, enter a Pokemon name when prompted.
2. The app will fetch the Pokemon's type(s) and display:
   - Strengths: Types the Pokemon is strong against
   - Weaknesses: Types the Pokemon is weak against
3. Type `exit` to close the application.

### Example Interaction:
```
Enter Pokemon name (or type 'exit' to quit): pikachu
Strengths: Water, Flying
Weaknesses: Ground
```

## Error Handling
- If an invalid Pokemon name is entered, the app will display an error message.
- If the API request fails, an appropriate message will be shown.
- Handles empty or unexpected responses from PokeAPI.

## Dependency Injection
The application uses `Microsoft.Extensions.DependencyInjection` to manage dependencies.

## License
No License required.

-----------

