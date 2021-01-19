// dllmain.cpp : Define o ponto de entrada para o aplicativo DLL.
#include "pch.h"
#include "CFlashFix.h"

const char CONFIG_FILE[] { ".\\Config.ini" };

#define VERSION     5187

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
#if VERSION == 5078
            const LPVOID BASE_W_ADDR = (LPVOID)(0x004842BD + 6);
            const LPVOID BASE_H_ADDR = (LPVOID)(0x004842CA + 6);

            const LPVOID BASE_PUZZLE_W_ADDR = (LPVOID)(0x004AD21A + 1);
            const LPVOID BASE_PUZZLE_H_ADDR = (LPVOID)(0x004AD231 + 1);
            const LPVOID BASE_PUZZLE_H2_ADDR = (LPVOID)(0x004AD251 + 1);

            const LPVOID BASE_MAIN_UI0_ADDR = (LPVOID)(0x00488D09 + 1);
            const LPVOID BASE_MAIN_UI1_ADDR = (LPVOID)(0x00488D0E + 1);

            const LPVOID BASE_ARROW_ICON_ADDR = (LPVOID)(0x004C852E + 1);
            const LPVOID BASE_ARROW_STRING_BLACK_ADDR = (LPVOID)(0x004C858E + 1);
            const LPVOID BASE_ARROW_STRING_WHITE_ADDR = (LPVOID)(0x004C85C8 + 1);

            const LPVOID BASE_HELP_BUTTON_ADDR = (LPVOID)(0x00430D50 + 1);
#elif VERSION == 5187


            const LPVOID BASE_W_ADDR = (LPVOID)(0x004EB22F + 6);
            const LPVOID BASE_H_ADDR = (LPVOID)(0x004EB239 + 6);
            const LPVOID BASE_W2_ADDR = (LPVOID)(0x004ED815 + 6);// new location pos=width
            const LPVOID BASE_H2_ADDR = (LPVOID)(0x004ED81F + 6);// new location pos=height

            const LPVOID BASE_PUZZLE_W_ADDR = (LPVOID)(0x00539C34 + 1);
            const LPVOID BASE_PUZZLE_H_ADDR = (LPVOID)(0x00539C4B + 1);
            const LPVOID BASE_PUZZLE_H2_ADDR = (LPVOID)(0x00539C6B + 1);

            const LPVOID BASE_MAIN_UI0_ADDR = (LPVOID)(0x004F7B8A + 1);
            const LPVOID BASE_MAIN_UI1_ADDR = (LPVOID)(0x004F7B8F + 1);

            // arrow only 1 location 
            const LPVOID BASE_ARROW_ADDR = (LPVOID)(0x00553B2D + 3); // int position = ((width - 1024) / 2) + 131;

            const LPVOID BASE_HELP_BUTTON_ADDR = (LPVOID)(0x0045649C + 1);

            // actually we can unlock fps XD
            const LPVOID BASE_FPS1_ADDR = (LPVOID)(0x004EB3AB + 2);// sbyte
            const LPVOID BASE_FPS2_ADDR = (LPVOID)(0x004EB3B4 + 2);// sbyte
#endif

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

#if VERSION >= 5187
            if (ReadProcessMemory(hProcess, BASE_W2_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get width memory offset. 1", "ReadProcessMemory error", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_W2_ADDR, &width, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 1 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_H2_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 2", "ReadProcessMemory error", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_H2_ADDR, &height, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 2 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }
#endif

            if (ReadProcessMemory(hProcess, BASE_PUZZLE_W_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get width memory offset. 3", "ReadProcessMemory error", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_PUZZLE_W_ADDR, &width, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 3 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_PUZZLE_H_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 4", "ReadProcessMemory error", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_PUZZLE_H_ADDR, &height, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 4 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }                
            }

            if (ReadProcessMemory(hProcess, BASE_PUZZLE_H2_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 4", "ReadProcessMemory error", MB_OK);

                if (!WriteProcessMemory(hProcess, BASE_PUZZLE_H2_ADDR, &height, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 4 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_MAIN_UI0_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 6", "ReadProcessMemory error", MB_OK);

                //sprintf_s(msg, "Old UI POS X Value: %d", read);
                //MessageBoxA(NULL, msg, "Main UI", MB_OK);

                int position = height - 141;
                if (!WriteProcessMemory(hProcess, BASE_MAIN_UI0_ADDR, &position, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 6 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_MAIN_UI1_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 5", "ReadProcessMemory error", MB_OK);

                //sprintf_s(msg, "Old UI POS X Value: %d", read);
                //MessageBoxA(NULL, msg, "Main UI", MB_OK);

                int position = (width - 1024) / 2;
                if (!WriteProcessMemory(hProcess, BASE_MAIN_UI1_ADDR, &position, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 5 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

#if VERSION < 5187
            if (ReadProcessMemory(hProcess, BASE_ARROW_ICON_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 6", "ReadProcessMemory error", MB_OK);

                int position = ((width - 1024) / 2) + 131;
                if (!WriteProcessMemory(hProcess, BASE_ARROW_ICON_ADDR, &position, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 6 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_ARROW_STRING_BLACK_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 7", "ReadProcessMemory error", MB_OK);

                int position = ((width - 1024) / 2) + 130;
                if (!WriteProcessMemory(hProcess, BASE_ARROW_STRING_BLACK_ADDR, &position, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 7 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (ReadProcessMemory(hProcess, BASE_ARROW_STRING_WHITE_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 8", "ReadProcessMemory error", MB_OK);

                int position = ((width - 1024) / 2) + 131;
                if (!WriteProcessMemory(hProcess, BASE_ARROW_STRING_WHITE_ADDR, &position, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 8 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }
#else
            if (ReadProcessMemory(hProcess, BASE_ARROW_ADDR, &read, 4, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 6", "ReadProcessMemory error", MB_OK);

                int position = ((width - 1024) / 2) + 131;
                if (!WriteProcessMemory(hProcess, BASE_ARROW_ADDR, &position, 4, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 6 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }
#endif

            if (ReadProcessMemory(hProcess, BASE_HELP_BUTTON_ADDR, &read, 1, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                if (bytes_read == 0)
                    MessageBoxA(NULL, "Could not get height memory offset. 9", "ReadProcessMemory error", MB_OK);

                int position = -30;
                if (!WriteProcessMemory(hProcess, BASE_HELP_BUTTON_ADDR, &position, 1, &bytes_written))
                {
                    sprintf_s(msg, "Error writing to memory! 9 %d", GetLastError());
                    MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                }
            }

            if (fpsMode != 0) 
            {
                short frameDelayMs = 0x19; // normal
                if (fpsMode == 2) // unlocked
                {
                    frameDelayMs = 0x1;
                }
                else if (fpsMode == 1) // 60 fps
                {
                    frameDelayMs = 0x10;
                }

                if (ReadProcessMemory(hProcess, BASE_FPS1_ADDR, &read, 1, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY)
                {
                    if (bytes_read == 0)
                        MessageBoxA(NULL, "Could not get height memory offset.", "ReadProcessMemory error", MB_OK);


                    if (!WriteProcessMemory(hProcess, BASE_FPS1_ADDR, &frameDelayMs, 2, &bytes_written))
                    {
                        sprintf_s(msg, "Error writing to memory! %d", GetLastError());
                        MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                    }
                }

                if (ReadProcessMemory(hProcess, BASE_FPS2_ADDR, &read, 1, &bytes_read) || GetLastError() == ERROR_PARTIAL_COPY) {
                    if (bytes_read == 0)
                        MessageBoxA(NULL, "Could not get height memory offset.", "ReadProcessMemory error", MB_OK);

                    if (!WriteProcessMemory(hProcess, BASE_FPS2_ADDR, &frameDelayMs, 2, &bytes_written))
                    {
                        sprintf_s(msg, "Error writing to memory! %d", GetLastError());
                        MessageBoxA(NULL, msg, "WriteProcessMemory error", MB_OK);
                    }
                }
            }

            CFlashFix flash;
            flash.Hook();
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