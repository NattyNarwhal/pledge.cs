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
using System.ComponentModel;
using Mono.Unix.Native;

namespace OpenBSD
{
    public class PledgeTest
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Pledging...");
            try 
            {
                Pledge.Init(string.Join(" ", args));
                Console.WriteLine("Pledged!");
            }
            catch (Win32Exception e)
            {
                Console.WriteLine("pledge(2) encountered an error: {0}\n\t{1}",
                    (Errno)e.NativeErrorCode, e.Message);
            }
            catch
            {
                Console.WriteLine("Your system ({0}) is not supported.",
                    Environment.OSVersion);
            }
        }
    }
}
