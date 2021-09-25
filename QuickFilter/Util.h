//#######################################################################################
//## Util.h :	Some functions
//#######################################################################################
#ifndef __UTIL_H_
#define __UTIL_H_
//#######################################################################################
static void LoadStringIntoBuffer(long nID, LPTSTR sz, long nDim = 255);
static HRESULT SetStrings(/*[in*/ ULONG cElems, /*[in]*/ const LPCOLESTR* rgpsz, /*[out]*/ CALPOLESTR* pStrings);
static HRESULT SetCookies(/*[in*/ ULONG cElems, /*[in*/ const DWORD* pdwCookies, /*[out]*/ CADWORD* pcaCookies);
//#######################################################################################
static void LoadStringIntoBuffer(long nID, LPTSTR sz, long nDim)
{
	//## ASSERT
	sz[0] = '\0';

	//## LOAD String
    LoadString(_Module.m_hInstResource, nID, sz, nDim);
}
//#######################################################################################
/**************/
/* SetStrings */
/**************/

// Copy array of strings to counted array of strings
static HRESULT SetStrings(/*[in*/ ULONG cElems, /*[in]*/ const LPCOLESTR* rgpsz, /*[out]*/ CALPOLESTR* pStrings)
{
    ATLASSERT (0 != cElems);
    ATLASSERT (NULL != rgpsz);
    ATLASSERT (NULL != pStrings);

    pStrings->cElems = 0;

    // Allocate array of string pointers
    ULONG len = cElems * sizeof(LPOLESTR);
    pStrings->pElems = (LPOLESTR*) CoTaskMemAlloc (len);
    if (NULL == pStrings->pElems) {
        return E_OUTOFMEMORY;
    }

#ifdef _DEBUG
    // Initialize the array to zero
    ::ZeroMemory (pStrings->pElems, len);
#endif

    // Allocate each string
    for (ULONG i = 0; i < cElems; i++) {
        // Allocate memory for the string and terminating NUL character
        ULONG len = (ocslen(rgpsz[i]) + 1) * sizeof(OLECHAR);
        pStrings->pElems[i] = (LPOLESTR) CoTaskMemAlloc (len);

        if (NULL == pStrings->pElems[i]) return E_OUTOFMEMORY;
        ocscpy(pStrings->pElems[i], rgpsz[i]);
        pStrings->cElems++;
    }
    return S_OK;
}

/**************/
/* SetCookies */
/**************/

// Set counted array of DWORDs to provided DWORD cookies
static HRESULT SetCookies(/*[in*/ ULONG cElems, /*[in*/ const DWORD* pdwCookies, /*[out]*/ CADWORD* pcaCookies)
{
    ATLASSERT (0 != cElems);
    ATLASSERT (NULL != pcaCookies);

    pcaCookies->cElems = 0;

    // Allocate array of cookies
    ULONG len = cElems * sizeof(DWORD);
    pcaCookies->pElems = (LPDWORD) CoTaskMemAlloc (len);
    if (NULL == pcaCookies->pElems) {
        return E_OUTOFMEMORY;
    }

    pcaCookies->cElems = cElems;
    ::CopyMemory (pcaCookies->pElems, pdwCookies, cElems * sizeof (DWORD));

    return S_OK;
}
//#######################################################################################
//#######################################################################################
#endif //__UTIL_H_
//#######################################################################################
