//#######################################################################################
//## AboutDlg.h :	Declaration of the CAboutDlg
//##				Based on a class found in a project made by Brent Rector
//##				Adapted by Magerusan Grigore Cosmin
//#######################################################################################
//##       "Better [is] the end of a thing than the beginning thereof: [and] the 
//##    patient in spirit [is] better than the proud in spirit."
//##                                                      Ecclesiastes 7:8 
//##
//##    Maranatha Jesus! Amen!
//#######################################################################################
#ifndef __ABOUTDLG_H_
#define __ABOUTDLG_H_

#include "resource.h"       // main symbols
//#######################################################################################
class CAboutDlg : public CAxDialogImpl<CAboutDlg>
{
//## CONSTRUCTOR
public:
	CAboutDlg();
	virtual ~CAboutDlg();

	enum { IDD = IDD_ABOUTDLG };

//## MESSAGES
BEGIN_MSG_MAP(CAboutDlg)
	MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
	COMMAND_ID_HANDLER(IDOK, OnOK)
	COMMAND_ID_HANDLER(IDCANCEL, OnCancel)
END_MSG_MAP()

//## METHODS
private:
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnOK(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnCancel(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
};
//#######################################################################################
#endif //__ABOUTDLG_H_
//#######################################################################################
