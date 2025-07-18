# Set the working directory to SDK
Set-Location "sdk/"
# Download the file with `Invoke-WebRequest`.
# The command can fail for `https://ntscorp.ru` returning the HTTP code 526 because its SSL certificate is expired.
# If that happens, blame whoever configure that website, not clients (or users that are downloading the SDK).
$SdkUrl = "https://ntscorp.ru/dev-c/ScriptHookV_SDK_1.0.617.1a.zip"
Invoke-WebRequest -Uri $SdkUrl -OutFile "sdk.zip" -Headers @{ "Referer" = "http://www.dev-c.com/gtav/scripthookv/" }
# Extract the contents of the file in the current directory
7z x sdk.zip
# Return to the location where we started
Set-Location "..\"
