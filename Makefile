MCS=mcs
MONO=mono

pledge.dll: pledge.cs
	$(MCS) -t:library -doc:pledge.dll.xml -r:Mono.Posix -o:pledge.dll pledge.cs

pledgetest.exe: pledge.dll pledgetest.cs
	$(MCS) -r:pledge.dll -r:Mono.Posix -o:pledgetest.exe pledgetest.cs

test: pledgetest.exe
	$(MONO) pledgetest.exe stdio vminfo cpath

clean:
	rm -f *.exe *.dll *.xml *.core
