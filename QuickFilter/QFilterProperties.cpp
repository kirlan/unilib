//#######################################################################################
//## QFilterProperties.cpp : Implementation of CQFilter Properties
//#######################################################################################
#include "stdafx.h"
#include "QuickFilter.h"
#include "QFilter.h"
//#######################################################################################
IMPLEMENT_PROPERTY	(NoOfCheckFilters,		n,		DISPID_QFILTER_NOOFCHECKFILTERS,		long,					long)
IMPLEMENT_PROPERTY	(NoOfColumns,			n,		DISPID_QFILTER_NOOFCOLUMNS,				enum NoOfColumnsValues,	enum NoOfColumnsValues)
//#######################################################################################
HRESULT CQFilter::ValidateProperty(DISPID dispid, void *pVal)
{
	//## ASSERT General
	if( pVal == NULL ) return E_POINTER;
	
	//## DECLARE
	static long *plVal = NULL;
	static VARIANT_BOOL *pbVal = NULL;

	//## ASSERT Particular
	switch(dispid){
		//## VALIDATE: Rows, Cols >= 0
		case DISPID_QFILTER_NOOFCHECKFILTERS:
			plVal = (long*)pVal;
			if (*plVal < 0) return E_INVALIDARG;
			if (*plVal > 200) return E_INVALIDARG;
			break;
		case DISPID_QFILTER_NOOFCOLUMNS:
			plVal = (long*)pVal;
			if (*plVal < fil2Columns) return E_INVALIDARG;;
			if (*plVal > fil4Columns) return E_INVALIDARG;;
			break;
	}

	//## RETURN
	return S_OK;
}
//#######################################################################################
//STDMETHODIMP CQFilter::GetDisplayString(DISPID dispid, BSTR *pBstr)
//{
//	//## TRACE
//	ATLTRACE2(atlTraceControls,2,_T("CQFilter::GetDisplayString\n"));
//
//	//## GET DisplayString
//    switch (dispid) {
//		case DISPID_QFILTER_NOOFCOLUMNS:
//			if (m_nNoOfColumns == fil2Columns)	*pBstr = SysAllocString (OLESTR("2 - fil2Columns"));
//			if (m_nNoOfColumns == fil3Columns)	*pBstr = SysAllocString (OLESTR("3 - fil3Columns"));
//			if (m_nNoOfColumns == fil4Columns)	*pBstr = SysAllocString (OLESTR("4 - fil4Columns"));
//			return	*pBstr ? S_OK : E_OUTOFMEMORY;
//			break; //## END Case
//
//		case DISPID_BACKCOLOR:				// Make Visual Basic apply default formatting
//		case DISPID_FORECOLOR:				//   for these color properties
//			return S_FALSE;					// Otherwise it display color values in decimal
//			break; //## END Case			//   and doesn't draw the color sample correctly
//		default:							// This is an undocumented return value that works...
//			return S_FALSE;					//## RETURN S_FALSE for all
//			break; //## END Case
//    }
//
//	//## DEFAULT
//    return IPerPropertyBrowsingImpl<CQFilter>::GetDisplayString(dispid, pBstr);
//}
//#######################################################################################
//static const LPCOLESTR    rszNoOfColumnsStrings [] = { OLESTR("2 - fil2Columns"), OLESTR("3 - fil3Columns"), OLESTR("4 - fil4Columns") };
//static const DWORD        rdwNoOfColumnsCookies [] = { 0, 1, 2 };
//static const long         rvbNoOfColumnsValues  [] = { 2, 3, 4 };
//
//static const UINT cNoOfColumnsStrings = DIM(rszNoOfColumnsStrings);
//static const UINT cNoOfColumnsCookies = DIM(rdwNoOfColumnsCookies);
//static const UINT cNoOfColumnsValues  = DIM(rvbNoOfColumnsValues);
//#######################################################################################
//STDMETHODIMP CQFilter::GetPredefinedStrings(/*[in]*/ DISPID dispid, /*[out]*/ CALPOLESTR *pcaStringsOut, /*[out]*/ CADWORD *pcaCookiesOut)
//{
//	//## ASSERT
//	if (NULL == pcaStringsOut || NULL == pcaCookiesOut) return E_POINTER;
//
//	//## INITIALIZE
//	pcaStringsOut->cElems = 0;
//	pcaStringsOut->pElems = NULL;
//	pcaCookiesOut->cElems = 0;
//	pcaCookiesOut->pElems = NULL;
//
//	//## PREDEFINED Strings
//    HRESULT hr = S_OK;
//    switch (dispid){
//		PREDEFINED_STRINGS_CASE(DISPID_QFILTER_NOOFCOLUMNS,			NoOfColumns)
//    }
//
//	//## DAFAULT
//    return IPerPropertyBrowsingImpl<CQFilter>::GetPredefinedStrings(dispid, pcaStringsOut, pcaCookiesOut);
//}
//## ====================================================================================
//STDMETHODIMP CQFilter::GetPredefinedValue(DISPID dispid, DWORD dwCookie, VARIANT* pVarOut)
//{
//	//## ASSERT
//    if (NULL == pVarOut) return E_POINTER;
//
//    ULONG i;
//    switch (dispid){
//		PREDEFINED_VALUES_CASE(DISPID_QFILTER_NOOFCOLUMNS,			NoOfColumns)
//	}
//
//	//## DEFAULT
//    return IPerPropertyBrowsingImpl<CQFilter>::GetPredefinedValue(dispid, dwCookie, pVarOut);
//}
//#######################################################################################
//STDMETHODIMP CQFilter::MapPropertyToPage(DISPID dispid, CLSID *pClsid)
//{
//	//## TRACE
//	ATLTRACE2(atlTraceControls,2,_T("CQFilter::MapPropertyToPage\n"));
//
//	// Look up the property page CLSID in the PROP MAP
//	HRESULT hr = IPerPropertyBrowsingImpl<CQFilter>::MapPropertyToPage(dispid, pClsid);
//
//	// When there is no property page for a listed property, fail the call
//	if (SUCCEEDED(hr) && CLSID_NULL == *pClsid) hr = E_INVALIDARG;
//	return hr;
//}
//#######################################################################################
//STDMETHODIMP CQFilter::MapPropertyToCategory(/*[in]*/ DISPID dispid, /*[out]*/ PROPCAT* ppropcat)
//{
//    if (NULL == ppropcat) return E_POINTER;
//
//    switch (dispid) {
//		case DISPID_FORECOLOR:
//		case DISPID_BACKCOLOR:
//			*ppropcat = PROPCAT_Appearance;
//			return S_OK;
//
//		case DISPID_ENABLED:
//			*ppropcat = PROPCAT_Behavior;
//			return S_OK;
//
//		default:
//			return E_FAIL;
//    }
//}
//## ====================================================================================
//STDMETHODIMP CQFilter::GetCategoryName(/*[in]*/ PROPCAT propcat, /*[in]*/ LCID lcid, /*[out]*/ BSTR* pbstrName)
//{
//    if(PROPCAT_Scoring == propcat) {
//        *pbstrName = ::SysAllocString(L"Scoring"); 
//        return S_OK;
//    }
//    return E_FAIL;
//}
//#######################################################################################
HRESULT CQFilter::OnPutProperty(DISPID dispid, void *pVal)
{
	//## ASSERT
	if( pVal == NULL ) return E_POINTER;

	//## DECLARE
	static long *plVal = NULL;
	static VARIANT_BOOL *pbVal = NULL;

	//## EXTRA Initialisations
	switch(dispid){
		case DISPID_QFILTER_NOOFCHECKFILTERS:
			//## RESIZE internal structures
			plVal = (long*)pVal;

			//## ADD
			while ((long)m_vecCheckFilters.size() < *plVal){
				CCheckComboBox *pwndCombo = new CCheckComboBox;
				CWindow *pwndLabel = new CWindow;

				m_vecCheckFilters.push_back( pwndCombo );
				m_vecCheckLabels.push_back( pwndLabel );

				if (m_hWnd) CreateCheckFilter( pwndCombo, m_vecCheckLabels.size() - 1 );
				if (m_hWnd) CreateCheckLabel( pwndLabel, m_vecCheckLabels.size() - 1 );
			};

			//## REMOVE
			while ((long)m_vecCheckFilters.size() > *plVal){
				CCheckComboBox *pwndCombo = m_vecCheckFilters.at( m_vecCheckFilters.size() - 1 );
				CWindow *pwndLabel = m_vecCheckLabels.at( m_vecCheckLabels.size() - 1 );

				m_vecCheckFilters.pop_back();
				m_vecCheckLabels.pop_back();

				if (pwndCombo){
					pwndCombo->DestroyWindow();
					delete pwndCombo;
				}
				if (pwndLabel){
					pwndLabel->DestroyWindow();
					delete pwndLabel;
				}
			};

			//## RESIZE
			if (::IsWindow(m_hWnd))
				SendMessage(WM_SIZE, SIZE_RESTORED);
			break;
		case DISPID_QFILTER_NOOFCOLUMNS:
			if (::IsWindow(m_hWnd))
				SendMessage(WM_SIZE, SIZE_RESTORED);
			break;
	}

	//## RETURN
	return S_OK;
}
//#######################################################################################
