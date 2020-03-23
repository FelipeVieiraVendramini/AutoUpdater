// CSV3Hook.cpp : Defines the exported functions for the DLL application.
//

#include "stdafx.h"
#include "hook.h"
#include <iostream>
#include "LegacyCipher.h"
#pragma comment(lib, "ws2_32.lib")
#pragma comment(lib, "user32.lib")

#define VERSION 5808

//
// hooks.h is from a library/project (MID) that I (InfamousNoone) wrote back in 2010.
// It was a /FAILED/ project, and I decided to discontinue it.
// If anyone is interested in it, they can see the following link below:
// http://subversion.assembla.com/svn/mid_project/trunk/MID_LIBRARY/
//

#define lib_func(lib, func) (GetProcAddress(GetModuleHandleA(lib), func))

char tqFormat[] = { 0x25, 0x73, 0xA3, 0xAC, 0xA1, 0xA3, 0x66, 0x64, 0x6A, 0x66, 0x2C, 0x6A, 0x6B, 0x67, 0x66, 0x6B, 0x6C, 0x00 };
char szPassword[32];
char szConfig[MAX_PATH];
HOOK_STUB snprintf_stub, send_stub, connect_stub, shellexec_stub;
PBYTE handler_stub;
CLegacyCipher* legacy;

extern LPVOID FindMemoryPattern(PBYTE pattern, bool* wildCards, int len);

int csv3_snprintf(char* str, int len, const char* format, ...)
{
	va_list args;
	va_start(args, format);
	if (strcmp(format, tqFormat) == 0)
	{
		char* password = va_arg(args, PCHAR);
		strcpy(szPassword, password);
		str[0] = NULL;
		strcat(str, password);
		strcat(str, &tqFormat[2]);

		//MessageBoxA(NULL, szPassword, "Password", MB_OK);

		return strlen(str);
	}
	else
	{
		return vsnprintf(str, len, format, args);
	}
}
int __stdcall csv3_connect(SOCKET s, sockaddr_in* name, int len)
{
	char szIPAddress[32];
	int nPort, rPort;
	
	rPort = ntohs(name->sin_port);
	if (rPort >= 9950 && rPort <= 9970)
	{
		GetPrivateProfileStringA("config", "host", "127.0.0.1", szIPAddress, 32, szConfig);
		nPort = GetPrivateProfileIntA("config", "port", 9958, szConfig);
	
		name->sin_addr.s_addr = inet_addr(szIPAddress);
		name->sin_port = htons(nPort);

		legacy = new CLegacyCipher();
		legacy->GenerateIV(0x13FA0F9D, 0x6D5C7962);
	}
	else if (rPort == 5816)
	{
		GetPrivateProfileStringA("config", "gamehost", "127.0.0.1", szIPAddress, 32, szConfig);
		nPort = GetPrivateProfileIntA("config", "gameport", 5816, szConfig);

		name->sin_addr.s_addr = inet_addr(szIPAddress);
		name->sin_port = htons(nPort);
	}

	typedef int (__stdcall *LPFCONNECT)(SOCKET,sockaddr_in*,int);
	return ((LPFCONNECT)connect_stub.Address)(s, name, len);
}
int __stdcall csv3_send(SOCKET s, PBYTE buf, int len, int flags)
{
	//MessageBoxA(NULL, "csv3_send()", "CSV3", MB_OK);

	if (legacy)
	{
		legacy->Decrypt(buf, len);

		strcat((char*)&buf[4+4+64], szPassword);

		legacy->Encrypt(buf, len);
		delete legacy;
		legacy = NULL;
	}

	// invoke old method
	typedef int (__stdcall *LPFSEND)(SOCKET,PBYTE,int,int);
	return ((LPFSEND)send_stub.Address)(s,buf,len,flags);
}
int __stdcall csv3_shellexec(HWND hwnd, char* lpOperation, char* lpFile, char* lpParams, char* lpDir, int nCmd)
{
	return SE_ERR_FNF;
}

BOOL __stdcall csv3_handler(PBYTE msg)
{
	return TRUE; // allow TQ to process this packet
}

PBYTE tq_handler;
__declspec(naked) BOOL __stdcall csv3_handler_hook()
{
	__asm
	{
		nop
		push ebp
		mov ebp, esp
		push dword ptr[ebp+12]
		push dword ptr[ebp+8]
		// ecx is already set
		mov eax, tq_handler		// this method cleans up after itself __stdcall
		call eax
		test eax,eax
		jz _leave
		// we have a packet... pass it to csv3_handler
		push dword ptr[ebp+12]
		call csv3_handler
		//
		_leave:
		pop ebp
		ret 8 // clean up after self
	}
}

PBYTE tq_send;
__declspec(naked) DWORD __stdcall csv3_tqsend(PBYTE msg, DWORD len)
{
	__asm
	{
		nop
		mov eax, tq_send
		jmp eax
	}
}

void csv3_init(HMODULE hModule)
{
	float f = 0; // load support for floating operations -- thx ntl3fty!
	char szDebug[256];
	
	//
	//	hook packet processor
	//

#if  VERSION < 5810
	BYTE pattern[] = { 0x85, 0xC0, 0x75, 0x00, 0x8B, 0x4E, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0x4D, 0x00, 0x00, 0x8B, 0x00, 0x00 };
#else
	BYTE pattern[] = { 0x85, 0xC0, 0x75, 0x00, 0x8B, 0x4F, 0x00, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x83, 0x4D, 0x00, 0x00, 0x8B, 0x00, 0x00 };
#endif
	bool wildcards[] = { 0, 0, 0, 1, 0, 0, 1, 0, 1, 1, 1, 1, 0, 0, 1, 1, 0, 1, 1 };
	PBYTE match = (PBYTE)FindMemoryPattern(pattern, wildcards, 19);

	if (match==NULL)
	{
		sprintf(szDebug, "failed 1", (DWORD)match);
		MessageBoxA(NULL, szDebug, "ERROR", MB_OK);
		return;
	}

	DWORD callDstInter;
	match = match - 5; // where the call occurs
	ReadProcessMemory(GetCurrentProcess(), match+1, &callDstInter, sizeof(callDstInter), NULL);
	tq_handler = (PBYTE)( (DWORD)match - (UINT_MAX - callDstInter) + 4 );
	// update the intermediate to our hook
	callDstInter = (DWORD)csv3_handler_hook - (DWORD)match + UINT_MAX - 4;
	WriteProcessMemory(GetCurrentProcess(), match+1, &callDstInter, sizeof(callDstInter), NULL);

	BYTE pattern2[] = { 0x85, 0xC9, 0x00, 0x00, 0xFF, 0x75, 0x00, 0x57, 0xE8, 0x00, 0x00, 0x00, 0x00, 0x89, 0x45 };
	bool wildcards2[] = { 0, 0, 1, 1, 0, 0, 1, 0, 0, 1, 1, 1, 1, 0, 0 };
	match = (PBYTE)FindMemoryPattern(pattern2, wildcards2, 15);
	
	if (match==NULL)
	{
		sprintf(szDebug, "failed 2", (DWORD)match);
		MessageBoxA(NULL, szDebug, "ERROR", MB_OK);
		return;
	}

	match += 8;
	ReadProcessMemory(GetCurrentProcess(), match+1, &callDstInter, sizeof(callDstInter), NULL);
	tq_send = (PBYTE)( (DWORD)match - (UINT_MAX - callDstInter) + 4);

	//sprintf(szDebug, "TQ Send: %x", (DWORD)tq_send);
	//MessageBoxA(NULL, szDebug, "OK", MB_OK);

	//
	//
	//

	//MessageBoxA(NULL, "csv3_init()", "CSV3", MB_OK);
	GetModuleFileNameA(NULL, szConfig, MAX_PATH);
	for (int i = strlen(szConfig) - 1; i >= 0; i--)
	{
		if (szConfig[i] == '\\')
		{
			szConfig[i+1] = NULL;
			break;
		}
	}
	strcat(szConfig, "LoaderSettings.ini");

	memset(szPassword, 0, 32);

	CreateHook32(lib_func("msvcr90.dll", "_snprintf"), csv3_snprintf, &snprintf_stub);
	CreateHook32(lib_func("ws2_32.dll", "send"), csv3_send, &send_stub);
	CreateHook32(lib_func("ws2_32.dll", "connect"), csv3_connect, &connect_stub);
	CreateHook32(lib_func("Shell32.dll", "ShellExecuteA"), csv3_shellexec, &shellexec_stub);
}