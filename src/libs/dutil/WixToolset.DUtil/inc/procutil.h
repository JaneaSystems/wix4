#pragma once
// Copyright (c) .NET Foundation and contributors. All rights reserved. Licensed under the Microsoft Reciprocal License. See LICENSE.TXT file in the project root for full license information.


#ifdef __cplusplus
extern "C" {
#endif

// structs
typedef struct _PROC_FILESYSTEMREDIRECTION
{
    BOOL fDisabled;
    LPVOID pvRevertState;
} PROC_FILESYSTEMREDIRECTION;

HRESULT DAPI ProcElevated(
    __in HANDLE hProcess,
    __out BOOL* pfElevated
    );

HRESULT DAPI ProcSystem(
    __in HANDLE hProcess,
    __out BOOL* pfSystem
    );

HRESULT DAPI ProcGetTokenInformation(
    __in HANDLE hProcess,
    __in TOKEN_INFORMATION_CLASS tokenInformationClass,
    __out LPVOID* ppvTokenInformation
    );

HRESULT DAPI ProcHasPrivilege(
    __in HANDLE hProcess,
    __in LPCWSTR wzPrivilegeName,
    __out BOOL* pfHasPrivilege
    );

HRESULT DAPI ProcEnablePrivilege(
    __in HANDLE hProcess,
    __in LPCWSTR wzPrivilegeName
    );

HRESULT DAPI ProcWow64(
    __in HANDLE hProcess,
    __out BOOL* pfWow64
    );
HRESULT DAPI ProcNativeMachine(
    __in HANDLE hProcess,
    __out USHORT* pusNativeMachine
    );
HRESULT DAPI ProcDisableWowFileSystemRedirection(
    __in PROC_FILESYSTEMREDIRECTION* pfsr
    );
HRESULT DAPI ProcRevertWowFileSystemRedirection(
    __in PROC_FILESYSTEMREDIRECTION* pfsr
    );

HRESULT DAPI ProcExec(
    __in_z LPCWSTR wzExecutablePath,
    __in_z_opt LPCWSTR wzCommandLine,
    __in int nCmdShow,
    __out HANDLE *phProcess
    );
HRESULT DAPI ProcExecute(
    __in_z LPWSTR wzCommand,
    __out HANDLE *phProcess,
    __out_opt HANDLE *phChildStdIn,
    __out_opt HANDLE *phChildStdOutErr
    );
HRESULT DAPI ProcWaitForCompletion(
    __in HANDLE hProcess,
    __in DWORD dwTimeout,
    __out_opt DWORD* pdwReturnCode
    );
HRESULT DAPI ProcWaitForIds(
    __in_ecount(cProcessIds) const DWORD* pdwProcessIds,
    __in DWORD cProcessIds,
    __in DWORD dwMilliseconds
    );
HRESULT DAPI ProcCloseIds(
    __in_ecount(cProcessIds) const DWORD* pdwProcessIds,
    __in DWORD cProcessIds
    );

// following code in proc2utl.cpp due to dependency on PSAPI.DLL.
HRESULT DAPI ProcFindAllIdsFromExeName(
    __in_z LPCWSTR wzExeName,
    __out DWORD** ppdwProcessIds,
    __out DWORD* pcProcessIds
    );

// following code in proc3utl.cpp due to dependency on Wtsapi32.DLL.
HRESULT DAPI ProcExecuteAsInteractiveUser(
    __in_z LPCWSTR wzExecutablePath,
    __in_z LPCWSTR wzCommand,
    __out HANDLE *phProcess
    );

#ifdef __cplusplus
}
#endif
