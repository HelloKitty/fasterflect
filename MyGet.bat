%NUGET% restore Fasterflect.sln -NoCache -NonInteractive -ConfigFile Nuget.config
msbuild Fasterflect.sln /p:Configuration=Release