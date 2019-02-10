$sourcePWA = "./release/pwa"
$destinationPWA = "./pwa.zip"
Add-Type -assembly "system.io.compression.filesystem"
[io.compression.zipfile]::CreateFromDirectory($sourcePWA,$destinationPWA)
cmd.exe /c "msdeploy -verb:sync -source:c
ontentPath="C:\deleteme.zip" -dest:auto,computerName='$env:WEB_SERVER_DOMAIN:
8172/msdeploy.axd?site=$env:WEB_SITE_NAME_STAGING_PWA',username=$env:WEB_SERVER_USER_NAME,p
assword=$env:WEB_SERVER_USER_PASSWORD -allowUntrusted"