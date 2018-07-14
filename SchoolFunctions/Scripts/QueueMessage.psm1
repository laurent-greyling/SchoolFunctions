Function Send-QueueMessage {
    param (
        [Parameter(Mandatory=$true)][string]$ResourceGroupName,
        [Parameter(Mandatory=$true)][string]$UserDetails
    )

    try{
        $queue = Get-AzureRmStorageQueueQueue -resourceGroup $ResourceGroupName -storageAccountName "coursematerial" -queueName "management"
        $message = $UserDetails | ConvertFrom-Json

        Add-AzureRmStorageQueueMessage -queue $queue -message $UserDetails
        Write-Host "Successfully sent" -ForegroundColor Green
    }
    catch{
        $ErrorMessage = $_.Exception.Message
        Write-Error "Oeps, something is wrong, check your userdetails Json string and try again"
        Write-Error $ErrorMessage
    }    
}

Export-ModuleMember -Function *-*