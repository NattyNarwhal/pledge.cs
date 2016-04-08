This is a library that allows C# and other .NET languages to use OpenBSD's
pledge(2) syscall to restrict their permissions. If used on other platforms,
the library will simply throw a PlatformNotSupportedException, which can be
caught and safely ignored. On the event the pledge call fails, the library
will throw Win32Exceptions, which again can be caught by your application,
and you can fail as needed.

Mono seems to require the promises "vminfo cpath" at a minimum, otherwise,
it'll crash on exit.

# Building info:

The only dependencies are Mono (4.2.1 from packages on OpenBSD 5.9 works
fine - it'll very likely work on older versions of Mono, possibly by
changing the compiler.) and `Mono.Posix`.

Just type `make`. The library and documentation will be created, ready to
install into the GAC or for distribution. In addition, XML documentation
is created as well.

# Using

After it's built, you can simply link in `pledge.dll`. `pledgetest.cs`
contains usage examples and handling the exceptions it throws in the event
your code isn't running on a new enough version of OpenBSD or it pledge
itself encounters an error.

# Licensing

The license is the same as OpenBSD's. See the comments in the source.

# Testing 

Typing `make test` will create a small executable to test pledge, and
execute it to see if it's working. You can then use `mono pledgetest.exe`
and pass it promises to execute with those promises. You should be able to
see its messages as it outputs to stdout unpledged, pledges, and then outputs
to stdio again, with pledge.

# TODO

* It would be nice if we could have some kind of type safety for promises,
  and guarantee the minimum for the runtime.
