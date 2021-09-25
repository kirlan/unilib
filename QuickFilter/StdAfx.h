//#######################################################################################
// stdafx.h
//#######################################################################################
#if !defined(AFX_STDAFX_H__9B93E206_85BB_4A5E_A361_CBBA554AF433__INCLUDED_)
#define AFX_STDAFX_H__9B93E206_85BB_4A5E_A361_CBBA554AF433__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000

#define STRICT
#ifndef _WIN32_WINNT
#define _WIN32_WINNT 0x0403
#endif
#define _ATL_APARTMENT_THREADED
#define _ATL_DEBUG_QI
#define _ATL_DEBUG_INTERFACES

#include <atlbase.h>
//You may derive a class from CComModule and use it if you want to override
//something, but do not change the name of _Module
extern CComModule _Module;
#include <atlcom.h>
#include <atlhost.h>
#include <atlctl.h>

//## MACROS BEGIN
#include "Macros.h"
#include "MacrosPP.h"
#include "Util.h"
//## MACROS END

//## OTHER BEGIN
#include "ATLControls.h"
#include <vector>
//## OTHER END

//{{AFX_INSERT_LOCATION}}
// Microsoft Visual C++ will insert additional declarations immediately before the previous line.
//#######################################################################################
#endif // !defined(AFX_STDAFX_H__9B93E206_85BB_4A5E_A361_CBBA554AF433__INCLUDED)
//#######################################################################################
