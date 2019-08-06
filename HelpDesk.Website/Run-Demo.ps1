$ScriptPath = split-path $SCRIPT:MyInvocation.MyCommand.Path -parent


function Start-ApiService()
{
	$ErrorActionPreference = 'SilentlyContinue'
	Write-Host "Starting Helpdesk API Service..."

	try
	{
		$process = Get-Process -Name "HelpDesk.Api"
		if ($process -eq $null)
		{
			Start-Process "cmd.exe" -ArgumentList @("/C", (Join-Path $ScriptPath "..\Helpdesk.Api\bin\Debug\netcoreapp2.1\win-x64\Helpdesk.Api.exe")) -WindowStyle Normal		
			Start-Sleep 5	
			Write-Host "Helpdesk API Service is running..."
		}
		else
		{
			Write-Host "Helpdesk API Service is already running..."
		}
	}
	catch
	{
		Write-Host "Helpdesk API Service is already running..."
	}
}

function Start-Website()
{
	$ErrorActionPreference = 'SilentlyContinue'
	Write-Host "Starting Web Server..."

	try
	{
	    Start-Process "cmd.exe" -ArgumentList @("/C", "cd $ScriptPath & npm run start") -WindowStyle Normal			
		Write-Host "Web Server running..."
	}
	catch
	{
		Write-Host "Web Server is already running..."
	}
}


Start-ApiService
Start-Website
