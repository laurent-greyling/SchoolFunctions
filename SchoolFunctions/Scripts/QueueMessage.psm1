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

Function Retrieve-SignupDetails{
    param(
       [Parameter(Mandatory=$true)][string]$Name,
       [Parameter(Mandatory=$true)][string]$Surname,
       [Parameter(Mandatory=$true)][string]$Course
    )

    $id = $Name.ToLower().Replace(" ","-") + "-" + $Surname.ToLower().Replace(" ", "-") + "-" + $Course.ToLower().Replace(" ", "-")
    
    $storageAccountName = "coursematerial"
    $storageAccount = Get-AzureRmStorageAccount -ResourceGroupName "coursematerial" `
      -AccountName  "coursematerial" `

    $ctx = $storageAccount.Context
    $storageTable = Get-AzureStorageTable –Name $tableName –Context $ctx

    $isSuccess = Get-AzureStorageTableRowByColumnName -table $storageTable `
    -columnName "RowKey" `
    -value $id `
    -operator Equal

    Write-Host $isSuccess.Reason -ForegroundColor Green
}

#Install-Module AzureRmStorageQueue
#Install-Module AzureRmStorageTable

Export-ModuleMember -Function *-*