//#######################################################################################
//## CheckComboBox.h : header file
//## [Magerusan G. Cosmin] 20-apr-2002 (Created)
//## [Magerusan G. Cosmin]  5-feb-2003 (Adapted for ATL)
//#######################################################################################
#if !defined(__CHECKCOMBOCTRL_H__)
#define __CHECKCOMBOCTRL_H__
//## ====================================================================================
#include "CheckTreeCtrl.h"
#include "CheckTreeData.h"
//## ====================================================================================
#define DROP_BUTTON_WIDTH 16
#define DROPDOWN_HEIGHT 220
#define TOOLTIP_MAX_WIDTH 350
#define TOOLTIP_MAX_CHARACTERS 50*20
#define DROPPED_WIDTH_NOT_SET -1
//#######################################################################################
class CCheckComboBox : public CWindowImpl < CCheckComboBox, ATLControls::CButtonT < CWindow > >
{
//## CONSTRUCTOR
public:
	CCheckComboBox();
	virtual ~CCheckComboBox();
	friend class CCheckTreeCtrl;

//## STATE
private:
	CCheckTreeCtrl*							m_pwndTree;
	CCheckTreeData							m_Data;
	ATLControls::CImageList					m_imgList;
	ATLControls::CToolTipCtrlT < CWindow > 	m_ToolTip;
	long									m_nDroppedWidth;
	BOOL									m_bMouseDown;
	void*									m_pQFilter;
	CComBSTR								m_bstrField;

//## MESSAGES
public:
BEGIN_MSG_MAP( CCheckComboBox )
	MESSAGE_RANGE_HANDLER(WM_KEYFIRST, WM_KEYLAST, OnPreKey)
	MESSAGE_RANGE_HANDLER(WM_MOUSEFIRST, WM_MOUSELAST, OnPreMouse)

	MESSAGE_HANDLER(WM_DESTROY, OnDestroy)
	MESSAGE_HANDLER(WM_LBUTTONDOWN, OnLButtonDown)
	MESSAGE_HANDLER(WM_LBUTTONDBLCLK, OnLButtonDblClk)
	MESSAGE_HANDLER(WM_LBUTTONUP, OnLButtonUp)
	MESSAGE_HANDLER(WM_KILLFOCUS, OnKillFocus)
	MESSAGE_HANDLER(WM_GETDLGCODE, OnGetDlgCode)
	MESSAGE_HANDLER(WM_KEYDOWN, OnKeyDown)
	MESSAGE_HANDLER(WM_SYSKEYDOWN, OnSysKeyDown)
END_MSG_MAP()

//## HANDLERS
public:
	LRESULT OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonDblClk(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnLButtonUp(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnKillFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnGetDlgCode(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPreKey(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnPreMouse(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnKeyDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSysKeyDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT DrawItem( LPDRAWITEMSTRUCT lpDrawItemStruct );
	LRESULT OnChange();

//## PROPERTIES
public:
	int				GetCount();
	BOOL			GetCheck(long nID);
	CComBSTR		GetCheckedIDs();
	CComBSTR		GetCheckedTexts();
	void*			GetImageList();
	long			GetDroppedWidth();
	CCheckTreeData* GetData(){ return &m_Data; };
	CComBSTR		GetField(){ return m_bstrField; };

	void			SetCheck(long nID, BOOL bCheck);
	void			SetDroppedWidth(long nWidth);
	void			SetQFilter(void* pQFilter){ m_pQFilter = pQFilter; };
	void			SetField(CComBSTR bstrField){ m_bstrField = bstrField; };

//## TREE Methods
public:
	void AddString(LPWSTR lpszString, long nID = INVALID_ID, long nLevel = ROOT_LEVEL + 1);
	void CheckAll(BOOL bCheck);

//## DROP Methods
private:
	void CreateDropWnd();
	void ShowDropWnd();
	void PlaceDropWnd();
	BOOL IsDropped();
	BOOL IsInside(POINT point);
	void Drop(BOOL bDrop);
	void HideDropDown();
	BOOL DrawButton( LPDRAWITEMSTRUCT lpDrawItemStruct, RECT rcDropButton );

//## TOOLTIP & CAPTION Methods
private:
	void InitToolTip();
	void SetToolTipText(CComBSTR bstrText, BOOL bActivate = TRUE);
	void SetToolTipText(int nId, BOOL bActivate = TRUE);
	void UpdateCaption();
};
//#######################################################################################
#endif // !defined(__CHECKCOMBOCTRL_H__)
//#######################################################################################
