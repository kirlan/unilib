

/* this ALWAYS GENERATED file contains the definitions for the interfaces */


 /* File created by MIDL compiler version 7.00.0555 */
/* at Tue Jul 12 15:24:50 2011
 */
/* Compiler settings for QuickFilter.idl:
    Oicf, W1, Zp8, env=Win32 (32b run), target_arch=X86 7.00.0555 
    protocol : dce , ms_ext, c_ext, robust
    error checks: allocation ref bounds_check enum stub_data 
    VC __declspec() decoration level: 
         __declspec(uuid()), __declspec(selectany), __declspec(novtable)
         DECLSPEC_UUID(), MIDL_INTERFACE()
*/
/* @@MIDL_FILE_HEADING(  ) */

#pragma warning( disable: 4049 )  /* more than 64k source lines */


/* verify that the <rpcndr.h> version is high enough to compile this file*/
#ifndef __REQUIRED_RPCNDR_H_VERSION__
#define __REQUIRED_RPCNDR_H_VERSION__ 475
#endif

#include "rpc.h"
#include "rpcndr.h"

#ifndef __RPCNDR_H_VERSION__
#error this stub requires an updated version of <rpcndr.h>
#endif // __RPCNDR_H_VERSION__


#ifndef __QuickFilter_h__
#define __QuickFilter_h__

#if defined(_MSC_VER) && (_MSC_VER >= 1020)
#pragma once
#endif

/* Forward Declarations */ 

#ifndef __IQFilter_FWD_DEFINED__
#define __IQFilter_FWD_DEFINED__
typedef interface IQFilter IQFilter;
#endif 	/* __IQFilter_FWD_DEFINED__ */


#ifndef ___IQFilterEvents_FWD_DEFINED__
#define ___IQFilterEvents_FWD_DEFINED__
typedef interface _IQFilterEvents _IQFilterEvents;
#endif 	/* ___IQFilterEvents_FWD_DEFINED__ */


#ifndef __QFilter_FWD_DEFINED__
#define __QFilter_FWD_DEFINED__

#ifdef __cplusplus
typedef class QFilter QFilter;
#else
typedef struct QFilter QFilter;
#endif /* __cplusplus */

#endif 	/* __QFilter_FWD_DEFINED__ */


#ifndef __QFilterPropPageGeneral_FWD_DEFINED__
#define __QFilterPropPageGeneral_FWD_DEFINED__

#ifdef __cplusplus
typedef class QFilterPropPageGeneral QFilterPropPageGeneral;
#else
typedef struct QFilterPropPageGeneral QFilterPropPageGeneral;
#endif /* __cplusplus */

#endif 	/* __QFilterPropPageGeneral_FWD_DEFINED__ */


/* header files for imported files */
#include "oaidl.h"
#include "ocidl.h"

#ifdef __cplusplus
extern "C"{
#endif 



#ifndef __QUICKFILTERLib_LIBRARY_DEFINED__
#define __QUICKFILTERLib_LIBRARY_DEFINED__

/* library QUICKFILTERLib */
/* [helpstring][version][uuid] */ 

typedef 
enum NoOfColumnsValues
    {	fil2Columns	= 2,
	fil3Columns	= 3,
	fil4Columns	= 4
    } 	_NoOfColumnsValues;


enum __MIDL___MIDL_itf_QuickFilter_0001_0064_0001
    {	DISPID_QFILTER_CHANGE	= 1
    } ;

EXTERN_C const IID LIBID_QUICKFILTERLib;

#ifndef __IQFilter_INTERFACE_DEFINED__
#define __IQFilter_INTERFACE_DEFINED__

/* interface IQFilter */
/* [unique][helpstring][dual][uuid][object] */ 


enum __MIDL_IQFilter_0001
    {	DISPID_QFILTER_NOOFCHECKFILTERS	= 1,
	DISPID_QFILTER_NOOFCOLUMNS	= ( DISPID_QFILTER_NOOFCHECKFILTERS + 1 ) ,
	DISPID_QFILTER_CHECKLABEL	= ( DISPID_QFILTER_NOOFCOLUMNS + 1 ) ,
	DISPID_QFILTER_DROPPEDWIDTH	= ( DISPID_QFILTER_CHECKLABEL + 1 ) ,
	DISPID_QFILTER_CHECK	= ( DISPID_QFILTER_DROPPEDWIDTH + 1 ) ,
	DISPID_QFILTER_FIELD	= ( DISPID_QFILTER_CHECK + 1 ) ,
	DISPID_QFILTER_CHECKFILTERIDS	= ( DISPID_QFILTER_FIELD + 1 ) ,
	DISPID_QFILTER_CHECKFILTERTEXTS	= ( DISPID_QFILTER_CHECKFILTERIDS + 1 ) ,
	DISPID_QFILTER_SQLCHECKFILTER	= ( DISPID_QFILTER_CHECKFILTERTEXTS + 1 ) ,
	DISPID_QFILTER_TEXTCHECKFILTER	= ( DISPID_QFILTER_SQLCHECKFILTER + 1 ) ,
	DISPID_QFILTER_SQLFILTER	= ( DISPID_QFILTER_TEXTCHECKFILTER + 1 ) ,
	DISPID_QFILTER_TEXTFILTER	= ( DISPID_QFILTER_SQLFILTER + 1 ) ,
	DISPID_QFILTER_ADDFOLDER	= ( DISPID_QFILTER_TEXTFILTER + 1 ) ,
	DISPID_QFILTER_ADDSTRING	= ( DISPID_QFILTER_ADDFOLDER + 1 ) ,
	DISPID_QFILTER_CHECKALL	= ( DISPID_QFILTER_ADDSTRING + 1 ) 
    } ;

EXTERN_C const IID IID_IQFilter;

#if defined(__cplusplus) && !defined(CINTERFACE)
    
    MIDL_INTERFACE("3A8AAEB8-5798-4ED2-8B58-9D92703F999E")
    IQFilter : public IDispatch
    {
    public:
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NoOfCheckFilters( 
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NoOfCheckFilters( 
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_NoOfColumns( 
            /* [retval][out] */ enum NoOfColumnsValues *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_NoOfColumns( 
            /* [in] */ enum NoOfColumnsValues newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CheckLabel( 
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_CheckLabel( 
            /* [in] */ long nFilter,
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_DroppedWidth( 
            /* [in] */ long nFilter,
            /* [retval][out] */ long *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_DroppedWidth( 
            /* [in] */ long nFilter,
            /* [in] */ long newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Check( 
            /* [in] */ long nFilter,
            /* [in] */ long nID,
            /* [retval][out] */ VARIANT_BOOL *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Check( 
            /* [in] */ long nFilter,
            /* [in] */ long nID,
            /* [in] */ VARIANT_BOOL newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_Field( 
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propput] */ HRESULT STDMETHODCALLTYPE put_Field( 
            /* [in] */ long nFilter,
            /* [in] */ BSTR newVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CheckFilterIDs( 
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_CheckFilterTexts( 
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SQLCheckFilter( 
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_TextCheckFilter( 
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_SQLFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id][propget] */ HRESULT STDMETHODCALLTYPE get_TextFilter( 
            /* [retval][out] */ BSTR *pVal) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddFolder( 
            /* [in] */ long nFilter,
            /* [in] */ BSTR bstrFolder) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AddString( 
            /* [in] */ long nFilter,
            /* [in] */ BSTR bstrString,
            /* [in] */ long nID,
            /* [in] */ long nLevel) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE CheckAll( 
            /* [in] */ long nFilter,
            /* [in] */ VARIANT_BOOL bCheck) = 0;
        
        virtual /* [helpstring][id] */ HRESULT STDMETHODCALLTYPE AboutBox( void) = 0;
        
    };
    
#else 	/* C style interface */

    typedef struct IQFilterVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            IQFilter * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            IQFilter * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            IQFilter * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            IQFilter * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            IQFilter * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            IQFilter * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            IQFilter * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NoOfCheckFilters )( 
            IQFilter * This,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NoOfCheckFilters )( 
            IQFilter * This,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_NoOfColumns )( 
            IQFilter * This,
            /* [retval][out] */ enum NoOfColumnsValues *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_NoOfColumns )( 
            IQFilter * This,
            /* [in] */ enum NoOfColumnsValues newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CheckLabel )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_CheckLabel )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_DroppedWidth )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ long *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_DroppedWidth )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ long newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Check )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ long nID,
            /* [retval][out] */ VARIANT_BOOL *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Check )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ long nID,
            /* [in] */ VARIANT_BOOL newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_Field )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propput] */ HRESULT ( STDMETHODCALLTYPE *put_Field )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ BSTR newVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CheckFilterIDs )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_CheckFilterTexts )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SQLCheckFilter )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TextCheckFilter )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_SQLFilter )( 
            IQFilter * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id][propget] */ HRESULT ( STDMETHODCALLTYPE *get_TextFilter )( 
            IQFilter * This,
            /* [retval][out] */ BSTR *pVal);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddFolder )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ BSTR bstrFolder);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AddString )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ BSTR bstrString,
            /* [in] */ long nID,
            /* [in] */ long nLevel);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *CheckAll )( 
            IQFilter * This,
            /* [in] */ long nFilter,
            /* [in] */ VARIANT_BOOL bCheck);
        
        /* [helpstring][id] */ HRESULT ( STDMETHODCALLTYPE *AboutBox )( 
            IQFilter * This);
        
        END_INTERFACE
    } IQFilterVtbl;

    interface IQFilter
    {
        CONST_VTBL struct IQFilterVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define IQFilter_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define IQFilter_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define IQFilter_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define IQFilter_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define IQFilter_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define IQFilter_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define IQFilter_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 


#define IQFilter_get_NoOfCheckFilters(This,pVal)	\
    ( (This)->lpVtbl -> get_NoOfCheckFilters(This,pVal) ) 

#define IQFilter_put_NoOfCheckFilters(This,newVal)	\
    ( (This)->lpVtbl -> put_NoOfCheckFilters(This,newVal) ) 

#define IQFilter_get_NoOfColumns(This,pVal)	\
    ( (This)->lpVtbl -> get_NoOfColumns(This,pVal) ) 

#define IQFilter_put_NoOfColumns(This,newVal)	\
    ( (This)->lpVtbl -> put_NoOfColumns(This,newVal) ) 

#define IQFilter_get_CheckLabel(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_CheckLabel(This,nFilter,pVal) ) 

#define IQFilter_put_CheckLabel(This,nFilter,newVal)	\
    ( (This)->lpVtbl -> put_CheckLabel(This,nFilter,newVal) ) 

#define IQFilter_get_DroppedWidth(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_DroppedWidth(This,nFilter,pVal) ) 

#define IQFilter_put_DroppedWidth(This,nFilter,newVal)	\
    ( (This)->lpVtbl -> put_DroppedWidth(This,nFilter,newVal) ) 

#define IQFilter_get_Check(This,nFilter,nID,pVal)	\
    ( (This)->lpVtbl -> get_Check(This,nFilter,nID,pVal) ) 

#define IQFilter_put_Check(This,nFilter,nID,newVal)	\
    ( (This)->lpVtbl -> put_Check(This,nFilter,nID,newVal) ) 

#define IQFilter_get_Field(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_Field(This,nFilter,pVal) ) 

#define IQFilter_put_Field(This,nFilter,newVal)	\
    ( (This)->lpVtbl -> put_Field(This,nFilter,newVal) ) 

#define IQFilter_get_CheckFilterIDs(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_CheckFilterIDs(This,nFilter,pVal) ) 

#define IQFilter_get_CheckFilterTexts(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_CheckFilterTexts(This,nFilter,pVal) ) 

#define IQFilter_get_SQLCheckFilter(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_SQLCheckFilter(This,nFilter,pVal) ) 

#define IQFilter_get_TextCheckFilter(This,nFilter,pVal)	\
    ( (This)->lpVtbl -> get_TextCheckFilter(This,nFilter,pVal) ) 

#define IQFilter_get_SQLFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_SQLFilter(This,pVal) ) 

#define IQFilter_get_TextFilter(This,pVal)	\
    ( (This)->lpVtbl -> get_TextFilter(This,pVal) ) 

#define IQFilter_AddFolder(This,nFilter,bstrFolder)	\
    ( (This)->lpVtbl -> AddFolder(This,nFilter,bstrFolder) ) 

#define IQFilter_AddString(This,nFilter,bstrString,nID,nLevel)	\
    ( (This)->lpVtbl -> AddString(This,nFilter,bstrString,nID,nLevel) ) 

#define IQFilter_CheckAll(This,nFilter,bCheck)	\
    ( (This)->lpVtbl -> CheckAll(This,nFilter,bCheck) ) 

#define IQFilter_AboutBox(This)	\
    ( (This)->lpVtbl -> AboutBox(This) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */




#endif 	/* __IQFilter_INTERFACE_DEFINED__ */


#ifndef ___IQFilterEvents_DISPINTERFACE_DEFINED__
#define ___IQFilterEvents_DISPINTERFACE_DEFINED__

/* dispinterface _IQFilterEvents */
/* [helpstring][uuid] */ 


EXTERN_C const IID DIID__IQFilterEvents;

#if defined(__cplusplus) && !defined(CINTERFACE)

    MIDL_INTERFACE("C2536D98-C6E8-4F31-B7FE-2AE079CB31A9")
    _IQFilterEvents : public IDispatch
    {
    };
    
#else 	/* C style interface */

    typedef struct _IQFilterEventsVtbl
    {
        BEGIN_INTERFACE
        
        HRESULT ( STDMETHODCALLTYPE *QueryInterface )( 
            _IQFilterEvents * This,
            /* [in] */ REFIID riid,
            /* [annotation][iid_is][out] */ 
            __RPC__deref_out  void **ppvObject);
        
        ULONG ( STDMETHODCALLTYPE *AddRef )( 
            _IQFilterEvents * This);
        
        ULONG ( STDMETHODCALLTYPE *Release )( 
            _IQFilterEvents * This);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfoCount )( 
            _IQFilterEvents * This,
            /* [out] */ UINT *pctinfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetTypeInfo )( 
            _IQFilterEvents * This,
            /* [in] */ UINT iTInfo,
            /* [in] */ LCID lcid,
            /* [out] */ ITypeInfo **ppTInfo);
        
        HRESULT ( STDMETHODCALLTYPE *GetIDsOfNames )( 
            _IQFilterEvents * This,
            /* [in] */ REFIID riid,
            /* [size_is][in] */ LPOLESTR *rgszNames,
            /* [range][in] */ UINT cNames,
            /* [in] */ LCID lcid,
            /* [size_is][out] */ DISPID *rgDispId);
        
        /* [local] */ HRESULT ( STDMETHODCALLTYPE *Invoke )( 
            _IQFilterEvents * This,
            /* [in] */ DISPID dispIdMember,
            /* [in] */ REFIID riid,
            /* [in] */ LCID lcid,
            /* [in] */ WORD wFlags,
            /* [out][in] */ DISPPARAMS *pDispParams,
            /* [out] */ VARIANT *pVarResult,
            /* [out] */ EXCEPINFO *pExcepInfo,
            /* [out] */ UINT *puArgErr);
        
        END_INTERFACE
    } _IQFilterEventsVtbl;

    interface _IQFilterEvents
    {
        CONST_VTBL struct _IQFilterEventsVtbl *lpVtbl;
    };

    

#ifdef COBJMACROS


#define _IQFilterEvents_QueryInterface(This,riid,ppvObject)	\
    ( (This)->lpVtbl -> QueryInterface(This,riid,ppvObject) ) 

#define _IQFilterEvents_AddRef(This)	\
    ( (This)->lpVtbl -> AddRef(This) ) 

#define _IQFilterEvents_Release(This)	\
    ( (This)->lpVtbl -> Release(This) ) 


#define _IQFilterEvents_GetTypeInfoCount(This,pctinfo)	\
    ( (This)->lpVtbl -> GetTypeInfoCount(This,pctinfo) ) 

#define _IQFilterEvents_GetTypeInfo(This,iTInfo,lcid,ppTInfo)	\
    ( (This)->lpVtbl -> GetTypeInfo(This,iTInfo,lcid,ppTInfo) ) 

#define _IQFilterEvents_GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId)	\
    ( (This)->lpVtbl -> GetIDsOfNames(This,riid,rgszNames,cNames,lcid,rgDispId) ) 

#define _IQFilterEvents_Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr)	\
    ( (This)->lpVtbl -> Invoke(This,dispIdMember,riid,lcid,wFlags,pDispParams,pVarResult,pExcepInfo,puArgErr) ) 

#endif /* COBJMACROS */


#endif 	/* C style interface */


#endif 	/* ___IQFilterEvents_DISPINTERFACE_DEFINED__ */


EXTERN_C const CLSID CLSID_QFilter;

#ifdef __cplusplus

class DECLSPEC_UUID("9DC527A9-6C42-46F7-BD63-B82A60602D3F")
QFilter;
#endif

EXTERN_C const CLSID CLSID_QFilterPropPageGeneral;

#ifdef __cplusplus

class DECLSPEC_UUID("A1F7015A-ACFD-A36c-ADCC-A026EB203889")
QFilterPropPageGeneral;
#endif
#endif /* __QUICKFILTERLib_LIBRARY_DEFINED__ */

/* Additional Prototypes for ALL interfaces */

/* end of Additional Prototypes */

#ifdef __cplusplus
}
#endif

#endif


