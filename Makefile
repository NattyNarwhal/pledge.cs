MCS=mcs
MONO=mono

pledge.dll: pledge.dll
	$(MCS) -t:library -doc:pledge.dll.xml -r:Mono.Posix -o:pledge.dll pledge.cs

pledgetest.exe: pledge.dll
	$(MCS) -r:pledge.dll -o:pledgetest.exe pledgetest.cs

test: pledgetest.exe
	$(MONO) pledgetest.exe stdio vminfo cpath

clean:
	rm *.exe *.dll *.xml *.core
