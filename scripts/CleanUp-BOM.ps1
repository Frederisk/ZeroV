#!/usr/bin/pwsh
using namespace System;
using namespace System.IO;
using namespace System.Linq;

param([String]$RootPath = $pwd)

[String[]]$allFiles = Get-ChildItem -Path $RootPath -Name *.cs -File -Recurse | Where-Object -FilterScript { $_ -notmatch '[\\/](?:bin|obj)[\\/]' };
# [FileInfo[]]$allFiles = $allFiles | Where-Object -FilterScript {$_.Name -notmatch 'obj'};

$allFiles | ForEach-Object -Process {
    [Byte[]]$content = Get-Content -Path $_ -AsByteStream -TotalCount 3;
    if (($null -ne $content) -and ($content.Length -eq 3)) {
        # Write-Host $_;
        # Write-Host -Object ($content[0].ToString('X') + $content[1].ToString('X') + $content[2].ToString('X'));
        if ([Enumerable]::SequenceEqual($content, [Byte[]]@(0xEF, 0xBB, 0xBF))) {
            Write-Host -Message ('Removing BOM:' + $_);
            [String]$noBOM = Get-Content -Path $_ -Encoding utf8BOM -Raw;
            Set-Content -Value $noBOM -Path $_ -Encoding utf8NoBOM -NoNewline | Out-Null;
        }
    }
} | Out-Null;
