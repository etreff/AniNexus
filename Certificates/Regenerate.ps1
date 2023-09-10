$certname = Read-Host 'Cert Name (Hit enter to skip)'
if ([System.String]::IsNullOrWhiteSpace($certname) -eq $false)
{
    $cert = New-SelfSignedCertificate -Subject "$certname" -CertStoreLocation 'Cert:\\CurrentUser\\My' -KeyExportPolicy Exportable -KeySpec Signature -KeyLength 4096 -KeyAlgorithm RSA -HashAlgorithm SHA512

    Write-Host 'Exporting cer.'
    Export-Certificate -Cert $cert -FilePath "$certname.cer" >$null

    $certPass = Read-Host 'Cert Pass'
    $certPassSecure = ConvertTo-SecureString -String $certPass -Force -AsPlainText
    Write-Host 'Exporting pfx.'
    Export-PfxCertificate -Cert $cert -FilePath "$certname.pfx" -Password $certPassSecure >$null

    Write-Host 'Importing pfx.'
    Import-PfxCertificate -Password $certPassSecure -CertStoreLocation 'Cert:\\CurrentUser\\My' -FilePath "$certname.pfx" >$null

    Set-Location -Path '..\AniNexus.Web'
    & dotnet user-secrets set "Azure:CertPass" $certPass
    & dotnet user-secrets set "Azure:Thumbprint" $cert.Thumbprint
}

$tenantId = Read-Host 'Tenant Id (Hit enter to skip)'
if ([System.String]::IsNullOrWhiteSpace($tenantId) -eq $false)
{
    & dotnet user-secrets set "Azure:TenantId" $tenantId
}

$clientId = Read-Host 'Client Id (Hit enter to skip)'
if ([System.String]::IsNullOrWhiteSpace($clientId) -eq $false)
{
    & dotnet user-secrets set "Azure:ClientId" $clientId
}

$connectionString = Read-Host 'Connection String (Hit enter to skip)'
if ([System.String]::IsNullOrWhiteSpace($connectionString) -eq $false)
{
    & dotnet user-secrets set "Azure:ConnectionString" $connectionString
}

Pop-Location