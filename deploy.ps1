 param (
    [string]$artifactPath
 )
[string]$web_depoy_host = $env:WEB_DEPLOYER;
[string]$token =  $env:AUTH_TOKEN;
[string]$accountName =  $env:APPVEYOR_ACCOUNT_NAME;
[string]$projectName = $env:APPVEYOR_PROJECT_NAME;
[string]$webSite = $env:WEB_SITE;
[string]$stage = $env:DEPLOY_STAGE;
$postParams = @{AccountName=$accountName;ProjectName=$projectName;WebSite=$webSite;Stage=$stage;Token=$token;Package=$artifactPath}
$Response = Invoke-WebRequest -Uri $web_depoy_host -Method POST -Body $postParams
Write-Host $Response.Content