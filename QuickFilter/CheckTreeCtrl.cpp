//#######################################################################################
//## CheckTreeCtrl.cpp : implementation file
//## [Magerusan G. Cosmin] 20-apr-2002 (Created)
//## [Magerusan G. Cosmin]  5-feb-2003 (Adapted for ATL)
//#######################################################################################
#include "stdafx.h"
#include "CheckTreeCtrl.h"
#include "CheckComboBox.h"
//#######################################################################################
CCheckTreeCtrl::CCheckTreeCtrl()
{
	//## INITIALIZE
	m_pwndParentCombo = NULL;
	m_bUpdateNeeded = FALSE;
	m_bScroll = FALSE;
}
//## ====================================================================================
CCheckTreeCtrl::~CCheckTreeCtrl()
{
}
//#######################################################################################
void CCheckTreeCtrl::Populate()
{
	//## ASSERT
	if (!m_hWnd) return;
	DeleteAllItems();

	//## GET Data
	CCheckTreeData* pTreeData = ((CCheckComboBox*)m_pwndParentCombo)->GetData();
	BOOL bImageList = (GetImageList(TVSIL_NORMAL) != NULL);

	//## INSERT ROOT
	HTREEITEM hRoot;
	if (bImageList)
			hRoot = InsertItem(ROOT_CAPTION, IMG_OPEN, IMG_OPEN, TVI_ROOT, TVI_LAST);
	else	hRoot = InsertItem(ROOT_CAPTION, TVI_ROOT, TVI_LAST);
	SetItemData(hRoot, ROOT_INDEX);

	//## ADD rest of the items
	USES_CONVERSION;
	HTREEITEM hParents[TREE_MAX_LEVELS];
	hParents[ROOT_LEVEL] = hRoot;
	long nLevel; BOOL bIsLeaf;
	for(long i=0+1; i<(long)pTreeData->size(); i++){
		nLevel = pTreeData->at(i).nLevel;
		bIsLeaf = pTreeData->at(i).bIsLeaf;
		if (bImageList)
				hParents[ nLevel ] = InsertItem( W2T(pTreeData->at(i).strCaption), (bIsLeaf) ? IMG_ITEM : IMG_CLOSE, (bIsLeaf) ? IMG_ITEM : IMG_CLOSE, hParents[ nLevel - 1 ], TVI_LAST );
		else	hParents[ nLevel ] = InsertItem( W2T(pTreeData->at(i).strCaption), hParents[ nLevel - 1 ], TVI_LAST );
		SetItemData(hParents[ nLevel ], i);
	}

	//## EXPAND Root
	this->Expand(hRoot, TVE_EXPAND);
}
//#######################################################################################
LRESULT CCheckTreeCtrl::OnPaint(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## UPDATE to state
	if (m_bUpdateNeeded){
		m_bUpdateNeeded = FALSE;
		UpdateToState();
	}
	return 0L;
}
//## ====================================================================================
#define GET_X_LPARAM(lp)                        ((int)(short)LOWORD(lp))
#define GET_Y_LPARAM(lp)                        ((int)(short)HIWORD(lp))
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnLButtonDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## CONVERT
	POINT point;
		point.x = GET_X_LPARAM( lParam );
		point.y = GET_Y_LPARAM( lParam );
	ClientToScreen( &point );
	HWND hWnd = WindowFromPoint( point );

	//## ASSERT
	if (!IsInside(point)){
		if (m_pwndParentCombo){
			//## CANCEL
			((CCheckComboBox*)m_pwndParentCombo)->Drop( FALSE );

			//## GO ON
			if (!m_pwndParentCombo) return 0L;
			if (((CCheckComboBox*)m_pwndParentCombo)->IsInside(point)) return 0L;
			if (hWnd){
				::ScreenToClient( hWnd, &point );
				::SendMessage( hWnd, WM_LBUTTONDOWN, MK_LBUTTON, MAKELONG(point.x, point.y));
			}
			return 0L;						//## INTERCEPTED
		}
	}

	//## RELEASE Capture
	if (hWnd == m_hWnd)
		if (GetCapture() == hWnd){
			LRESULT result = ::SendMessage( hWnd, WM_NCHITTEST, NULL, MAKELONG(point.x, point.y));
			if ((result == HTVSCROLL) || (result == HTHSCROLL)){
				::ReleaseCapture();
				m_bScroll = TRUE;
				::SendMessage( hWnd, WM_NCLBUTTONDOWN, result, MAKELONG(point.x, point.y));
			}
		}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnLButtonDblClk(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## CONVERT
	POINT point;
		point.x = GET_X_LPARAM( lParam );
		point.y = GET_Y_LPARAM( lParam );
	ClientToScreen( &point );
	HWND hWnd = WindowFromPoint( point );

	//## RELEASE Capture
	if (hWnd == m_hWnd)
		if (GetCapture() == hWnd){
			LRESULT result = ::SendMessage( hWnd, WM_NCHITTEST, NULL, MAKELONG(point.x, point.y));
			if ((result == HTVSCROLL) || (result == HTHSCROLL)){
				::ReleaseCapture();
				m_bScroll = TRUE;
				::SendMessage( hWnd, WM_NCLBUTTONDOWN, result, MAKELONG(point.x, point.y));
			}
		}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnRButtonDown(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## DO NOT CALL Default
	bHandled = TRUE;

	//## CONVERT
	POINT point;
		point.x = GET_X_LPARAM( lParam );
		point.y = GET_Y_LPARAM( lParam );
	ClientToScreen( &point );
	HWND hWnd = WindowFromPoint( point );

	//## ASSERT
	if (!IsInside(point)){
		if (m_pwndParentCombo){
			//## CANCEL
			((CCheckComboBox*)m_pwndParentCombo)->Drop( FALSE );

			//## GO ON
			if (!m_pwndParentCombo) return 0L;
			if (((CCheckComboBox*)m_pwndParentCombo)->IsInside(point)) return 0L;
			if (hWnd){
				::ScreenToClient( hWnd, &point );
				::SendMessage( hWnd, WM_RBUTTONDOWN, MK_LBUTTON, MAKELONG(point.x, point.y));
			}
			return 0L;						//## INTERCEPTED
		}
	}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnLButtonUp(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## UPDATE Needed
	m_bUpdateNeeded = TRUE;

	//## UPDATE to state
	UpdateToState();

	//## SET Capture
	SetCapture();
	return 0L;
}
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnCancelMode(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## CANCEL
	if (IsWindowVisible())
		if (m_pwndParentCombo)
			((CCheckComboBox*)m_pwndParentCombo)->Drop( FALSE );
	return 0L;
}
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnCaptureChanged(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## SET Capture
	if (IsWindowVisible())
		if (lParam == NULL)
			if (m_bScroll){
				SetCapture();
				m_bScroll = FALSE;
			}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CCheckTreeCtrl::OnDrop(BOOL bDrop)
{
	//## CAPTURE or RELEASE
	if (bDrop){
		SetCapture();
	}else{
		ReleaseCapture();
	}

	//## RETURN
	return 0L;
}
//#######################################################################################
BOOL CCheckTreeCtrl::IsInside(POINT point)
{
	//## IS Inside the combo
	RECT rc;
	GetWindowRect( &rc );
	if ((rc.left <= point.x) && (point.x <= rc.right) &&
		(rc.top <= point.y) && (point.y <= rc.bottom)) return TRUE;

	//## NOT Inside
	return FALSE;
}
//#######################################################################################
void CCheckTreeCtrl::UpdateToState()
{
	//## ASSERT
	if (!m_pwndParentCombo) return;

	//## DECLARE
	long nIndex; BOOL bChecked;

	//## GET Data
	CCheckTreeData* pTreeData = ((CCheckComboBox*)m_pwndParentCombo)->GetData();
	if (!pTreeData) return;
	if (!pTreeData->size()) return;

	//## SCAN TREE
	HTREEITEM hItem = GetNextItem(TVI_ROOT, TVGN_FIRSTVISIBLE);
	while(hItem){
		//## GET ItemData = Index
		nIndex = GetItemData(hItem);

		//## UPDATE Image if needed
		if ((nIndex >= 0) && (!pTreeData->at(nIndex).bIsLeaf))
			UpdateItemImage(hItem);

		//## GET Checked. Modification => update & exit
		bChecked = GetCheck(hItem);
		if ((nIndex >= 0) && (CLEAN_BOOL(pTreeData->at(nIndex).bChecked) != CLEAN_BOOL(bChecked))){
			//## STATE
			pTreeData->at(nIndex).bChecked = bChecked;
			pTreeData->UpdateChecks(nIndex);
			UpdateFromState();
			UpdateParentWnd();
			Invalidate();

			//## EVENT
			((CCheckComboBox*)m_pwndParentCombo)->OnChange();
			return;
		}
	
		//## GO ON
		hItem = GetNextItem(hItem, TVGN_NEXTVISIBLE);
	}
}
//#######################################################################################
BOOL CCheckTreeCtrl::GetItemExpanded(HTREEITEM hItem)
{
	//## ASSERT
	if (!hItem) return FALSE;

	//## RETURN Expanded
	return ((GetItemState(hItem, TVIS_EXPANDED) & TVIS_EXPANDED) != 0L);
}
//## ====================================================================================
BOOL CCheckTreeCtrl::GetCheck(HTREEITEM hItem)
{
	ATLASSERT(::IsWindow(m_hWnd));
	TVITEM item;
	item.mask = TVIF_HANDLE | TVIF_STATE;
	item.hItem = hItem;
	item.stateMask = TVIS_STATEIMAGEMASK;
	SendMessage(m_hWnd, TVM_GETITEM, 0, (LPARAM)&item);
	return ((BOOL)(item.state >> 12) - 1);
}
//## ====================================================================================
void CCheckTreeCtrl::SetCheck(HTREEITEM hItem, BOOL bCheck)
{
	ATLASSERT(::IsWindow(m_hWnd));
	TVITEM item;
	item.mask = TVIF_HANDLE | TVIF_STATE;
	item.hItem = hItem;
	item.stateMask = TVIS_STATEIMAGEMASK;
	item.state = INDEXTOSTATEIMAGEMASK((bCheck ? 2 : 1));
	SendMessage(m_hWnd, TVM_SETITEM, 0, (LPARAM)&item);
}
//## ====================================================================================
void CCheckTreeCtrl::UpdateItemImage(HTREEITEM hItem)
{
	//## ASSERT
	if (!hItem) return;

	//## DECLARE
	BOOL bExpanded;
	int nImage, nSelectedImage;

	//## GET Expanded + Image
	bExpanded = GetItemExpanded(hItem);
	GetItemImage(hItem, nImage, nSelectedImage);

	//## UPDTE Image if necessary
	if (bExpanded && (nImage == IMG_CLOSE))
		SetItemImage(hItem, IMG_OPEN, IMG_OPEN);
	if (!bExpanded && (nImage == IMG_OPEN))
		SetItemImage(hItem, IMG_CLOSE, IMG_CLOSE);
}
//#######################################################################################
void CCheckTreeCtrl::UpdateFromState()
{
	//## ASSERT
	if (!m_pwndParentCombo) return;

	//## GET Data
	CCheckTreeData* pTreeData = ((CCheckComboBox*)m_pwndParentCombo)->GetData();
	if (pTreeData->size() <= 0) return;

	//## GET Root & RootIndex
	HTREEITEM hItem = GetRootItem();
	long nIndex = GetItemData(hItem);

	//## SET Check
	SetCheck( hItem, pTreeData->at(nIndex).bChecked );

	//## SELECT
	HTREEITEM hItemToSelect = GetNextItem(TVI_ROOT, TVGN_CARET);
	if (!hItemToSelect) hItemToSelect = hItem;
	SelectItem( hItemToSelect );
	
	//## RECURSIVE
	RecursiveUpdateFromState(hItem);
}
//## ====================================================================================
void CCheckTreeCtrl::RecursiveUpdateFromState(HTREEITEM hParentItem)
{
	//## ASSERT
	if (!ItemHasChildren(hParentItem)) return;

	//## GET Data
	CCheckTreeData* pTreeData = ((CCheckComboBox*)m_pwndParentCombo)->GetData();

	//## SCAN
	long nIndex = 0L;
	HTREEITEM hItem = GetChildItem(hParentItem);
	while(hItem){
		//## GET ItemData = Index
		nIndex = GetItemData(hItem);

		//## SET Check
		SetCheck( hItem, pTreeData->at(nIndex).bChecked );

		//## RECURSIVE
		RecursiveUpdateFromState(hItem);

		//## GO ON
		hItem = GetNextItem(hItem, TVGN_NEXT);
	}	
}
//## ====================================================================================
void CCheckTreeCtrl::UpdateParentWnd()
{
	//## ASSERT
	if (!m_pwndParentCombo) return;

	//## GET Data
	CCheckTreeData* pTreeData = ((CCheckComboBox*)m_pwndParentCombo)->GetData();
	if (!pTreeData) return;

	//## SET Caption
	USES_CONVERSION;
	m_pwndParentCombo->SetWindowText( W2T(pTreeData->GetCheckedTexts()) );
}
//#######################################################################################
