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
[string]$package = [string]::Format("https://ci.appveyor.com/api/projects/{0}/{1}/artifacts/{2}?branch={3}",$accountName,$projectName,$artifactName,$branch);
$postParams = @{PackageUrl=$package;Domain=$domain;Token=$token;}
$Response = Invoke-WebRequest -Uri $web_depoy_host -Method POST -Body $postParams
Write-Host $Response.Content
if($Response.StatusCode -ne 200) {
    throw "Web Deploy wasn't successful!"
}