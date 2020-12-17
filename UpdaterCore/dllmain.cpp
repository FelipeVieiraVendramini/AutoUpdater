// dllmain.cpp : Define o ponto de entrada para o aplicativo DLL.
#include "pch.h"

const char CONFIG_FILE[] { ".\\AutoPatch.ini" };

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    char msg[128];
    /*sprintf_s(msg, "Call: %d", ul_reason_for_call);
    MessageBoxA(NULL, msg, "Init", MB_OK);

    memset(msg, 0, sizeof msg);*/
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    {
        // DWORD idProcess = GetCurrentProcessId();
        HANDLE hProcess = GetCurrentProcess();//OpenProcess(PROCESS_ALL_ACCESS, false, idProcess);
        DWORD dwProc = (DWORD)hProcess;
        if (hProcess) 
        {
            const LPVOID BASE_W_ADDR = (LPVOID) (0x00469DEF + 6);
            const LPVOID BASE_H_ADDR = (LPVOID) (0x00469DFC + 6);

            const LPVOID BASE_PUZZLE_W_ADDR = (LPVOID)(0x0048F898 + 1);
            const LPVOID BASE_PUZZLE_H_ADDR = (LPVOID)(0x0048F8AF + 1);
            const LPVOID BASE_PUZZLE_H2_ADDR = (LPVOID)(0x0048F8CF + 1);

            const LPVOID BASE_FPS1_ADDR = (LPVOID)(0x00468D28);
            const LPVOID BASE_FPS2_ADDR = (LPVOID)(0x00468D31);

            int width = GetPrivateProfileIntA("GameResolution", "Width", 1024, CONFIG_FILE);
            int height = GetPrivateProfileIntA("GameResolution", "Height", 768, CONFIG_FILE);
            int fpsMode = GetPrivateProfileIntA("GameSetup", "FpsMode", 1, CONFIG_FILE);

            int read = 0;
            SIZE_T bytes_read = 0, bytes_written = 0;
            if (ReadProcessMemory(hProcess, BASE_W_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get width memory offset. 1", "ReadProcessMemory error", MB_OK);
                
                if (!WriteProcessMemory(hProcess, BASE_W_ADDR, &width, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 1 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_H_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 2", "ReadProcessMemory error", MB_OK);
                
                if (!WriteProcessMemory(hProcess, BASE_H_ADDR, &height, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 2 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_PUZZLE_W_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get width memory offset. 3", "ReadProcessMemory error", MB_OK);

                sprintf_s(msg, "Puzzle width %d", read);
                MessageBoxA(NULL, msg, "Puzzle", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_PUZZLE_W_ADDR, &width, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 3 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_PUZZLE_H_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 4", "ReadProcessMemory error", MB_OK);

                sprintf_s(msg, "Puzzle height %d", read);
                MessageBoxA(NULL, msg, "Puzzle", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_PUZZLE_H_ADDR, &height, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 4 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }                
            }

            if (ReadProcessMemory(hProcess, BASE_PUZZLE_H2_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 4", "ReadProcessMemory error", MB_OK);

                sprintf_s(msg, "Puzzle height2 %d", read);
                MessageBoxA(NULL, msg, "Puzzle", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_PUZZLE_H2_ADDR, &height, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 4 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            //if (fpsMode != 0) 
            //{
            //    int frameDelayMs = 0x19; // normal
            //    if (fpsMode == 2) // unlocked
            //    {
            //        frameDelayMs = 0x1;
            //    }
            //    else if (fpsMode == 1)
            //    {
            //        frameDelayMs = 0x10;
            //    }

            //    if (ReadProcessMemory(hProcess, BASE_FPS1_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
            //        if (bytes_read == 0)
            //            MessageBoxA(NULL, "Could not get height memory offset.", "ReadProcessMemory error", MB_OK);

            //        if (!WriteProcessMemory(hProcess, BASE_FPS1_ADDR, &frameDelayMs, 4, &bytes_written))
            //        {
            //            sprintf_s(msg, "Error writing to memory! %d", GetLastError());
            //            MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
            //        }
            //    }

            //    if (ReadProcessMemory(hProcess, BASE_FPS2_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
            //        if (bytes_read == 0)
            //            MessageBoxA(NULL, "Could not get height memory offset.", "ReadProcessMemory error", MB_OK);

            //        if (!WriteProcessMemory(hProcess, BASE_FPS2_ADDR, &frameDelayMs, 4, &bytes_written))
            //        {
            //            sprintf_s(msg, "Error writing to memory! %d", GetLastError());
            //            MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
            //        }
            //    }
            //}
        }
        break;
    }
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}