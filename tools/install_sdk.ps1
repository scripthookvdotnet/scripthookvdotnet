# Set the working directory to SDK
Set-Location "sdk/"
# Download the file with Invoke-WebRequest.
# The SDK URL is expected to get unavailable every time a new SDK is released.
# See the following issue for details. URL: https://github.com/scripthookvdotnet/scripthookvdotnet/issues/1500
$SdkUrl = "https://ntscorp.ru/dev-c/ScriptHookV_SDK_1.0.617.1a.zip"
Invoke-WebRequest -Uri $SdkUrl -OutFile "sdk.zip" -Headers @{ "Referer" = "http://www.dev-c.com/gtav/scripthookv/" }
# Extract the contents of the file in the current directory
7z x sdk.zip
# Return to the location where we started
Set-Location "..\"
