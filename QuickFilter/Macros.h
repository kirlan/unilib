//#######################################################################################
//## Macros.h :	My Macros
//#######################################################################################
#ifndef __MACROS_H_
#define __MACROS_H_


//## ====================================================================================
#define COLOR_SYS 0x80000000
#define TWIPS_PER_INCH 1440
#define EVENT_AFTER_TAB_KEY 0x0080L
#define EVENT_SPACE_KEY_ON_CHECKBOX 0x0081L
//## ====================================================================================
#define SIMPLE_BUFFER_SIZE 256
//## ====================================================================================
#define DIM(a) (sizeof(a)/sizeof(a[0]))
#define BASECHARS (DIM(szValueNameBase) - 1)
#define CLEAN_BOOL(b) (b != 0)
//## ====================================================================================
typedef enum tagKEYMODIFIERS{
	KEYMOD_NONE    = 0X00000000,
	KEYMOD_SHIFT   = 0x00000001,
	KEYMOD_CONTROL = 0x00000002,
	KEYMOD_ALT     = 0x00000004
}KEYMODIFIERS;
//## ====================================================================================
enum{
	VISIBLE_NEXT = 1,
	VISIBLE_PREV,
	VISIBLE_FIRST,
	VISIBLE_LAST
};
//## ====================================================================================


//#######################################################################################
//## TRY CATCH Macros [MCosmin] 9-may-2002
//#######################################################################################
//## DEFINE the following if you want try/catches to be enabled
#define HANDLE_TRY_CATCH

//## [MCosmin] 9-may-2002
#ifndef HANDLE_TRY_CATCH
#define ____TRY
#else
#define ____TRY BOOL bCatched = 0;\
try{ SetLastError(0);
#endif

//## [MCosmin] 9-may-2002
#ifndef HANDLE_TRY_CATCH
#define ____CATCH_SILENT
#define ____CATCH(Function)
#define ____CATCH_ALL(Function)
#else
#define ____CATCH_SILENT }catch(...){};
#define ____CATCH(Function) }catch(...){ ShowLastError(Function); bCatched = TRUE; };
#define ____CATCH_ALL(Function) ____CATCH(Function)\
if ((!bCatched) && (GetLastError() != 0)) ShowLastError(Function);
#endif

#ifdef UNICODE
#define STR(operation)\
	wcs##operation
#else
#define STR(operation)\
	str##operation
#endif

static void ShowLastError(LPTSTR szFunction = NULL)
{
	//## FUNCTION
	TCHAR szBuffer[SIMPLE_BUFFER_SIZE];
	if (szFunction){
		STR(cpy)(szBuffer, TEXT("Function: "));
		STR(cat)(szBuffer, szFunction);
	}

	//## GET Last Error Number
	static long err; err = GetLastError();
	if (err == 0){
		STR(cat)(szBuffer, TEXT("\nWarning: Unspecified!"));
		::MessageBox(NULL, szBuffer, TEXT("QFilter"), MB_OK);
		return;
	}

	//## FORMAT Error Message
	LPVOID lpMsgBuf;
	FormatMessage( 
		FORMAT_MESSAGE_ALLOCATE_BUFFER | 
		FORMAT_MESSAGE_FROM_SYSTEM | 
		FORMAT_MESSAGE_IGNORE_INSERTS,
		NULL,
		GetLastError(),
		0,
		(LPTSTR) &lpMsgBuf,
		0,
		NULL 
	);

	//## SHOW Error Message
	TCHAR *szMessage = new TCHAR[STR(len)(szBuffer) + STR(len)(TEXT("\nError: ")) + STR(len)((LPTSTR)lpMsgBuf) + 1];
		STR(cpy)(szMessage, szBuffer);
		STR(cat)(szMessage, TEXT("\nError: "));
		STR(cat)(szMessage, (LPTSTR)lpMsgBuf);
	::MessageBox(NULL, szMessage, TEXT("QFilter"), MB_OK);
	delete [] szMessage;

	//## FREE
	LocalFree( lpMsgBuf );
}
//#######################################################################################

//#######################################################################################
//## SIMPLE PROPERY Macro [MCosmin] 11-may-2002
//#######################################################################################

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY_R(PropertyName, PropertyType)\
    STDMETHODIMP get_##PropertyName(/*[out, retval]*/PropertyType *pVal);

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY_R2(PropertyName, PropertyType, PropertyType0, PropertyName0)\
    STDMETHODIMP get_##PropertyName(/*[in]*/PropertyType0 PropertyName0, /*[out, retval]*/PropertyType *pVal);\

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY_W(PropertyName, PropertyType)\
    STDMETHODIMP put_##PropertyName(/*[in]*/PropertyType newVal);

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY_W2(PropertyName, PropertyType, PropertyType0, PropertyName0)\
    STDMETHODIMP put_##PropertyName(/*[in]*/PropertyType0 PropertyName0, /*[in]*/PropertyType newVal);

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY_F(PropertyName, PropertyType)\
    STDMETHODIMP putref_##PropertyName(/*[out]*/PropertyType Val);

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY(PropertyName, PropertyType)\
	DECLARE_SIMPLE_PROPERTY_R(PropertyName, PropertyType)\
	DECLARE_SIMPLE_PROPERTY_W(PropertyName, PropertyType)

//## ====================================================================================
#define DECLARE_RANGE_PROPERTY_W(PropertyName, PropertyType)\
    STDMETHODIMP put_##PropertyName(/*[in]*/long nRow1, /*[in]*/long nCol1, /*[in]*/long nRow2, /*[in]*/long nCol2, /*[in]*/PropertyType newVal);

//## ====================================================================================
#define DECLARE_RANGE_PROPERTY_F(PropertyName, PropertyType)\
    STDMETHODIMP putref_##PropertyName(/*[in]*/long nRow1, /*[in]*/long nCol1, /*[in]*/long nRow2, /*[in]*/long nCol2, /*[in]*/PropertyType newVal);

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY2(PropertyName, PropertyType, PropertyType0, PropertyName0)\
	DECLARE_SIMPLE_PROPERTY_R2(PropertyName, PropertyType, PropertyType0, PropertyName0)\
	DECLARE_SIMPLE_PROPERTY_W2(PropertyName, PropertyType, PropertyType0, PropertyName0)\

//## ====================================================================================
#define DECLARE_SIMPLE_PROPERTY3(PropertyName, PropertyType, PropertyType0, PropertyName0, PropertyType1, PropertyName1)\
    STDMETHODIMP get_##PropertyName(/*[in]*/PropertyType1 PropertyName1, /*[in]*/PropertyType0 PropertyName0, /*[out, retval]*/PropertyType *pVal);\
    STDMETHODIMP put_##PropertyName(/*[in]*/PropertyType1 PropertyName1, /*[in]*/PropertyType0 PropertyName0, /*[in]*/PropertyType newVal);

//## ====================================================================================
#define IMPLEMENT_PROPERTY(PropertyName, Prefix, DispID, PropertyType, Cast)\
STDMETHODIMP CQFilter::get_##PropertyName(/*[out]*/PropertyType *pVal)\
{\
	if( pVal == NULL ) return E_POINTER;\
	*pVal = m_##Prefix##PropertyName;\
	return S_OK;\
}\
\
STDMETHODIMP CQFilter::put_##PropertyName(PropertyType newVal)\
{\
	AssertFilter();\
	HRESULT hr = ValidateProperty(DispID, &newVal);\
	if (!SUCCEEDED(hr)) return hr;\
\
    if (!m_nFreezeEvents)\
        if (FireOnRequestEdit(DispID) == S_FALSE)\
            return S_FALSE;\
\
	m_##Prefix##PropertyName = newVal;\
	OnPutProperty(DispID, &newVal);\
    m_bRequiresSave = TRUE;\
\
	FireOnChanged( DispID );\
	FireViewChange();\
    SendOnDataChange(NULL);\
\
	return S_OK;\
}

//## ====================================================================================
#define IMPLEMENT_STOCK_PROPERTY(PropertyName, Prefix, DispID, PropertyType, Cast)\
STDMETHODIMP CQFilter::get_##PropertyName(/*[out]*/PropertyType *pVal)\
{\
	if( pVal == NULL ) return E_POINTER;\
	*pVal = m_##Prefix##PropertyName;\
	return S_OK;\
}\
\
STDMETHODIMP CQFilter::put_##PropertyName(PropertyType newVal)\
{\
	AssertFilter();\
	HRESULT hr = ValidateProperty(DispID, &newVal);\
	if (!SUCCEEDED(hr)) return hr;\
\
	m_##Prefix##PropertyName = newVal;\
	OnPutProperty(DispID, &newVal);\
    m_bRequiresSave = TRUE;\
\
	FireOnChanged( DispID );\
	FireViewChange();\
    SendOnDataChange(NULL);\
\
	return S_OK;\
}

//## ====================================================================================
#define IMPLEMENT_SIMPLE_PROPERTY(PropertyName, Prefix, DispID, PropertyType, Cast)\
STDMETHODIMP CQFilter::get_##PropertyName(/*[out]*/PropertyType *pVal)\
{\
	if( pVal == NULL ) return E_POINTER;\
	*pVal = m_##Prefix##PropertyName;\
	return S_OK;\
}\
\
STDMETHODIMP CQFilter::put_##PropertyName(PropertyType newVal)\
{\
	AssertFilter();\
	HRESULT hr = ValidateProperty(DispID, &newVal);\
	if (!SUCCEEDED(hr)) return hr;\
\
    if (!m_nFreezeEvents)\
        if (FireOnRequestEdit(DispID) == S_FALSE)\
            return S_FALSE;\
\
	m_##Prefix##PropertyName = newVal;\
	OnPutProperty(DispID, &newVal);\
    m_bRequiresSave = TRUE;\
\
	FireOnChanged( DispID );\
	FireViewChange();\
    SendOnDataChange(NULL);\
\
	return S_OK;\
}
//## ====================================================================================

//## ====================================================================================
#define IMPLEMENT_ROWCOL_PROPERTY(RowCol, PropertyName, Prefix, DispID, PropertyType, Cast)\
STDMETHODIMP CQFilter::get_##RowCol##PropertyName(long nIndex, /*[out]*/PropertyType *pVal)\
{\
	if( pVal == NULL ) return E_POINTER;\
	if ((nIndex < 0) || (nIndex >= m_arr##RowCol##Info.GetSize())) return E_INVALIDARG;\
\
	*pVal = m_arr##RowCol##Info.At(nIndex).m_##Prefix##PropertyName;\
\
	return S_OK;\
}\
\
STDMETHODIMP CQFilter::put_##RowCol##PropertyName(long nIndex, PropertyType newVal)\
{\
	if ((nIndex < 0) || (nIndex >= m_arr##RowCol##Info.GetSize())) return E_INVALIDARG;\
	HRESULT hr = ValidateProperty(DispID, &newVal);\
	if (!SUCCEEDED(hr)) return hr;\
\
	m_arr##RowCol##Info.At(nIndex).m_##Prefix##PropertyName = newVal;\
	OnPutProperty(DispID, &nIndex); /*## The index is needed */\
\
	return S_OK;\
}

//#######################################################################################
//## PREDEFINED STRINGS / VALUE Macro [MCosmin] 13-may-2002
//#######################################################################################
#define PREDEFINED_STRINGS_CASE(DispID, Property)\
	case DispID:\
		/*## ASSERT */\
		ATLASSERT (c##Property##Strings == c##Property##Cookies);\
		ATLASSERT (c##Property##Strings == c##Property##Values);\
\
		/*## GET Strings */\
		hr = SetStrings(c##Property##Values, rsz##Property##Strings, pcaStringsOut);\
		if (FAILED (hr)) return hr;\
		return SetCookies(c##Property##Values, rdw##Property##Cookies, pcaCookiesOut);
//## ====================================================================================
#define PREDEFINED_VALUES_CASE(DispID, Property)\
	case DispID:\
		/*## WALK through cookie array looking for matching value */\
		for (i = 0; i < c##Property##Cookies; i++){\
			if (rdw##Property##Cookies[i] == dwCookie){\
				pVarOut->vt = VT_I4;\
				pVarOut->lVal = rvb##Property##Values[i];\
				return S_OK;\
			}\
		}\
		return E_INVALIDARG;
//#######################################################################################

//#######################################################################################
//## PICTURE LOAD / SAVE MACROS
//#######################################################################################
#define USES_PICTURE\
	PICTDESC pictDesc; HRESULT hr;\
	memset(&pictDesc, 0, sizeof(pictDesc));\
	pictDesc.cbSizeofstruct = sizeof(pictDesc);\
	pictDesc.picType = PICTYPE_BITMAP;\
	pictDesc.bmp.hpal = 0;
//## ====================================================================================
#define LOAD_BITMAP(PictureName, ID_BITMAP)\
	pictDesc.picType = PICTYPE_BITMAP;\
	pictDesc.bmp.hbitmap = ::LoadBitmap( _Module.GetModuleInstance(), MAKEINTRESOURCE(ID_BITMAP) );\
	hr = OleCreatePictureIndirect( &pictDesc, IID_IDispatch, TRUE, (void**)&m_pPic##PictureName );\
	if (hr != S_OK) m_pPic##PictureName = NULL;
//## ====================================================================================
#define LOAD_ICON(PictureName, ID_ICON)\
	pictDesc.picType = PICTYPE_ICON;\
	pictDesc.icon.hicon = ::LoadIcon( _Module.GetModuleInstance(), MAKEINTRESOURCE(ID_ICON) );\
	hr = OleCreatePictureIndirect( &pictDesc, IID_IDispatch, TRUE, (void**)&m_pPic##PictureName );\
	if (hr != S_OK) m_pPic##PictureName = NULL;
//## ====================================================================================
#define RELEASE_PICTURE(PictureName)\
	if (m_pPic##PictureName)\
		m_pPic##PictureName->Release();
//#######################################################################################

//#######################################################################################
#endif //__MACROS_H_
//#######################################################################################
