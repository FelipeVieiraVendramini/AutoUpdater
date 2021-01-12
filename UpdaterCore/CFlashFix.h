#pragma once
class CFlashFix
{
	typedef HMODULE(_stdcall* HookLoadLibrary)(_In_ LPCWSTR lpLibFileName,
		_Reserved_ HANDLE hFile,
		_In_ DWORD dwFlags);
public:
	static void Hook();
	static HookLoadLibrary pLoadLibrary;
	static  HMODULE _stdcall LoadLibraryDetour(_In_ LPCWSTR lpLibFileName,
		_Reserved_ HANDLE hFile,
		_In_ DWORD dwFlags);
};