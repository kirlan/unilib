
QuickFilterps.dll: dlldata.obj QuickFilter_p.obj QuickFilter_i.obj
	link /dll /out:QuickFilterps.dll /def:QuickFilterps.def /entry:DllMain dlldata.obj QuickFilter_p.obj QuickFilter_i.obj \
		kernel32.lib rpcndr.lib rpcns4.lib rpcrt4.lib oleaut32.lib uuid.lib \

.c.obj:
	cl /c /Ox /DWIN32 /D_WIN32_WINNT=0x0400 /DREGISTER_PROXY_DLL \
		$<

clean:
	@del QuickFilterps.dll
	@del QuickFilterps.lib
	@del QuickFilterps.exp
	@del dlldata.obj
	@del QuickFilter_p.obj
	@del QuickFilter_i.obj
