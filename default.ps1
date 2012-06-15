properties {
	$TargetFramework = "net-4.0"
	$DownloadDependentPackages = $true
	$UploadPackage = $false
	$NugetKey = ""
}

$baseDir  = resolve-path .
$buildBase = "$baseDir\build"
$sourceDir = "$baseDir\src"
$outDir =  "$buildBase\output"
$toolsDir = "$baseDir\tools"
$ilMergeTool = "$toolsDir\ILMerge\ILMerge.exe"
$nugetExec = "$toolsDir\NuGet\NuGet.exe"
$script:msBuild = ""
$script:isEnvironmentInitialized = $false
$script:ilmergeTargetFramework = ""
$script:msbuildBaseTargetFramework = ""	
$ilMergeKey = "$srcDir\NServiceBus.snk"
$script:packageVersion = "1.0.0"

include $toolsDir\psake\buildutils.ps1

task default -depends CompileMain

task Clean {
	delete-directory $buildBase -ErrorAction silentlycontinue
}

task Init -depends Clean {
	create-directory $buildBase
}

task InstallDependentPackages {
	cd "$baseDir\packages"
	$files =  dir -Exclude *.config
	cd $baseDir
	$installDependentPackages = $DownloadDependentPackages;
	if($installDependentPackages -eq $false){
		$installDependentPackages = ((($files -ne $null) -and ($files.count -gt 0)) -eq $false)
	}
	if($installDependentPackages){
	 	dir -recurse -include ('packages.config') |ForEach-Object {
		$packageconfig = [io.path]::Combine($_.directory,$_.name)

		write-host $packageconfig 

		 exec{ &$nugetExec install $packageconfig -o packages } 
		}
	}
 }
 
task InitEnvironment{

	if($script:isEnvironmentInitialized -ne $true){
		if ($TargetFramework -eq "net-4.0"){
			$netfxInstallroot ="" 
			$netfxInstallroot =	Get-RegistryValue 'HKLM:\SOFTWARE\Microsoft\.NETFramework\' 'InstallRoot' 
			
			$netfxCurrent = $netfxInstallroot + "v4.0.30319"
			
			$script:msBuild = $netfxCurrent + "\msbuild.exe"
			
			echo ".Net 4.0 build requested - $script:msBuild" 

			$script:ilmergeTargetFramework  = "v4," + $netfxCurrent
			
			$script:msBuildTargetFramework ="/p:TargetFrameworkVersion=v4.0 /ToolsVersion:4.0"
			
			$script:isEnvironmentInitialized = $true
		}
	
	}
}
 
task CompileMain -depends InstallDependentPackages, InitEnvironment, Init {
 	$solutionFile = "SettingsManager.sln"
	exec { &$script:msBuild $solutionFile /p:OutDir="$buildBase\" }
 }

