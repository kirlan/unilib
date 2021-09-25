//#######################################################################################
//## QFilter.cpp : Implementation of CQFilter
//## [Magerusan G. Cosmin]  5-feb-2003 (Created)
//#######################################################################################
#include "stdafx.h"
#include "QuickFilter.h"
#include "QFilter.h"
//## ====================================================================================
CQFilter::CQFilter()
{
	//## INITIALIZE
	m_bWindowOnly = TRUE;
	CalcExtent(m_sizeExtent);

	//## RESET Properties
	ResetProperties();
}
//## ====================================================================================
CQFilter::~CQFilter()
{
}
//## ====================================================================================
void CQFilter::ResetProperties()
{
	//## MISC Properties
	m_nNoOfCheckFilters = 5;
	m_nNoOfColumns = fil2Columns;
}
//## ====================================================================================
STDMETHODIMP CQFilter::InterfaceSupportsErrorInfo(REFIID riid)
{
	static const IID* arr[] = 
	{
		&IID_IQFilter,
	};
	for (int i=0; i<sizeof(arr)/sizeof(arr[0]); i++)
	{
		if (InlineIsEqualGUID(*arr[i], riid))
			return S_OK;
	}
	return S_FALSE;
}
//## ====================================================================================
STDMETHODIMP CQFilter::OnAmbientPropertyChange(DISPID dispid)
{
	if (dispid == DISPID_AMBIENT_BACKCOLOR)
	{
		SetBackgroundColorFromAmbient();
		FireViewChange();
	}
	return IOleControlImpl<CQFilter>::OnAmbientPropertyChange(dispid);
}
//#######################################################################################
void CQFilter::AssertFilter()
{
	//## ASSERT
	if (!m_hWnd) return;

____TRY
	//## INITIALIZE Properties
	OnPutProperty( DISPID_QFILTER_NOOFCHECKFILTERS, &m_nNoOfCheckFilters );

____CATCH( TEXT("function AssertFilter()") )
}
//#######################################################################################
LRESULT CQFilter::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## ASSERT
	AssertFilter();

	//## REGISTER CommonControls (DateTimePicker)
	INITCOMMONCONTROLSEX icc;
	icc.dwSize = sizeof(INITCOMMONCONTROLSEX);
	icc.dwICC = ICC_DATE_CLASSES;
	InitCommonControlsEx( &icc );

	//## CREATE Controls
	for (long i=0; i<(long)m_vecCheckFilters.size(); i++){
		CreateCheckFilter( m_vecCheckFilters.at(i), i );
		CreateCheckLabel( m_vecCheckLabels.at(i), i );
	}

	//## REFRESH LAYOUT
	BOOL b = TRUE;
	OnSize(NULL, NULL, NULL, b);

	//## RETURN
	return 0;
}
//## ====================================================================================
LRESULT CQFilter::OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## CALL Default
	bHandled = FALSE;

	//## DESTROY Controls
	for (long i=0; i<(long)m_vecCheckFilters.size(); i++){
		if (m_vecCheckFilters.at(i)){
			m_vecCheckFilters.at(i)->DestroyWindow();
			delete m_vecCheckFilters.at(i);
		}
		if (m_vecCheckLabels.at(i)){
			m_vecCheckLabels.at(i)->DestroyWindow();
			delete m_vecCheckLabels.at(i);
		}
	}

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CQFilter::OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## ASSERT
	ATLASSERT( fil2Columns == 2 );
	ATLASSERT( fil3Columns == 3 );
	ATLASSERT( fil4Columns == 4 );

	//## INITIALIZE
	long nCheckFilters = m_vecCheckFilters.size();
	long nCheckLabels = m_vecCheckLabels.size();	ATLASSERT(nCheckFilters == nCheckLabels);
	long nCols = m_nNoOfColumns;
	long nRows = (nCheckFilters + nCols - 1) / nCols;
	long nCol = 0, nRow = 0;
	UINT nFlags = NULL;
	RECT rcOld, rcNew; CWindow *pwnd;
	RECT rcParent; GetWindowRect( &rcParent );

	//## BEGIN RESIZE 
	HDWP hdwp = ::BeginDeferWindowPos( nCheckFilters + nCheckLabels );

	//## ITERATE CheckLabels
	for(long i=0; i<nCheckLabels; i++){
		//## ASSERT
		pwnd = m_vecCheckLabels.at( i );
		if (!pwnd) continue;
		if (!pwnd->m_hWnd) continue;
		ATLASSERT( pwnd->GetParent() == m_hWnd );

		//## GET CELL Control RECT
		::GetWindowRect( pwnd->m_hWnd, &rcOld );
		nCol = i / nRows;
		nRow = i % nRows;
		GetCellControlRect( nRow, nCol, nRows, nCols, TRUE, rcParent, rcNew );

		//## ADD
		nFlags = SWP_NOZORDER; // | SWP_NOACTIVATE | SWP_NOREPOSITION
		if ((rcNew.left == rcOld.left) && (rcNew.top == rcOld.top)) nFlags |= SWP_NOMOVE;
		if ((rcNew.right - rcNew.left == rcOld.right - rcOld.left) && (rcNew.bottom - rcNew.top == rcOld.bottom - rcOld.top)) nFlags |= SWP_NOSIZE;
		ScreenToClient( &rcNew );
		if (!((nFlags & SWP_NOMOVE) && (nFlags & SWP_NOSIZE)))
			::DeferWindowPos( hdwp, pwnd->m_hWnd, NULL, rcNew.left, rcNew.top, rcNew.right - rcNew.left, rcNew.bottom - rcNew.top, nFlags);
	}

	//## ITERATE CheckFilter
	for(long i=0; i<nCheckFilters; i++){
		//## ASSERT
		pwnd = m_vecCheckFilters.at( i );
		if (!pwnd) continue;
		if (!pwnd->m_hWnd) continue;
		ATLASSERT( pwnd->GetParent() == m_hWnd );

		//## GET CELL Control RECT
		::GetWindowRect( pwnd->m_hWnd, &rcOld );
		nCol = i / nRows;
		nRow = i % nRows;
		GetCellControlRect( nRow, nCol, nRows, nCols, FALSE, rcParent, rcNew );

		//## ADD
		nFlags = SWP_NOZORDER; // | SWP_NOACTIVATE | SWP_NOREPOSITION
		if ((rcNew.left == rcOld.left) && (rcNew.top == rcOld.top)) nFlags |= SWP_NOMOVE;
		if ((rcNew.right - rcNew.left == rcOld.right - rcOld.left) && (rcNew.bottom - rcNew.top == rcOld.bottom - rcOld.top)) nFlags |= SWP_NOSIZE;
		ScreenToClient( &rcNew );
		if (!((nFlags & SWP_NOMOVE) && (nFlags & SWP_NOSIZE)))
			::DeferWindowPos( hdwp, pwnd->m_hWnd, NULL, rcNew.left, rcNew.top, rcNew.right - rcNew.left, rcNew.bottom - rcNew.top, nFlags);
	}

	//## END RESIZE
	if (::EndDeferWindowPos( hdwp ) == 0) ATLTRACE("MCosmin: Sizing error: %d", GetLastError());

	//## RETURN
	return 0L;
}
//## ====================================================================================
LRESULT CQFilter::OnDraw(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## ASSERT
	long nControlID = wParam;
	long nIndex = nControlID - IDC_CHECK_FILTER;
	if ((nIndex < 0) || (nIndex >= (long)m_vecCheckFilters.size())) return 0L;

	//## DRAW
	m_vecCheckFilters.at( nIndex )->DrawItem( (LPDRAWITEMSTRUCT)lParam );

	//## RETURN
	return 0L;
}
//#######################################################################################
void CQFilter::CreateCheckFilter( CCheckComboBox *pwndCombo, long nIndex )
{
	//## ASSERT
	if (pwndCombo->m_hWnd) return;

	//## CREATE
	static RECT rc = {0, 0, 20, 20};
	pwndCombo->Create(	m_hWnd, rc, NULL,
						WS_CHILDWINDOW | WS_VISIBLE | WS_TABSTOP | BS_OWNERDRAW,
						WS_EX_LEFT | WS_EX_LTRREADING | WS_EX_NOPARENTNOTIFY,
						IDC_CHECK_FILTER + nIndex );
	pwndCombo->ModifyStyle( NULL, NULL );
	pwndCombo->SetQFilter( this );
	pwndCombo->SetFont( GetFont() );
}
//## ====================================================================================
void CQFilter::CreateCheckLabel( CWindow *pwndLabel, long nIndex )
{
	//## ASSERT
	if (pwndLabel->m_hWnd) return;

	//## CREATE
	static RECT rc = {0, 0, 20, 20};
	pwndLabel->Create(	TEXT("STATIC"), m_hWnd, rc, NULL, 
						WS_CHILDWINDOW | WS_VISIBLE | WS_GROUP | SS_LEFT, 
						WS_EX_LEFT | WS_EX_LTRREADING | WS_EX_RIGHTSCROLLBAR | WS_EX_NOPARENTNOTIFY,
						IDC_CHECK_LABEL + nIndex );
	pwndLabel->SetFont( GetFont() );

	//## DEFAULT LABEL
	static char buffer[SIMPLE_BUFFER_SIZE];
	sprintf(buffer, "Label%d", nIndex);
	USES_CONVERSION;
	pwndLabel->SetWindowText( A2T(buffer) );
}
//#######################################################################################
STDMETHODIMP CQFilter::AboutBox()
{
    CAboutDlg dlg;
    dlg.DoModal();
    return S_OK;
}
//#######################################################################################
void CQFilter::GetCellControlRect( long &nRow, long &nCol, long &nRows, long &nCols, BOOL bLabel, RECT &rcParent, RECT &rc )
{
	//## ASSERT
	ATLASSERT(nCols != 0);

	//## BIG RECT
	rc.left = rcParent.left + ((rcParent.right - rcParent.left) * nCol) / nCols;
	rc.right = rc.left + (rcParent.right - rcParent.left) / nCols;
	rc.top = rcParent.top + nRow * (CELL_CONTROL_HEIGHT + CELL_CONTROL_VSPACE);
	rc.bottom = rc.top + CELL_CONTROL_HEIGHT;

	//## HALF RECT
	if (bLabel) rc.right = rc.left + (rc.right - rc.left) / 2;
	else		rc.left = rc.left + (rc.right - rc.left) / 2;

	//## CUT HSpace
	rc.right = rc.right - CELL_CONTROL_HSPACE;

	//## ASSERT
	if (rc.right < rc.left) rc.right = rc.left;
	if (rc.bottom < rc.top) rc.bottom = rc.top;
}
//#######################################################################################
#define ASSERT_CHECKCOMBO()\
	if ((nFilter < 0) && (nFilter >= (long)m_vecCheckFilters.size())) return E_FAIL;\
	CCheckComboBox* pCombo = m_vecCheckFilters.at(nFilter);\
	if (!pCombo) return E_FAIL;\
	if (!::IsWindow(pCombo->m_hWnd)) return E_FAIL;
#define ASSERT_CHECKLABEL()\
	if ((nFilter < 0) && (nFilter >= (long)m_vecCheckLabels.size())) return E_FAIL;\
	CWindow* pLabel = m_vecCheckLabels.at(nFilter);\
	if (!pLabel) return E_FAIL;\
	if (!::IsWindow(pLabel->m_hWnd)) return E_FAIL;
//#######################################################################################
STDMETHODIMP CQFilter::AddFolder(long nFilter, BSTR bstrFolder)
{
	ASSERT_CHECKCOMBO();
	pCombo->AddString( bstrFolder );
	return S_OK;
}
//## ====================================================================================
STDMETHODIMP CQFilter::AddString(long nFilter, BSTR bstrString, long nID, long nLevel)
{
	ASSERT_CHECKCOMBO();
	pCombo->AddString( bstrString, nID, nLevel );
	return S_OK;
}
//## ====================================================================================
STDMETHODIMP CQFilter::CheckAll(long nFilter, VARIANT_BOOL bCheck)
{
	ASSERT_CHECKCOMBO();
	pCombo->CheckAll( bCheck );
	return S_OK;
}
//#######################################################################################
STDMETHODIMP CQFilter::get_CheckLabel(long nFilter, BSTR *pVal)
{
	//## ASSERT
	ASSERT_CHECKLABEL();

	//## GET Caption
	CComBSTR bstr;
	pLabel->GetWindowText( bstr.m_str );
	*pVal = bstr.Copy();

	//## RETURN
	return S_OK;
}
//## ====================================================================================
STDMETHODIMP CQFilter::put_CheckLabel(long nFilter, BSTR newVal)
{
	//## ASSERT
	ASSERT_CHECKLABEL();

	//## SET Caption
	USES_CONVERSION;
	pLabel->SetWindowText( W2T(newVal) );

	//## RETURN
	return S_OK;
}
//#######################################################################################
STDMETHODIMP CQFilter::get_DroppedWidth(long nFilter, long *pVal)
{
	ASSERT_CHECKCOMBO();
	*pVal = pCombo->GetDroppedWidth();
	return S_OK;
}
STDMETHODIMP CQFilter::put_DroppedWidth(long nFilter, long newVal)
{
	ASSERT_CHECKCOMBO();
	pCombo->SetDroppedWidth( newVal );
	return S_OK;
}
//#######################################################################################
STDMETHODIMP CQFilter::get_Check(long nFilter, long nID, VARIANT_BOOL *pVal)
{
	ASSERT_CHECKCOMBO();
	*pVal = pCombo->GetCheck( nID );
	return S_OK;
}
STDMETHODIMP CQFilter::put_Check(long nFilter, long nID, VARIANT_BOOL newVal)
{
	ASSERT_CHECKCOMBO();
	pCombo->SetCheck( nID, newVal );
	return S_OK;
}
//#######################################################################################
STDMETHODIMP CQFilter::get_Field(long nFilter, BSTR *pVal)
{
	ASSERT_CHECKCOMBO();
	*pVal = pCombo->GetField().Copy();
	return S_OK;
}
STDMETHODIMP CQFilter::put_Field(long nFilter, BSTR newVal)
{
	ASSERT_CHECKCOMBO();
	pCombo->SetField(newVal);
	return S_OK;
}
//#######################################################################################
STDMETHODIMP CQFilter::get_CheckFilterIDs(long nFilter, BSTR *pVal)
{
	//## ASSERT
	ASSERT_CHECKCOMBO();

	//## GET Checked IDS
	CComBSTR bstr = pCombo->GetCheckedIDs();
	*pVal = bstr.Copy();

	//## RETURN
	return S_OK;
}
STDMETHODIMP CQFilter::get_CheckFilterTexts(long nFilter, BSTR *pVal)
{
	//## ASSERT
	ASSERT_CHECKCOMBO();

	//## GET Checked Texts
	CComBSTR bstr = pCombo->GetCheckedTexts();
	*pVal = bstr.Copy();

	//## RETURN
	return S_OK;
}
STDMETHODIMP CQFilter::get_SQLCheckFilter(long nFilter, BSTR *pVal)
{
	//## ASSERT
	ASSERT_CHECKCOMBO();

	//## CHECKED IDS
	CComBSTR bstrCheckedIDS = pCombo->GetCheckedIDs();
	if ((bstrCheckedIDS.Length() <= 0) || (wcsicmp( bstrCheckedIDS, L"*" ) == 0)){
		*pVal = NULL;
		return S_OK;
	}

	//## GET CheckFilter
	CComBSTR bstr = " (";
	bstr += pCombo->GetField();
	bstr += " IN ";
	bstr += bstrCheckedIDS;
	bstr += ") ";
	*pVal = bstr.Copy();

	//## RETURN
	return S_OK;
}
STDMETHODIMP CQFilter::get_TextCheckFilter(long nFilter, BSTR *pVal)
{
	//## ASSERT
	ASSERT_CHECKLABEL();
	ASSERT_CHECKCOMBO();

	//## GET Label
	CComBSTR bstrLabel;
	pLabel->GetWindowText( bstrLabel.m_str );

	//## CHECKED TEXTS
	CComBSTR bstrCheckedTexts = pCombo->GetCheckedTexts();
	if ((bstrCheckedTexts.Length() <= 0) || (wcsicmp( bstrCheckedTexts, L"*" ) == 0)){
		*pVal = NULL;
		return S_OK;
	}

	//## GET CheckFilter
	CComBSTR bstr = " (";
	bstr += bstrLabel;
	bstr += " ";
	bstr += bstrCheckedTexts;
	bstr += ") ";
	*pVal = bstr.Copy();

	//## RETURN
	return S_OK;
}
STDMETHODIMP CQFilter::get_SQLFilter(BSTR *pVal)
{
	//## BUILD SQL Filter
	CComBSTR bstrFilter = "";
	for(long i=0; i<(long)m_vecCheckFilters.size(); i++){
		CComBSTR bstr;
		get_SQLCheckFilter( i, &bstr.m_str );	//## SAFE
		if (bstr.Length() > 0){
			if (bstrFilter.Length() > 0)
				bstrFilter += " AND ";
			bstrFilter += bstr;
		}
	}

	//## RETURN
	*pVal = bstrFilter.Copy();
	return S_OK;
}
STDMETHODIMP CQFilter::get_TextFilter(BSTR *pVal)
{
	//## BUILD SQL Filter
	CComBSTR bstrFilter = "";
	for(long i=0; i<(long)m_vecCheckFilters.size(); i++){
		CComBSTR bstr;
		get_TextCheckFilter( i, &bstr.m_str );	//## SAFE
		if (bstr.Length() > 0){
			if (bstrFilter.Length() > 0)
				bstrFilter += " AND ";
			bstrFilter += bstr;
		}
	}

	//## RETURN
	*pVal = bstrFilter.Copy();
	return S_OK;
}
//#######################################################################################
