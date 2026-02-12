dotnet publish ../src/WateryTart.Platform.Windows -p:PublishProfile=Winx64 -o ../output

vpk pack --packId WateryTart --channel win-x64 --packVersion 1.0.1 --packDir ../output --mainExe WateryTartDesktop.exe  --icon ../Assets/logo_square.ico --outputDir ../releases