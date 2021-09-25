//#######################################################################################
//## QFilterPropPageGeneral.cpp : Implementation of CQFilterPropPageGeneral
//#######################################################################################
#include "stdafx.h"
#include "QuickFilter.h"
#include "QFilterPropPageGeneral.h"
#include "MacrosPP.h"

/////////////////////////////////////////////////////////////////////////////
// IPropertyPage interface methods

/**************/
/* SetObjects */
/**************/

// Implementation notes:
// Visual Basic 6 does not obey the stated requirements for this method.

// Here are the requirements:
// The property page is required to keep the pointers returned by this method
// or others queried through them. If these specific IUnknown pointers are held,
// the property page must call IUnknown::AddRef through each when caching them,
// until the time when IPropertyPage::SetObjects is called with cObjects equal to zero.
// At that time, the property page must call IUnknown::Release through each pointer,
// releasing any objects that it held.

// The caller must provide the property page with these objects before calling
// IPropertyPage::Activate, and should call IPropertyPage::SetObjects with zero
// as the parameter when deactivating the page or when releasing the object entirely.
// Each call to SetObjects with a non-NULL ppUnk parameter must be matched with a
// later call to SetObjects with zero in the cObjects parameter.

// Visual Basic 6 can repeatedly call SetObjects with non-zero in the cObjects parameter
// with no intervening call to SetObjects with zero as the cObjects parameter.
//   To reproduce:
//      In design mode: Add control(s) to form. Select control(s).
//      Display the controls' property page(s) (View Property Pages (Shift+F4)).
//      VB calls SetObjects with a non-zero cObjects to send the selected object(s)
//          to the property page (as it should).
//      Change a property using VB's View Property Window (F4)
//      VB calls SetObjects *again* with a non-zero cObjects presumably so that the
//          the property page reloads the changed property from the object(s).
//      Implementing IPropertyNotifySink on the property page (the correct way to
//          receive property change notifications) doesn't affect this behavior.

// Visual Basic 6 normally *never* calls SetObjects with zero in the cObjects parameter.
//  To reproduce:
//      Do the above and close the VB project.
//      Notice that SetObjects is never called to release the object array.
//      Workaround is to released the array in the property page destructor
//          if it isn't already released.

// Visual Basic 6 only calls SetObjects with zero in the cObjects parameter when
// a preceding call to SetObjects with a non-zero value for cObjects returns
// a failure status.

STDMETHODIMP CQFilterPropPageGeneral::SetObjects(ULONG cObjects, IUnknown** ppUnk)
{
    ATLTRACE2(atlTraceControls,2,_T("CQFilterPropPageGeneral::SetObjects\n"));

	if (ppUnk == NULL) return E_POINTER;

    ///////////////////////////////////////////////////////
    // Remove previous objects, connections, and cookies //
    ///////////////////////////////////////////////////////
    CleanupObjectArray();

    ///////////////////////////////////////////////////////////////////
    // Allocate new object array, make connections, and save cookies //
    ///////////////////////////////////////////////////////////////////
    if (cObjects > 0) {
        // Allocate object array
        ATLTRY(m_ppUnk = new LPUNKNOWN[cObjects]);
        if (m_ppUnk == NULL) return E_OUTOFMEMORY;

        // Allocate connection cookies
        ATLTRY(m_pCookies = new DWORD[cObjects]);
        if (m_pCookies == NULL) {
            delete [] m_ppUnk;
            m_ppUnk = NULL;
            return E_OUTOFMEMORY;
        }

        // Make a connection to each object's connection point
        for (UINT i = 0; i < cObjects; i++) {
            // Ensure object supports the default interface...
            // We need this interface to send changes back to object
            HRESULT hr = ppUnk[i]->QueryInterface (IID_IQFilter, (void**)&m_ppUnk[i]);
            if (FAILED(hr)) return hr;

            // Establish a connection point from object to our sink...
            // We need this to receive change notifications from the object
            hr = AtlAdvise(m_ppUnk[i],
                           static_cast<IPropertyNotifySink*>(this),
					       IID_IPropertyNotifySink,
                           &m_pCookies[i]);
//            if (FAILED(hr)) return hr;
        }
    }
    m_nObjects = cObjects;

    ////////////////////////////////////////////////////////////
    // Transfer properties from first object to property page //
    ////////////////////////////////////////////////////////////

    InitializeControlsFromObject (DISPID_UNKNOWN);

    return S_OK;
}

/************/
/* Activate */
/************/

STDMETHODIMP CQFilterPropPageGeneral::Activate(HWND hWndParent, LPCRECT pRect, BOOL bModal)
{
    ATLTRACE(_T("CQFilterPropPageGeneral::Activate\n"));
    HRESULT hr = IPropertyPage2Impl<CQFilterPropPageGeneral>::Activate(hWndParent, pRect, bModal);
    if (SUCCEEDED (hr) && DISPID_UNKNOWN != m_EditProperty) {
        EditProperty (m_EditProperty);
        m_EditProperty = DISPID_UNKNOWN;
    }
    return hr;
}

/*********/
/* Apply */
/*********/

STDMETHODIMP CQFilterPropPageGeneral::Apply(void)
{
    ATLTRACE(_T("CQFilterPropPageGeneral::Apply\n"));
    SetPropertiesFromControls(DISPID_UNKNOWN);
    m_bDirty = FALSE;
    m_flags  = 0;
    return S_OK;
}

/////////////////////////////////////////////////////////////////////////////
// IPropertyNotifySink interface methods

STDMETHODIMP CQFilterPropPageGeneral::OnChanged(DISPID dispid)
{
    InitializeControlsFromObject (dispid);
    return S_OK;
}

STDMETHODIMP CQFilterPropPageGeneral::OnRequestEdit(DISPID dispid)
{
    return S_OK;
}

/**********************/
/* CleanupObjectArray */
/**********************/

void CQFilterPropPageGeneral::CleanupObjectArray()
{
    // Free existing array of objects, if any
    if (m_ppUnk != NULL && m_nObjects > 0) {
        for (UINT i = 0; i < m_nObjects; i++) {
            if (NULL == m_ppUnk[i]) break;
            // Unadvise the connection
			AtlUnadvise(m_ppUnk[i], IID_IPropertyNotifySink, m_pCookies[i]);
            m_ppUnk[i]->Release();
        }

        delete [] m_ppUnk;
        delete [] m_pCookies;
	}
    // Currently no objects in list
	m_ppUnk    = NULL;
	m_pCookies = NULL;
    m_nObjects = 0;
}

//#######################################################################################
CQFilterPropPageGeneral::CQFilterPropPageGeneral()
{
	m_dwTitleID     = IDS_PP_GENERAL_TITLE;
	m_dwHelpFileID  = IDS_PP_GENERAL_IDS_HELPFILE;
	m_dwDocStringID = IDS_PP_GENERAL_IDS_DOCSTRING;

    m_flags         = 0;    // No changes yet
    m_pCookies      = NULL;
    m_EditProperty  = DISPID_UNKNOWN;
}
//## ====================================================================================
CQFilterPropPageGeneral::~CQFilterPropPageGeneral()
{
    CleanupObjectArray() ;
}
//#######################################################################################
//#######################################################################################
//#######################################################################################
STDMETHODIMP CQFilterPropPageGeneral::EditProperty(DISPID dispid)
{
    ATLTRACE(_T("CQFilterPropPageGeneral::EditProperty\n"));
    m_EditProperty = dispid;

    if (IsWindow()) {
        switch (dispid) {
			FOCUS_ENTRY(DISPID_QFILTER_NOOFCOLUMNS,			IDC_TAB_NOOFCOLUMNS)
			FOCUS_ENTRY(DISPID_QFILTER_NOOFCHECKFILTERS,	IDC_EDIT_NOOFCHECKFILTERS)
			default: return E_INVALIDARG;
        }
    }
    return S_OK;
}
//#######################################################################################
void CQFilterPropPageGeneral::InsertTabItem(ATLControls::CTabCtrlT<CWindow> &tab, UINT nMask, int nItem, LPCTSTR lpszItem, int nImage, LPARAM lParam)
{
	//## ITEM
	TC_ITEM item;
	item.mask = nMask;
	item.iImage = nImage;
	item.lParam = lParam;
	item.pszText = (LPTSTR)lpszItem;

	//## INSERT
	tab.InsertItem( nItem, &item );
}
//## ====================================================================================
LRESULT CQFilterPropPageGeneral::OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
	//## INITIALIZE Controls (UI)
	INITIALIZE_TAB(NOOFCOLUMNS,			NoOfColumns)

	INITIALIZE_EDIT(NOOFCHECKFILTERS,	NoOfCheckFilters)

	InsertTabItem( m_tabNoOfColumns,	TCIF_IMAGE | TCIF_TEXT, 0, TEXT(" fil2Columns "), 0, 0 );
	InsertTabItem( m_tabNoOfColumns,	TCIF_IMAGE | TCIF_TEXT, 1, TEXT(" fil3Columns "), 1, 1 );
	InsertTabItem( m_tabNoOfColumns,	TCIF_IMAGE | TCIF_TEXT, 2, TEXT(" fil4Columns "), 2, 2 );

	//## INITIALIZE Controls (Data)
    InitializeControlsFromObject(DISPID_UNKNOWN);
    return 0;
}
//#######################################################################################
LRESULT CQFilterPropPageGeneral::OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled)
{
    //INITCOMMONCONTROLSEX ic = { sizeof (ic), ICC_UPDOWN_CLASS };
    //BOOL bInit = InitCommonControlsEx (&ic);
    return 0;
}
//#######################################################################################
LRESULT CQFilterPropPageGeneral::OnSelchange(WPARAM wParam, LPNMHDR lParam, BOOL& b)
{
	CHANGE_ENTRY_N(DISPID_QFILTER_NOOFCOLUMNS,	n,	NoOfColumns,	enum NoOfColumnsValues, 2)

    SetDirty (m_flags != 0);
	return 0;
}
//#######################################################################################
LRESULT CQFilterPropPageGeneral::OnChangeEdit(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    BOOL bTranslated = FALSE;

	CHANGE_ENTRY_FREE_INT(DISPID_QFILTER_NOOFCHECKFILTERS,	IDC_EDIT_NOOFCHECKFILTERS,		n,	NoOfCheckFilters,		long)

    SetDirty (m_flags != 0);
	return 0;
}
//#######################################################################################
LRESULT CQFilterPropPageGeneral::OnClickedCheck(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled)
{
    SetDirty (m_flags != 0);
	return 0;
}
//#######################################################################################
void CQFilterPropPageGeneral::InitializeControlsFromObject(DISPID dispid)
{
    // The array already contains IQFilter* so this is redundant...
    CComQIPtr<IQFilter> pQFilter = m_ppUnk[0];

	INITIALIZE_CASE_N		(DISPID_QFILTER_NOOFCOLUMNS,		n,	NoOfColumns,			enum NoOfColumnsValues,		2)

	INITIALIZE_CASE_FREE_INT(DISPID_QFILTER_NOOFCHECKFILTERS,	IDC_EDIT_NOOFCHECKFILTERS,	n,	NoOfCheckFilters,		long)
}
//#######################################################################################
void CQFilterPropPageGeneral::SetPropertiesFromControls(DISPID dispid)
{
    // For all objects in array...
    for (UINT i = 0; i < m_nObjects; i++) {
        // Get the appropriate interface...
        CComQIPtr<IQFilter> pQFilter = m_ppUnk[i];

		SET_CASE(DISPID_QFILTER_NOOFCOLUMNS,		n,	NoOfColumns,			enum NoOfColumnsValues)

		SET_CASE(DISPID_QFILTER_NOOFCHECKFILTERS,	n,	NoOfCheckFilters,		long)
    }
}
//#######################################################################################
