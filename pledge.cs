using System;
using System.Runtime.InteropServices;
using System.ComponentModel;
using Mono.Unix.Native;

namespace OpenBSD
{
    /// <summary>
    /// Uses OpenBSD's pledge(2) syscall to reduce process privleges.
    /// </summary>
    /// <remarks>
    /// Consult the OpenBSD manual page: http://man.openbsd.org/OpenBSD-current/man2/pledge.2
    /// </remarks>
    public static class Pledge
    {
        [DllImport("libc.so", SetLastError = true, CharSet = CharSet.Auto)]
        private static extern int pledge(string promises, string[] paths);

        private static bool IsOpenBSD()
        {
            if (Environment.OSVersion.Platform != PlatformID.Unix)
                return false;
            Utsname uname;
            Syscall.uname(out uname);
            return uname.sysname == "OpenBSD";
        }

        /// <summary>
        /// Use OpenBSD's pledge(2) syscall to reduce process privleges.
        /// </summary>
        /// <param name="promises">The list of privleges to drop to.</param>
        /// <param name="paths">The list of paths to allow access to.</param>
        public static void Init(string promises, string paths)
        {
            // check for if it's not unix, not openbsd, not openbsd 5.9
            if (Environment.OSVersion.Platform != PlatformID.Unix
                || !IsOpenBSD()
                || Environment.OSVersion.Version < new Version(5, 9))
            {
                throw new PlatformNotSupportedException("pledge(2) is only supported by OpenBSD.");
            }
            if (pledge(promises, null) == -1)
            {
                Errno e = (Errno)Marshal.GetLastWin32Error();
                switch (e)
                {
                    case Errno.E2BIG:
                        throw new Win32Exception((int)e, "The paths array is tooo large.");
                    case Errno.EINVAL:
                        throw new Win32Exception((int)e, "The promises are malformed or invalid.");
                    case Errno.EPERM:
                        throw new Win32Exception((int)e, "The process is trying to increase permissions.");
                    case Errno.ENAMETOOLONG:
                        throw new Win32Exception((int)e, "A path or the promises are too long.");
                    case Errno.EFAULT:
                        throw new Win32Exception((int)e, "The paths or promises point outside of memory.");
                    default:
                        throw new Win32Exception((int)e, "The system has thrown an unknown error.");
                }
            }
        }

        /// <summary>
        /// Uses OpenBSD's pledge(2) syscall to reduce process privleges.
        /// </summary>
        /// <param name="promises">The list of privleges to drop to.</param>
        public static void Init(string promises)
        {
            Init(promises, null);
        }
    }
}
