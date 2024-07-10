using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace DCSGlobal.BusinessRules.CoreLibraryII
{



    class HandleInfo
    {
        [DllImport("ntdll.dll", CharSet = CharSet.Auto)]
        public static extern uint NtQuerySystemInformation(int SystemInformationClass, IntPtr SystemInformation, int SystemInformationLength, out int ReturnLength);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr VirtualAlloc(IntPtr address, uint numBytes, uint commitOrReserve, uint pageProtectionMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool VirtualFree(IntPtr address, uint numBytes, uint pageFreeMode);

        [StructLayout(LayoutKind.Sequential)]
        public struct SYSTEM_HANDLE_INFORMATION
        {
            public int ProcessId;
            public byte ObjectTypeNumber;
            public byte Flags; // 1 = PROTECT_FROM_CLOSE, 2 = INHERIT
            public short Handle;
            public int Object;
            public int GrantedAccess;
        }

        static uint MEM_COMMIT = 0x1000;
        static uint PAGE_READWRITE = 0x04;
        static uint MEM_DECOMMIT = 0x4000;
        static int SystemHandleInformation = 16;
        static uint STATUS_INFO_LENGTH_MISMATCH = 0xC0000004;

        public HandleInfo()
        {
            IntPtr memptr = VirtualAlloc(IntPtr.Zero, 100, MEM_COMMIT, PAGE_READWRITE);

            int returnLength = 0;
            bool success = false;

            uint result = NtQuerySystemInformation(SystemHandleInformation, memptr, 100, out returnLength);
            if (result == STATUS_INFO_LENGTH_MISMATCH)
            {
                success = VirtualFree(memptr, 0, MEM_DECOMMIT);
                memptr = VirtualAlloc(IntPtr.Zero, (uint)(returnLength + 256), MEM_COMMIT, PAGE_READWRITE);
                result = NtQuerySystemInformation(SystemHandleInformation, memptr, returnLength, out returnLength);
            }

            int handleCount = Marshal.ReadInt32(memptr);
            SYSTEM_HANDLE_INFORMATION[] returnHandles = new SYSTEM_HANDLE_INFORMATION[handleCount];

            using (StreamWriter sw = new StreamWriter(@"C:\NtQueryDbg.txt"))
            {
                sw.WriteLine("@ Offset\tProcess Id\tHandle Id\tHandleType");
                for (int i = 0; i < handleCount; i++)
                {
                    SYSTEM_HANDLE_INFORMATION thisHandle = (SYSTEM_HANDLE_INFORMATION)Marshal.PtrToStructure(
                        new IntPtr(memptr.ToInt32() + 4 + i * Marshal.SizeOf(typeof(SYSTEM_HANDLE_INFORMATION))),
                        typeof(SYSTEM_HANDLE_INFORMATION));
                    sw.WriteLine("{0}\t{1}\t{2}\t{3}", i.ToString(), thisHandle.ProcessId.ToString(), thisHandle.Handle.ToString(), thisHandle.ObjectTypeNumber.ToString());
                }
            }

            success = VirtualFree(memptr, 0, MEM_DECOMMIT);
        }
    }
}


