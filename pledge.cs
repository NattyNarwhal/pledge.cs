/*
 * Copyright (c) 2016 Calvin Buckley <calvin@openmailbox.org>
 *
 * Permission to use, copy, modify, and distribute this software for any
 * purpose with or without fee is hereby granted, provided that the above
 * copyright notice and this permission notice appear in all copies.
 *
 * THE SOFTWARE IS PROVIDED "AS IS" AND THE AUTHOR DISCLAIMS ALL WARRANTIES
 * WITH REGARD TO THIS SOFTWARE INCLUDING ALL IMPLIED WARRANTIES OF
 * MERCHANTABILITY AND FITNESS. IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR
 * ANY SPECIAL, DIRECT, INDIRECT, OR CONSEQUENTIAL DAMAGES OR ANY DAMAGES
 * WHATSOEVER RESULTING FROM LOSS OF USE, DATA OR PROFITS, WHETHER IN AN
 * ACTION OF CONTRACT, NEGLIGENCE OR OTHER TORTIOUS ACTION, ARISING OUT OF
 * OR IN CONNECTION WITH THE USE OR PERFORMANCE OF THIS SOFTWARE.
 */

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
        /// <exception cref="PlatformNotSupportedException">
        /// Thrown if the the current OS isn't OpenBSD.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// Thrown if pledge returns an error.
        /// </exception>
        public static void Init(string promises, string[] paths)
        {
            // check for if it's not unix, not openbsd, not openbsd 5.9
            if (Environment.OSVersion.Platform != PlatformID.Unix
                || !IsOpenBSD()
                || Environment.OSVersion.Version < new Version(5, 9))
            {
                throw new PlatformNotSupportedException("pledge(2) is only supported by OpenBSD 5.9 or later.");
            }
            if (pledge(promises, paths) == -1)
            {
                Errno e = (Errno)Marshal.GetLastWin32Error();
                switch (e)
                {
                    case Errno.E2BIG:
                        throw new Win32Exception((int)e, "The paths array is too large.");
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
        /// <exception cref="PlatformNotSupportedException">
        /// Thrown if the the current OS isn't OpenBSD.
        /// </exception>
        /// <exception cref="Win32Exception">
        /// Thrown if pledge returns an error.
        /// </exception>
        public static void Init(string promises)
        {
            Init(promises, null);
        }
    }
}
