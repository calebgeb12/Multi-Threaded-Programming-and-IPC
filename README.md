# Multi-Threaded-Programming-and-IPC Setup Guide

1) In your computer terminal, run the following command: "wsl --install" (without the quotes)
2) If you get any errors, paste them into ChatGPT and follow ChatGPT's instructions
3) Install Ubuntu Desktop from the Microsoft Store or through your preferred browser
4) After Ubuntu finishes downloading, click on it and go through the setup process for installation
5) After Ubuntu finishes installation, open it and you should see a linux terminal=
7) Download the latest .NET version and set it up
8) In the linux terminal, copy and paste the following to ensure .NET has been setup properly: dotnet --version
9) type ls to bring up your folder directory
10) use cd to navigate into "Multi-Threaded-Programming-and-IPC" directory
11) type ls again and you should see the following options: MyApp, README.md
12) cd into MyApp: "cd MyApp"
13) type ls again and you should the these two programs along with some others: IPC.cs, Program.cs
14) Now, run the following program, replacing <fileName> with the program you want to run (omit the .cs at the end): dotnet run --project ./MyApp.csproj --property:StartupObject=MyApp.<fileName>

    a) for Program.cs: dotnet run --project ./MyApp.csproj --property:StartupObject=MyApp.Program
    
    b) for IPC.cs: dotnet run --project ./MyApp.csproj --property:StartupObject=MyApp.IPC
    
16) if you want to stop one of these programs prematurely, click "cntrl + c"
