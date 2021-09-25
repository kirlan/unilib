//#######################################################################################
//## CheckTreeCtrl.h : header file
//## [Magerusan G. Cosmin] 20-apr-2002 (Created)
//## [Magerusan G. Cosmin]  5-feb-2003 (Adapted for ATL)
//#######################################################################################
#if !defined(__CHECKTREECTRL_H__)
#define __CHECKTREECTRL_H__

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
//## ====================================================================================
#define IMG_OPEN	0L
#define IMG_CLOSE	1L
#define IMG_ITEM	2L
//#######################################################################################
class CCheckTreeCtrl : public CWindowImpl < CCheckTreeCtrl , ATLControls::CTreeViewCtrlExT < CWindow > > 
{
//## CONSTRUCTOR
public:
	CCheckTreeCtrl();
	virtual ~CCheckTreeCtrl();

//## STATE
private:
	CWindow *m_pwndParentCombo;
	BOOL m_bUpdateNeeded;
	long m_bScroll;

//## MESSAGES
public:
BEGIN_MSG_MAP( CCheckTreeCtrl )
	MESSAGE_HANDLER(WM_PAINT, OnPaint)
	MESSAGE_HANDLER(WM_LBUTTONDOWN, OnLButtonDown)
	MESSAGE_HANDLER(WM_LBUTTONDBLCLK, OnLButtonDblClk)
	MESSAGE_HANDLER(WM_RBUTTONDOWN, OnRButtonDown)
	MESSAGE_HANDLER(WM_LBUTTONUP, OnLButtonUp)
	MESSAGE_HANDLER(WM_CANCELMODE, OnCancelMode)
	MESSAGE_HANDLER(WM_CAPTURECHANGED, OnCaptureChanged)
END_MSG_MAP()

//## HANDLERS
public:
	LRESULT OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonDblClk(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnRButtonDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonUp(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCancelMode(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCaptureChanged(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnDrop(BOOL bDrop); //## INTERNAL

//## UPDATE methods
private:
	void UpdateItemImage(HTREEITEM hItem);
	void RecursiveUpdateFromState(HTREEITEM hParentItem);
	void UpdateParentWnd();
public:
	void UpdateToState();
	void UpdateFromState();

//## METHODS
public:
	void Populate();
	void SetParentCombo(CWindow *pwnd){ m_pwndParentCombo = pwnd; };
	BOOL IsInside(POINT point);

//## TREE Specific
private:
	BOOL GetItemExpanded(HTREEITEM hItem);
	BOOL GetCheck(HTREEITEM hItem);
	void SetCheck(HTREEITEM hItem, BOOL bCheck);
};
//#######################################################################################
#endif // !defined(__CHECKTREECTRL_H__)
//#######################################################################################
