# Ask
A simple console app that answers all your questions! Based on Les Jackson's awesome [ChatGPT .NET API Client](https://www.youtube.com/watch?v=25i-Dh6U6Cw) video.

# Installation
1. Clone the repository.
2. Modify the appsettings.json file to include your API Key.
3. There are two ways to generate the exe: 
    a) Open a terminal in the project's directory and run `dotnet publish -r <RUNTIME_IDENTIFIER>` (See a list of identifiers [here](https://learn.microsoft.com/en-us/dotnet/core/rid-catalog)).
    b) Open the solution with Visual Studio. Right click on the console app project -> Publish. Click on Publish and open the target location.
4. Go to the `release/publish` folder e.g: D:\Development\Ask\Ask\bin\Release\net6.0\win-x64\publish
5. Copy and paste the exe and the appsettings to the location you want on your drive.
6. Add the folder to the PATH environment variable.

# Usage
Open a terminal and run `ask "bash command to remove a directory recursively"`

# More Info
- [Completions API](https://platform.openai.com/docs/api-reference/completions)
- [API Keys](https://platform.openai.com/account/api-keys)