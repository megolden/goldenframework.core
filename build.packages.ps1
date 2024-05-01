param(
  [Parameter(Mandatory=$true)][string]$Version,
  [Parameter(Mandatory=$true)][string]$Push
)

if (Test-Path packages) {
	
  # Get-ChildItem packages -Recurse | Remove-Item -Recurse -Force -ErrorAction SilentlyContinue
  
}

dotnet pack src/Golden.Common -c Release -o packages -p:DebugType=None -p:Version=$Version
dotnet pack src/Golden.Common.Persian -c Release -o packages -p:DebugType=None -p:Version=$Version

if ("$Push" -match "y|yes|true") {

  dotnet nuget push "packages/Golden.Common.$Version.nupkg" -s local
  dotnet nuget push "packages/Golden.Common.Persian.$Version.nupkg" -s local

}
