//#######################################################################################
//## QFilterPropPageGeneral.h : Declaration of the CQFilterPropPageGeneral
//#######################################################################################
#ifndef __QFilterPropPageGeneral_H_
#define __QFilterPropPageGeneral_H_

#include "resource.h"       // main symbols

#include <commctrl.h>
//#######################################################################################
EXTERN_C const CLSID CLSID_QFilterPropPageGeneral;
//#######################################################################################
class ATL_NO_VTABLE CQFilterPropPageGeneral:
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CQFilterPropPageGeneral, &CLSID_QFilterPropPageGeneral>,
	public IPropertyPage2Impl<CQFilterPropPageGeneral>,
	public CDialogImpl<CQFilterPropPageGeneral>,
    public IPropertyNotifySink
{
public:
	CQFilterPropPageGeneral();
	~CQFilterPropPageGeneral();

	enum {IDD = IDD_QFILTER_PROP_PAGE_GENERAL};

DECLARE_REGISTRY_RESOURCEID(IDR_QFILTER_PROP_PAGE_GENERAL)
DECLARE_PROTECT_FINAL_CONSTRUCT()

BEGIN_COM_MAP(CQFilterPropPageGeneral) 
	COM_INTERFACE_ENTRY(IPropertyPage)
	COM_INTERFACE_ENTRY_IMPL(IPropertyPage2)
	COM_INTERFACE_ENTRY(IPropertyNotifySink)
END_COM_MAP()

BEGIN_MSG_MAP(CQFilterPropPageGeneral)
	CHAIN_MSG_MAP(IPropertyPageImpl<CQFilterPropPageGeneral>)
	MESSAGE_HANDLER(WM_INITDIALOG, OnInitDialog)
	MESSAGE_HANDLER(WM_CREATE, OnCreate)

	NOTIFY_CODE_HANDLER(TCN_SELCHANGE, OnSelchange)
	COMMAND_HANDLER(IDC_EDIT_NOOFCHECKFILTERS,		EN_CHANGE, OnChangeEdit)
END_MSG_MAP()

// IPropertyPage2
	STDMETHODIMP Activate(HWND hWndParent, LPCRECT pRect, BOOL /* bModal */);
	STDMETHODIMP Apply(void);
    STDMETHODIMP EditProperty(DISPID dispID);
    STDMETHODIMP SetObjects(ULONG nObjects, IUnknown** ppUnk);

// IPropertyNotifySink
    STDMETHODIMP OnChanged(DISPID dispid);
    STDMETHODIMP OnRequestEdit(DISPID dispid);

// Dialog box messge handlers
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnCreate(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSelchange(WPARAM wParam, LPNMHDR lParam, BOOL& b);
	LRESULT OnChangeEdit(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);
	LRESULT OnClickedCheck(WORD wNotifyCode, WORD wID, HWND hWndCtl, BOOL& bHandled);

private:
	DISPID m_EditProperty;
    enum{
		DISPID_QFILTER_NOOFCOLUMNS_CHANGED = 1,
		DISPID_QFILTER_NOOFCHECKFILTERS_CHANGED = 2,
	};

	ATLControls::CTabCtrlT<CWindow> m_tabNoOfColumns;

	ATLControls::CImageList m_imgNoOfColumns;

	ATLControls::CEditT<CWindow>	m_txtNoOfCheckFilters;

	short			m_flags;            // Change flags

	long			m_nNoOfColumnsOrig;
	long			m_nNoOfColumnsNew;
	long			m_nNoOfCheckFiltersOrig;
	long			m_nNoOfCheckFiltersNew;

	LPDWORD m_pCookies;    // Array of connection tokens used by
                           // IConnectionPoint::Advise/Unadvise.

protected:
	void InitializeControlsFromObject(DISPID dispid);
    void SetPropertiesFromControls(DISPID dispid);
	void CleanupObjectArray();

private:
	void InsertTabItem(ATLControls::CTabCtrlT<CWindow> &tab, UINT nMask, int nItem, LPCTSTR lpszItem, int nImage, LPARAM lParam);
};
//#######################################################################################
#endif //__QFilterPropPageGeneral_H_
//#######################################################################################
