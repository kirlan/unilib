//#######################################################################################
//## QFilter.h : Declaration of the CQFilter
//## [Magerusan G. Cosmin]  5-feb-2003 (Created)
//#######################################################################################
#ifndef __QFILTER_H_
#define __QFILTER_H_

#include "resource.h"       // main symbols
#include <atlctl.h>
#include "CheckComboBox.h"
#include "QFilterEventsCP.h"
#include "AboutDlg.h"
#include "QFilterPropPageGeneral.h"
//#######################################################################################
#define CELL_CONTROL_HEIGHT 20
#define CELL_CONTROL_VSPACE 2
#define CELL_CONTROL_HSPACE 5
//#######################################################################################
class ATL_NO_VTABLE CQFilter : 
    //## COM object implementation
	public CComObjectRootEx<CComSingleThreadModel>,
	public CComCoClass<CQFilter, &CLSID_QFilter>,
	public IDispatchImpl<IQFilter, &IID_IQFilter, &LIBID_QUICKFILTERLib>,
    //## Control implementation
	public CComCompositeControl<CQFilter>,
	public IOleControlImpl<CQFilter>,
	public IOleObjectImpl<CQFilter>,
	public IOleInPlaceActiveObjectImpl<CQFilter>,
	public IViewObjectExImpl<CQFilter>,
	public IOleInPlaceObjectWindowlessImpl<CQFilter>,
	public IQuickActivateImpl<CQFilter>,
	public IDataObjectImpl<CQFilter>,
    //## Persistance
	public IPersistStreamInitImpl<CQFilter>,
	public IPersistStorageImpl<CQFilter>,
    //## Property page implementation
	public ISpecifyPropertyPagesImpl<CQFilter>,
    //## Connection point container support
	public IConnectionPointContainerImpl<CQFilter>,
	public IProvideClassInfo2Impl<&CLSID_QFilter, &DIID__IQFilterEvents, &LIBID_QUICKFILTERLib>,
    //## Events and property change notifications
	public IPropertyNotifySinkCP<CQFilter>,
	public CProxy_IQFilterEvents<CQFilter>,
	//## Error Infor
	public ISupportErrorInfo
	//## Other
{
//## CONSTRUCTOR
public:
	CQFilter();
	~CQFilter();
	void ResetProperties();
	enum { IDD = IDD_QFILTER };

//## ASSERT
public:
	void AssertFilter();

//## MACRO
public:
DECLARE_REGISTRY_RESOURCEID(IDR_QFILTER)
DECLARE_PROTECT_FINAL_CONSTRUCT()
BEGIN_COM_MAP(CQFilter)
	COM_INTERFACE_ENTRY(IQFilter)
	COM_INTERFACE_ENTRY(IDispatch)
	COM_INTERFACE_ENTRY(IViewObjectEx)
	COM_INTERFACE_ENTRY(IViewObject2)
	COM_INTERFACE_ENTRY(IViewObject)
	COM_INTERFACE_ENTRY(IOleInPlaceObjectWindowless)
	COM_INTERFACE_ENTRY(IOleInPlaceObject)
	COM_INTERFACE_ENTRY2(IOleWindow, IOleInPlaceObjectWindowless)
	COM_INTERFACE_ENTRY(IOleInPlaceActiveObject)
	COM_INTERFACE_ENTRY(IOleControl)
	COM_INTERFACE_ENTRY(IOleObject)
	COM_INTERFACE_ENTRY(IPersistStreamInit)
	COM_INTERFACE_ENTRY2(IPersist, IPersistStreamInit)
	COM_INTERFACE_ENTRY(ISupportErrorInfo)
	COM_INTERFACE_ENTRY(IConnectionPointContainer)
	COM_INTERFACE_ENTRY(ISpecifyPropertyPages)
	COM_INTERFACE_ENTRY(IQuickActivate)
	COM_INTERFACE_ENTRY(IPersistStorage)
	COM_INTERFACE_ENTRY(IDataObject)
	COM_INTERFACE_ENTRY(IProvideClassInfo)
	COM_INTERFACE_ENTRY(IProvideClassInfo2)
END_COM_MAP()

BEGIN_PROP_MAP(CQFilter)
	PROP_DATA_ENTRY("_cx", m_sizeExtent.cx, VT_UI4)
	PROP_DATA_ENTRY("_cy", m_sizeExtent.cy, VT_UI4)

	PROP_PAGE(CLSID_QFilterPropPageGeneral)

	PROP_ENTRY("NoOfColumns",			DISPID_QFILTER_NOOFCOLUMNS,				CLSID_QFilterPropPageGeneral)
	PROP_ENTRY("NoOfCheckFilters",		DISPID_QFILTER_NOOFCHECKFILTERS,		CLSID_QFilterPropPageGeneral)
END_PROP_MAP()

BEGIN_CONNECTION_POINT_MAP(CQFilter)
	CONNECTION_POINT_ENTRY(DIID__IQFilterEvents)
	CONNECTION_POINT_ENTRY(IID_IPropertyNotifySink)
END_CONNECTION_POINT_MAP()

BEGIN_MSG_MAP(CQFilter)
	MESSAGE_HANDLER(WM_INITDIALOG,	OnInitDialog);
	MESSAGE_HANDLER(WM_DESTROY,		OnDestroy);
	MESSAGE_HANDLER(WM_SIZE,		OnSize);

	MESSAGE_HANDLER(WM_DRAWITEM,	OnDraw);

	CHAIN_MSG_MAP(CComCompositeControl<CQFilter>)
END_MSG_MAP()

BEGIN_SINK_MAP(CQFilter)
END_SINK_MAP()

//## INTERFACE IOleControlImpl
public:
	STDMETHOD(OnAmbientPropertyChange)(DISPID dispid);

//## INTERFACE ISupportsErrorInfo
public:
	STDMETHOD(InterfaceSupportsErrorInfo)(REFIID riid);

//## INTERFACE IViewObjectEx
public:
	DECLARE_VIEW_STATUS(0)

//## ABOUT
public:
    STDMETHODIMP AboutBox();

//## DATA
private:
	long m_nNoOfCheckFilters;
	enum NoOfColumnsValues m_nNoOfColumns;
	std::vector< CCheckComboBox* >	m_vecCheckFilters;
	std::vector< CWindow* >			m_vecCheckLabels;

//## MESSAGES
private:
	LRESULT OnInitDialog(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnDestroy(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnSize(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);
	LRESULT OnDraw(UINT uMsg, WPARAM wParam, LPARAM lParam, BOOL& bHandled);

//## METHODS
private:
	void CreateCheckFilter( CCheckComboBox *pwndCombo, long nIndex );
	void CreateCheckLabel( CWindow *pwndLabel, long nIndex );
	inline void GetCellControlRect( long &nRow, long &nCol, long &nRows, long &nCols, BOOL bLabel, RECT &rcParent, RECT &rc );

//## PROPERTIES
private:
	HRESULT ValidateProperty(DISPID dispid, void *pVal);
	HRESULT OnPutProperty(DISPID dispid, void *pVal);

public:
	DECLARE_SIMPLE_PROPERTY   (NoOfCheckFilters,	long)
	DECLARE_SIMPLE_PROPERTY   (NoOfColumns,			enum NoOfColumnsValues)
	DECLARE_SIMPLE_PROPERTY2  (CheckLabel,			BSTR, long, nFilter)
	DECLARE_SIMPLE_PROPERTY2  (DroppedWidth,		long, long, nFilter)
	DECLARE_SIMPLE_PROPERTY3  (Check,				VARIANT_BOOL, long, nFilter, long, nID)
	DECLARE_SIMPLE_PROPERTY2  (Field,				BSTR, long, nFilter)
	DECLARE_SIMPLE_PROPERTY_R2(CheckFilterIDs,		BSTR, long, nFilter)
	DECLARE_SIMPLE_PROPERTY_R2(CheckFilterTexts,	BSTR, long, nFilter)
	DECLARE_SIMPLE_PROPERTY_R2(SQLCheckFilter,		BSTR, long, nFilter)
	DECLARE_SIMPLE_PROPERTY_R2(TextCheckFilter,		BSTR, long, nFilter)
	DECLARE_SIMPLE_PROPERTY_R (SQLFilter,			BSTR)
	DECLARE_SIMPLE_PROPERTY_R (TextFilter,			BSTR)

//## IQFilter
public:
	STDMETHOD(CheckAll)(/*[in]*/ long nFilter, /*[in]*/ VARIANT_BOOL bCheck);
	STDMETHOD(AddString)(/*[in]*/ long nFilter, /*[in]*/ BSTR bstrString, /*[in]*/ long nID, /*[in]*/ long nLevel);
	STDMETHOD(AddFolder)(/*[in]*/ long nFilter, /*[in]*/ BSTR bstrFolder);
};
//#######################################################################################
#endif //__QFILTER_H_
//#######################################################################################
