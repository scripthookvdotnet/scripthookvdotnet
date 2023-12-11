$path = (& "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe" -latest -products * -requires Microsoft.VisualStudio.Workload.ManagedDesktop Microsoft.VisualStudio.Workload.Web -requiresAny -property installationPath)
if (!(test-path $path))
{
    return ""
}

$path = join-path $path 'Common7\IDE\CommonExtensions\Microsoft\TestWindow'
return $path
