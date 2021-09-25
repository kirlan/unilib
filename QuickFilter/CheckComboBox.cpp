//#######################################################################################
//## CheckComboBox.cpp: implementation of the CCheckComboBox class.
//#######################################################################################
#include "stdafx.h"
#include "CheckComboBox.h"
#include "Common.h"
#include "resource.h"
#include "QuickFilter.h"
#include "QFilter.h"
#include "XPCompatible\\visualstylesxp.h"
//#######################################################################################
CCheckComboBox::CCheckComboBox()
{
	//## INITIALIZE
	m_pwndTree = NULL;
	m_nDroppedWidth = DROPPED_WIDTH_NOT_SET;
	m_bMouseDown = FALSE;
	m_pQFilter = NULL;
}
//## ====================================================================================
CCheckComboBox::~CCheckComboBox()
{
	//## HIDE ActiveDropDown
	HideDropDown();
}
//#######################################################################################
LRESULT CCheckComboBox::OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## HIDE ActiveDropDown
	HideDropDown();

	//## DESTROY Tree
	if (m_pwndTree){
		if(::IsWindow(m_pwndTree->m_hWnd))
			m_pwndTree->DestroyWindow();
		delete m_pwndTree; m_pwndTree = NULL;
	}
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnLButtonDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## SHOW Tree
	m_bMouseDown = TRUE;
	ShowDropWnd();
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnLButtonDblClk(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## SHOW Tree
	m_bMouseDown = TRUE;
	ShowDropWnd();
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnLButtonUp(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## SMOOTH Capture
	m_bMouseDown = FALSE;
	if (IsDropped()){
		HWND hWnd = GetCapture();
		if ((!hWnd) || (!::IsWindow(hWnd)) || (hWnd != m_pwndTree->m_hWnd))
			m_pwndTree->OnDrop( TRUE );
	}
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnPreKey(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## INTERCEPT Keys => send them to tree
	if ((uMsg >= WM_KEYFIRST) && (uMsg <= WM_KEYLAST))
		if(m_pwndTree)
			if(m_pwndTree->IsWindowVisible()){
				m_pwndTree->SendMessage( uMsg, wParam, lParam);
				m_pwndTree->UpdateToState();
			}
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnKeyDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## INTERCEPT
	switch ((int)wParam){
		case VK_F4:
			ShowDropWnd();
			bHandled = TRUE;
			break;
		case VK_SPACE:
			bHandled = TRUE;
	}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnSysKeyDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## INTERCEPT
	switch ((int)wParam){
		case VK_UP:
		case VK_DOWN:
			ShowDropWnd();
			bHandled = TRUE;
			break;
	}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnPreMouse(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;
	if (!::IsWindow(m_ToolTip.m_hWnd)) return 0L;

	//## PROCESS
	MSG msg;
		msg.hwnd = m_hWnd;
		msg.message = uMsg;
		msg.lParam = lParam;
		msg.wParam = wParam;
		msg.pt.x = 0;
		msg.pt.y = 0;
		msg.time = 0;
	m_ToolTip.RelayEvent( &msg );
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnKillFocus(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## HIDE DropDown
	HideDropDown();
	return 0L;
}
//## ====================================================================================
LRESULT CCheckComboBox::OnGetDlgCode(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	bHandled = TRUE;
	return DLGC_WANTARROWS | DLGC_WANTCHARS | DLGC_BUTTON;
}
//## ====================================================================================
#define DEFLATE_RECT_EX( rc, l, t, r, b )\
	rc.left += l; rc.top += t; rc.right -= r; rc.bottom -= b;
#define DEFLATE_RECT( rc, dx, dy )\
	DEFLATE_RECT_EX( rc, dx, dy, dx, dy )
#define HEIGHT_RECT( rc ) (rc.bottom - rc.top)
//## ====================================================================================
LRESULT CCheckComboBox::DrawItem( LPDRAWITEMSTRUCT lpDrawItemStruct )
{
	//## GET HDC
	HDC hDC = lpDrawItemStruct->hDC;
	if (!hDC) return 0L;

	//## GET Rect
	RECT rc = lpDrawItemStruct->rcItem;

	//## FRAME
	::DrawEdge( hDC, &rc, EDGE_SUNKEN, BF_TOP | BF_LEFT | BF_BOTTOM | BF_RIGHT );

	//## DRAW white BK
	DEFLATE_RECT( rc, 2, 2 );
	static LOGBRUSH logWindow = { BS_SOLID, GetSysColor(COLOR_WINDOW), NULL };
		static CGDIObject< HBRUSH > brWindow( CreateBrushIndirect( &logWindow ) );
	static LOGBRUSH logBtnFace = { BS_SOLID, GetSysColor(COLOR_BTNFACE) , NULL };
		static CGDIObject< HBRUSH > brBtnFace( CreateBrushIndirect( &logBtnFace ) );
	static LOGBRUSH logBtnShadow = { BS_SOLID, GetSysColor(COLOR_BTNSHADOW) , NULL };
		static CGDIObject< HBRUSH > brBtnShadow( CreateBrushIndirect( &logBtnShadow ) );
	::FillRect( hDC, &rc, (IsWindowEnabled()) ? (brWindow.m_hObj) : (brBtnFace.m_hObj) );

	//## COMPUTE Button Rect
	RECT rcButton = rc;
	rcButton.left = rcButton.right - DROP_BUTTON_WIDTH;
	if (rcButton.left < rc.left) rcButton.left = rc.left;

	//## COMPUTE Caption Rect
	RECT rcCaption = rc;
	rcCaption.right = rcButton.left - 1;
	if (rcCaption.right < rcCaption.left) rcCaption.right = rcCaption.left;

	//## FOCUS
	DEFLATE_RECT( rcCaption, 1, 1 );
	if (lpDrawItemStruct->itemState & ODS_FOCUS) ::DrawFocusRect( hDC, &rcCaption );
	DEFLATE_RECT( rcCaption, 2, 1 );

	//## GET Caption
	TCHAR buffer[SIMPLE_BUFFER_SIZE];
	long nLength = GetWindowText( buffer, SIMPLE_BUFFER_SIZE - 1 );

	//## DRAW Caption
	SetBkColor( hDC, (IsWindowEnabled()) ? GetSysColor(COLOR_WINDOW) : GetSysColor(COLOR_BTNFACE) );
	COLORREF crOldColor = SetTextColor(lpDrawItemStruct->hDC, RGB(0, 0, 0));
	DrawText( hDC, buffer, nLength, &rcCaption, DT_SINGLELINE | DT_VCENTER );
	SetTextColor( hDC, crOldColor );

	//## DRAW Button
	DrawButton( lpDrawItemStruct, rcButton );
	return 0L;
}
//## ====================================================================================
BOOL CCheckComboBox::DrawButton( LPDRAWITEMSTRUCT lpDrawItemStruct, RECT rcDropButton )
{
	//## GET HDC
	HDC hDC = lpDrawItemStruct->hDC;
	if (!hDC) return FALSE;
	RECT rcButton = rcDropButton;

	//## BRUSHES
	static LOGBRUSH logWindow = { BS_SOLID, GetSysColor(COLOR_WINDOW), NULL };
		static CGDIObject< HBRUSH > brWindow( CreateBrushIndirect( &logWindow ) );
	static LOGBRUSH logBtnFace = { BS_SOLID, GetSysColor(COLOR_BTNFACE) , NULL };
		static CGDIObject< HBRUSH > brBtnFace( CreateBrushIndirect( &logBtnFace ) );
	static LOGBRUSH logBtnShadow = { BS_SOLID, GetSysColor(COLOR_BTNSHADOW) , NULL };
		static CGDIObject< HBRUSH > brBtnShadow( CreateBrushIndirect( &logBtnShadow ) );

	//## GET Button Style
	DWORD dwBtnStyle = DFCS_BUTTONPUSH;
	if ( lpDrawItemStruct->itemState & ODS_SELECTED ) dwBtnStyle |= DFCS_PUSHED;
	if ( !IsWindowEnabled() ) dwBtnStyle |= DFCS_INACTIVE;

	//## THEME (the following code is adapted to ATL)
	BOOL bThemeActive = FALSE;
	HRESULT hr;
	static CVisualStylesXP g_xpStyle;
	bThemeActive = g_xpStyle.UseVisualStyles();

	HTHEME hTheme = NULL;
	if( bThemeActive )
		hTheme = g_xpStyle.OpenThemeData( m_hWnd, L"COMBOBOX" );

	// Theme drop btn style
	int nDropBtnThemeStyle = 0;
	if( IsDropped() ){ 
		nDropBtnThemeStyle = CBXS_PRESSED;
	}else{
		nDropBtnThemeStyle = CBXS_NORMAL;
		if ( lpDrawItemStruct->itemState & ODS_SELECTED ) nDropBtnThemeStyle = CBXS_HOT;
		if ( !IsWindowEnabled() ) nDropBtnThemeStyle = CBXS_DISABLED;
	}

	if( bThemeActive ){
		hr = g_xpStyle.DrawThemeBackground( hTheme, hDC, CP_DROPDOWNBUTTON, nDropBtnThemeStyle, &rcDropButton, NULL);
	}else{
		//## DRAW Frame
		if (lpDrawItemStruct->itemState & ODS_SELECTED){
			FrameRect( hDC, &rcButton, brBtnShadow.m_hObj );
			DEFLATE_RECT( rcButton, 1, 1 )
			FillRect( hDC, &rcButton, brBtnFace.m_hObj );
			DEFLATE_RECT_EX( rcButton, 1, 3, 0, 0 )
		}else{
			FrameRect( hDC, &rcButton, brBtnFace.m_hObj );
			DEFLATE_RECT_EX( rcButton, 1, 1, 0, 0 )
			DrawFrameControl(lpDrawItemStruct->hDC, &rcButton, DFC_BUTTON, dwBtnStyle);
		}

		//## DRAW Arrow
		static CGDIObject< HPEN > penBlack( CreatePen(PS_SOLID, 1, RGB(0, 0, 0)) );
		static CGDIObject< HPEN > penBtnShadow( CreatePen(PS_SOLID, 1, GetSysColor(COLOR_BTNSHADOW)) );
		HPEN hpenOld;
		if (IsWindowEnabled())
				hpenOld = (HPEN)SelectObject( hDC, &penBlack );
		else	hpenOld = (HPEN)SelectObject( hDC, &penBtnShadow );
		for(long i=0; i<4; i++){
			MoveToEx( hDC, rcButton.left + 3 + i, rcButton.top + HEIGHT_RECT( rcButton )/2 - 2 + i, NULL );
			LineTo( hDC, rcButton.left + 3 + 7 - i, rcButton.top + HEIGHT_RECT( rcButton )/2 - 2 + i );
			if (!IsWindowEnabled())
				SetPixel( hDC, rcButton.left + 3 + 7 - i, rcButton.top + HEIGHT_RECT( rcButton )/2 - 2 + i + 1, 0xFFFFFF );
		}
		SelectObject( hDC, hpenOld );
	}

	//## CLOSE
	if( bThemeActive )
		hr = g_xpStyle.CloseThemeData( hTheme );

	return TRUE;
}
//#######################################################################################
LRESULT CCheckComboBox::OnChange()
{
	if (m_pQFilter)
		((CQFilter*)m_pQFilter)->Fire_OnChange();
	return 0L;
}
//#######################################################################################
void CCheckComboBox::CreateDropWnd()
{
	//## ASSERT
	if (m_pwndTree) return;

	//## CREATE Tree
	RECT rc = {0, 0, 0, 0};
	m_pwndTree = new CCheckTreeCtrl;
	m_pwndTree->Create(	GetDesktopWindow(),
						rc,
						NULL,
						WS_CHILD | WS_BORDER |
						WS_CLIPSIBLINGS | WS_OVERLAPPED |
						TVS_HASBUTTONS | TVS_HASLINES | TVS_CHECKBOXES | TVS_DISABLEDRAGDROP |
						TVS_SHOWSELALWAYS | TVS_FULLROWSELECT,
						WS_EX_LEFT | WS_EX_LTRREADING | WS_EX_RIGHTSCROLLBAR | WS_EX_TOOLWINDOW);

	//## MOVE on top
	m_pwndTree->SetWindowPos(HWND_TOPMOST, 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);

	//## SET Parent
	m_pwndTree->SetParentCombo( this );

	//## ATTACH ImageList
	m_imgList.Create(IDB_TREE_ICONS, 18, 1, RGB(0xFF, 0xFF, 0xFF));
	if ((HIMAGELIST)m_imgList)
		m_pwndTree->SetImageList( (HIMAGELIST)m_imgList, TVSIL_NORMAL );

	//## POPULATE
	m_pwndTree->Populate();
}
//## ====================================================================================
void CCheckComboBox::ShowDropWnd()
{
	//## ASSERT
	if (!m_pwndTree) CreateDropWnd();
	if (!m_pwndTree) return;

	//## SHOW
	PlaceDropWnd();

	//## DROP
	Drop( !IsDropped() );
}
//## ====================================================================================
void CCheckComboBox::PlaceDropWnd()
{
	//## ASSERT
	if (!m_pwndTree) CreateDropWnd();
	if (!m_pwndTree) return;

	//## GET ComboRect
	RECT rc;
	GetWindowRect( &rc );

	//## MOVE Tree
	m_pwndTree->MoveWindow(rc.left, rc.bottom, GetDroppedWidth(), DROPDOWN_HEIGHT);
}
//## ====================================================================================
BOOL CCheckComboBox::IsDropped()
{
	//## ASSERT
	if (!m_pwndTree) return FALSE;

	//## RETURN
	return ( m_pwndTree->IsWindowVisible() );
}
//## ====================================================================================
BOOL CCheckComboBox::IsInside(POINT point)
{
	//## IS Inside the combo
	RECT rc;
	GetWindowRect( &rc );
	if ((rc.left <= point.x) && (point.x <= rc.right) &&
		(rc.top <= point.y) && (point.y <= rc.bottom)) return TRUE;

	//## NOT Inside
	return FALSE;
}
//## ====================================================================================
void CCheckComboBox::Drop(BOOL bDrop)
{
	//## ASSERT
	if (!m_pwndTree) return;

	//## DROP/UNDROP
	if (bDrop){
		//## ASSERT
		HideDropDown();

		//## STRANGE (I do not know why... but without an image list, the following lines are needed)
		if (!(HIMAGELIST)m_imgList){
			m_pwndTree->ShowWindow( SW_SHOW );
			m_pwndTree->UpdateFromState();
		}

		//## UPDATE
		m_pwndTree->UpdateFromState();

		//## SHOW Window
		m_pwndTree->ShowWindow( SW_SHOW );
		m_pwndTree->SetFocus();

		//## TOOLTIP
		SetToolTipText( "" );

		//## EVENT
		if (!m_bMouseDown)
			m_pwndTree->OnDrop( TRUE );
	}else{
		//## ASSERT
		HideDropDown();

		//## HIDE Window
		m_pwndTree->ShowWindow( SW_HIDE );

		//## CAPTION
		UpdateCaption();

		//## EVENT
		m_pwndTree->OnDrop( FALSE );
	}
}
//## ====================================================================================
void CCheckComboBox::HideDropDown()
{
	//## ASSERT
	if (!m_pwndTree) return;
	if (!m_pwndTree->m_hWnd) return;
	if (!m_pwndTree->IsWindowVisible()) return;

	//## HIDE Window
	m_pwndTree->ShowWindow( SW_HIDE );
}
//#######################################################################################
int CCheckComboBox::GetCount()
{
	return m_Data.size();
}
//## ====================================================================================
BOOL CCheckComboBox::GetCheck(long nID)
{
	return m_Data.GetCheck(nID);
}
//## ====================================================================================
CComBSTR CCheckComboBox::GetCheckedIDs()
{
	return m_Data.GetCheckedIDs();
}
//## ====================================================================================
CComBSTR	CCheckComboBox::GetCheckedTexts()
{
	return m_Data.GetCheckedTexts();
}
//## ====================================================================================
void* CCheckComboBox::GetImageList()
{
	//## ASSERT
	if ((HIMAGELIST)m_imgList) return &m_imgList;
	return NULL;
}
//## ====================================================================================
long CCheckComboBox::GetDroppedWidth()
{
	//## ASSERT
	if (m_nDroppedWidth != DROPPED_WIDTH_NOT_SET) return m_nDroppedWidth;
	if (!m_hWnd) return DROPPED_WIDTH_NOT_SET;

	//## GET ComboRect
	RECT rc;
	GetWindowRect( &rc );

	//## SET, RETURN DroppedWidth
	return (rc.right - rc.left);
}
//## ====================================================================================
void CCheckComboBox::SetCheck(long nID, BOOL bCheck)
{
	m_Data.SetCheck(nID, bCheck);
	UpdateCaption();
}
//## ====================================================================================
void CCheckComboBox::SetDroppedWidth(long nWidth)
{
	m_nDroppedWidth = nWidth;
}
//#######################################################################################
void CCheckComboBox::InitToolTip()
{
	if (m_ToolTip.m_hWnd == NULL){
		____TRY
			m_ToolTip.Create( m_hWnd );
			m_ToolTip.Activate( FALSE );
		____CATCH_SILENT
	}
}
//## ====================================================================================
void CCheckComboBox::SetToolTipText(int nId, BOOL bActivate)
{
	//## LOAD String
	CComBSTR strText;
	strText.LoadString(nId);

	//## SET Tooltip
	if (strText.Length() > 0) SetToolTipText(strText, bActivate);
}
//## ====================================================================================
void CCheckComboBox::SetToolTipText(CComBSTR strText, BOOL bActivate)
{
	//## INITIALIZE ToolTip
	InitToolTip();

	//## IF there is no tooltip defined then add it
	USES_CONVERSION;
	if (m_ToolTip.GetToolCount() == 0)
	{
		RECT rectBtn; 
		GetClientRect( &rectBtn );
		m_ToolTip.AddTool( m_hWnd, W2T(strText), &rectBtn, 1 );
	}

	//## SET text for tooltip
	m_ToolTip.UpdateTipText( W2T(strText), m_hWnd, 1 );
	m_ToolTip.Activate(bActivate);
}
//## ====================================================================================
void CCheckComboBox::UpdateCaption()
{
	//## CAPTION
	CComBSTR str = m_Data.GetCheckedTexts();
	USES_CONVERSION;
	SetWindowText( W2T(str) );

	//## ASSERT
	if (!::IsWindow(m_ToolTip.m_hWnd)) InitToolTip();
	if (!::IsWindow(m_ToolTip.m_hWnd)) return;

	//## TOOLTIP
	m_ToolTip.SetMaxTipWidth( TOOLTIP_MAX_WIDTH );
	if (str.Length() > TOOLTIP_MAX_CHARACTERS){
		str.m_str[ TOOLTIP_MAX_CHARACTERS-3 ] = '\0'; //## UGLY Mid(0, (TOOLTIP_MAX_CHARACTERS-3))
		CComBSTR strNew = (OLECHAR*)str.m_str;
		str.m_str[ TOOLTIP_MAX_CHARACTERS-3 ] = ' ';
		strNew += "...";
		str = strNew;
	}
	SetToolTipText( str );
}
//#######################################################################################
void CCheckComboBox::AddString(LPWSTR lpszString, long nID, long nLevel)
{
	//## IF is the first item => add root
	if (m_Data.size() == 0)
		m_Data.AddString(lpszString, INVALID_ID, ROOT_LEVEL);

	//## ADD
	m_Data.AddString(lpszString, nID, nLevel);	
}
//## ====================================================================================
void CCheckComboBox::CheckAll(BOOL bCheck)
{
	m_Data.CheckAll(bCheck);
	UpdateCaption();
}
//#######################################################################################
