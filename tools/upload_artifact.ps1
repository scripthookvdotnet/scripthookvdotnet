# Find all of the NuGet Packages
Get-ChildItem ".\*.nupkg" |
# And upload every single one of them as AppVeyor Artifacts
ForEach-Object {
	Push-AppveyorArtifact $_.FullName -FileName $_.Name
}
