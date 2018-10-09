 param (
    [string]$artifactPath,
    [string]$domain
 )
[string]$web_depoy_host = $env:WEB_DEPLOYER;
[string]$token =  $env:AUTH_TOKEN;
[string]$accountName =  $env:APPVEYOR_ACCOUNT_NAME;
[string]$projectName = $env:APPVEYOR_PROJECT_NAME;
[string]$webSite = $env:WEB_SITE;
[string]$stage = $env:ASPNETCORE_ENVIRONMENT;
$postParams = @{AccountName=$accountName;ProjectName=$projectName;Domain=$domain;Stage=$stage;Token=$token;Package=$artifactPath}
$Response = Invoke-WebRequest -Uri $web_depoy_host -Method POST -Body $postParams
Write-Host $Response.Content
if($Response.StatusCode -ne 200) {
    throw "Web Deploy wasn't successful!"
}