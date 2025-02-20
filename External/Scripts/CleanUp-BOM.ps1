#!/usr/bin/pwsh
#requires -Version 7

using namespace System;
using namespace System.IO;
using namespace System.Linq;
using namespace System.Management.Automation;

[CmdletBinding()]
param(
    [String]$RootPath = $pwd,
    [String]$FileName = '*.cs'
)

[String[]]$allFiles = Get-ChildItem -Path $RootPath -Name $FileName -File -Recurse | Where-Object -FilterScript { $_ -notmatch '(?i)(?:^|[\\/])(?:bin|obj)[\\/]' };
Write-Verbose -Message 'All files have been obtained.';
# [FileInfo[]]$allFiles = $allFiles | Where-Object -FilterScript {$_.Name -notmatch 'obj'};

$allFiles | ForEach-Object -Process {
    [Byte[]]$content = Get-Content -Path $_ -AsByteStream -TotalCount 3;
    if (($null -ne $content) -and (3 -eq $content.Length)) {
        # Write-Host $_;
        # Write-Host -Object ($content[0].ToString('X') + $content[1].ToString('X') + $content[2].ToString('X'));
        if ([Enumerable]::SequenceEqual($content, [Byte[]]@(0xEF, 0xBB, 0xBF))) {
            Write-Verbose -Message ('Removing BOM:' + $_ + '... ');
            [String]$noBOM = Get-Content -Path $_ -Encoding utf8BOM -Raw;
            Set-Content -Value $noBOM -Path $_ -Encoding utf8NoBOM -NoNewline | Out-Null;
            # Write-Verbose -Message 'Done!';
        }
    }
} | Out-Null;
Write-Verbose -Message 'All Done!';
