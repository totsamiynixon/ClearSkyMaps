 param (
    [string]$artifactName,
    [string]$domain
 )
[string]$web_depoy_host = $env:WEB_DEPLOYER;
[string]$token =  $env:AUTH_TOKEN;
[string]$accountName =  $env:APPVEYOR_ACCOUNT_NAME;
[string]$projectName = $env:APPVEYOR_PROJECT_NAME;
[string]$webSite = $env:WEB_SITE;
[string]$branch = $env:APPVEYOR_REPO_BRANCH;
[string]$package = "https://ci.appveyor.com/api/projects/$accountName/$projectName/artifacts/$artifactName?branch=$branch";
$postParams = @{PackageUrl=$package;Domain=$domain;Token=$token;}
$Response = Invoke-WebRequest -Uri $web_depoy_host -Method POST -Body $postParams
Write-Host $Response.Content
if($Response.StatusCode -ne 200) {
    throw "Web Deploy wasn't successful!"
}