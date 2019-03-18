Set-Item -Path Env:_JAVA_OPTIONS -Value ("-Xmx256M")
$file = Join-Path $env:NEO4J_BIN 'neo4j.bat'
Write-Host $file
& $file console

