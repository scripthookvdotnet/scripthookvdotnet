# Set the working directory to SDK
Set-Location "sdk/"
# Download the file with Invoke-WebRequest
$source = "http://reshade.me/downloads/ScriptHookV_SDK_1.0.617.1a.zip"
Invoke-WebRequest $source -OutFile "sdk.zip"
# Extract the contents of the file in the current directory
7z x sdk.zip
# Return to the location where we started
Set-Location "..\"
