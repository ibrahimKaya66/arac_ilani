# main branch protection - GitHub API ile
# Kullanım: .\protect-main-branch.ps1 -Token "ghp_xxx"
# Token: Settings > Developer settings > Personal access tokens (repo + admin:repo)

param(
    [Parameter(Mandatory=$true)]
    [string]$Token
)

$owner = "ibrahimKaya66"
$repo = "arac_ilani"
$branch = "main"

$body = '{"required_pull_request_reviews":null,"required_status_checks":null,"enforce_admins":true,"restrictions":null,"allow_force_pushes":false,"allow_deletions":false}'

$headers = @{
    "Authorization" = "Bearer $Token"
    "Accept" = "application/vnd.github+json"
    "X-GitHub-Api-Version" = "2022-11-28"
}

try {
    $response = Invoke-RestMethod -Uri "https://api.github.com/repos/$owner/$repo/branches/$branch/protection" `
        -Method Put -Headers $headers -Body $body -ContentType "application/json"
    Write-Host "main branch korundu."
} catch {
    Write-Host "Hata: $_"
}
