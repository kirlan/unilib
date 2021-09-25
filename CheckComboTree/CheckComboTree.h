// CheckComboTree.h : main header file for the CheckComboTree DLL
//

#pragma once

#ifndef __AFXWIN_H__
	#error "include 'stdafx.h' before including this file for PCH"
#endif

#include "resource.h"		// main symbols


// CCheckComboTreeApp
// See CheckComboTree.cpp for the implementation of this class
//

class CCheckComboTreeApp : public CWinApp
{
public:
	CCheckComboTreeApp();

// Overrides
public:
	virtual BOOL InitInstance();

	DECLARE_MESSAGE_MAP()
};
