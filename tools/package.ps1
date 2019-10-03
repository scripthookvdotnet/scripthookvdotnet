# Get all of the SHVDN Projects
Get-ChildItem ".\source\**\ScriptHookVDotNet*.csproj" |
# And package them using NuGet
ForEach-Object {
	nuget pack $_.FullName
}
