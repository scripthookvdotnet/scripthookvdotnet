# Set the working directory to SDK
Set-Location "sdk/"
# Download the file with Invoke-WebRequest
$source = "https://drive.google.com/uc?export=download&id=1NPh-CQYDUm10vk53LagICROEpDY4aTo8"
Invoke-WebRequest $source -OutFile "sdk.zip"
# Extract the contents of the file in the current directory
7z x sdk.zip
# Return to the location where we started
Set-Location "..\"
