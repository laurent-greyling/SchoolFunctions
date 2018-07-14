Function Send-QueueMessage {
    param (
        [Parameter(Mandatory=$true)][string]$ResourceGroupName,
        [Parameter(Mandatory=$true)][string]$SasKeyName,
        [Parameter(Mandatory=$true)][string]$SasKey
    )

    $ErrorActionPreference = "Stop"
     
    $serviceBusResource = Find-AzureRmResource `
        -ResourceType "Microsoft.ServiceBus/namespaces" `
        -ResourceNameContains "coursematerial" `
        -ResourceGroupNameEquals $ResourceGroupName
         
    $nameSpace = $serviceBusResource.Name
    $resourceUri = "https://$nameSpace.servicebus.windows.net/management"

    $sasToken = NewSaSToken -resourceUri $resourceUri -sasKeyName $SasKeyName -sasKey $SasKey
          
    SendMessage `
        -restApiUri "$resourceUri/messages" `
        -sasToken $sasToken

    Write-Host "Sent message" -ForegroundColor Green
}

function NewSaSToken {
    param (
        [parameter(Mandatory=$true)][String]$ResourceUri,
        [parameter(Mandatory=$true)][String]$SasKeyName,
        [parameter(Mandatory=$true)][String]$SasKey
    )

    $dateTime = (Get-Date).ToUniversalTime() - ([datetime]'1/1/1970')
    $weekInSeconds = 7 * 24 * 60 * 60
    $expiry = [System.Convert]::ToString([int]($dateTime.TotalSeconds) + $weekInSeconds)

    Add-Type -AssemblyName System.Web
    $encodedResourceUri = [System.Web.HttpUtility]::UrlEncode($ResourceUri)

    $stringToSign = $encodedResourceUri + "`n" + $expiry
    $stringToSignBytes = [System.Text.Encoding]::UTF8.GetBytes($stringToSign)
    $keyBytes = [System.Text.Encoding]::UTF8.GetBytes($SasKey)
    $hmac = [System.Security.Cryptography.HMACSHA256]::new($keyBytes)
    $hashOfStringToSign = $hmac.ComputeHash($stringToSignBytes)
    $signature = [System.Convert]::ToBase64String($hashOfStringToSign)
    $encodedSignature = [System.Web.HttpUtility]::UrlEncode($signature) 

    $sasToken = "SharedAccessSignature sr=$encodedResourceUri&sig=$encodedSignature&se=$expiry&skn=$SasKeyName" 

    return $sasToken
}

Function SendMessage {
    param (
        [parameter(Mandatory=$true)][String]$RestApiUri,
        [parameter(Mandatory=$true)][String]$SasToken
    )

    $headers = @{'Authorization'=$SasToken} 
    $headers.Add('MessageType','UploadCourse')

    $contentType = 'application/atom+xml;type=entry;charset=utf-8'

    Invoke-RestMethod -Method Post -Uri $RestApiUri -Headers $headers -ContentType $contentType
}

Export-ModuleMember -Function *-*