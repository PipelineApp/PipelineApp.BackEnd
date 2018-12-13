#!/bin/bash
set -ev
dotnet test "PipelineApp.BackEnd.Test/PipelineApp.BackEnd.Test.csproj" -c Release //p:CollectCoverage=true //p:CoverletOutputFormat=opencover
"$HOME/.nuget/packages/reportgenerator/4.0.4/tools/net47/ReportGenerator.exe" -reports:"PipelineApp.BackEnd.Test/coverage.opencover.xml" -targetdir:"Reports" -verbosity:"Info"
