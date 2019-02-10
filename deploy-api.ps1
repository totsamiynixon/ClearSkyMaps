$sourceAPI = "./release/api"
$destinationAPI = "./api.zip"
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($sourceAPI,$destinationAPI)
cmd.exe /c "msdeploy -verb:sync -source:c
ontentPath="C:\deleteme.zip" -dest:auto,computerName='$env:WEB_SERVER_DOMAIN:
8172/msdeploy.axd?site=$env:WEB_SITE_NAME_STAGING_API',username=$env:WEB_SERVER_USER_NAME,p
assword=$env:WEB_SERVER_USER_PASSWORD -allowUntrusted"