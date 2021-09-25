//#######################################################################################
//## MacrosPP.h :	My Macros for Property Pages
//#######################################################################################
#ifndef __MACROS_PP_H_
#define __MACROS_PP_H_

//## [MCosmin] 13-May-2002 ==============================================================
#define IMG_WIDTH 40
#define INITIALIZE_TAB(__TAB__, __tab__)\
	m_tab##__tab__ = GetDlgItem(IDC_TAB_##__TAB__);\
	m_img##__tab__.Create(IDB_BITMAP_##__TAB__, IMG_WIDTH, 1, RGB(0x55, 0x55, 0x55));\
	m_tab##__tab__.SetImageList( m_img##__tab__.m_hImageList );
//## ====================================================================================

//## ====================================================================================
#define INITIALIZE_EDIT(__EDIT__, __edit__)\
	m_txt##__edit__ = GetDlgItem(IDC_EDIT_##__EDIT__);\
//## ====================================================================================

//## ====================================================================================
#define INITIALIZE_CHECK(__CHECK__, __check__)\
	m_chk##__check__ = GetDlgItem(IDC_CHECK_##__CHECK__);\
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define CHANGE_ENTRY(DispID, Prefix, Property, Cast)\
	m_##Prefix##Property##New = m_tab##Property.GetCurSel();\
\
    if (m_##Prefix##Property##New != m_##Prefix##Property##Orig)\
			m_flags |=  DispID##_CHANGED;\
    else	m_flags &= ~DispID##_CHANGED;
//## ====================================================================================

//## [MCosmin] 10-Feb-2003 ==============================================================
#define CHANGE_ENTRY_N(DispID, Prefix, Property, Cast, nn)\
	m_##Prefix##Property##New = m_tab##Property.GetCurSel() + nn;\
\
    if (m_##Prefix##Property##New != m_##Prefix##Property##Orig)\
			m_flags |=  DispID##_CHANGED;\
    else	m_flags &= ~DispID##_CHANGED;
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define CHANGE_ENTRY_CHECK(DispID, Prefix, Property, Cast)\
	m_##Prefix##Property##New = (m_chk##Property.GetCheck() == BST_CHECKED);\
\
    if (m_##Prefix##Property##New != m_##Prefix##Property##Orig)\
			m_flags |=  DispID##_CHANGED;\
    else	m_flags &= ~DispID##_CHANGED;
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define CHANGE_ENTRY_FREE_INT(DispID, ControlID, Prefix, Property, Cast)\
	m_##Prefix##Property##New = GetDlgItemInt(ControlID, &bTranslated);\
\
    if (m_##Prefix##Property##New != m_##Prefix##Property##Orig)\
			m_flags |=  DispID##_CHANGED;\
    else	m_flags &= ~DispID##_CHANGED;
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define INITIALIZE_CASE_N(DispID, Prefix, Property, Cast, nn)\
    if (DispID == dispid || DISPID_UNKNOWN == dispid) {\
        /*## GET Appearance property */\
        HRESULT hr = pQFilter->get_##Property( (Cast*)&m_##Prefix##Property##Orig );\
        ATLASSERT (SUCCEEDED (hr));\
        m_##Prefix##Property##New = m_##Prefix##Property##Orig;\
\
		/*## SET it */\
        if (IsWindow ()) {\
			m_tab##Property.SetCurSel( m_##Prefix##Property##Orig - nn );\
        }\
    }
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define INITIALIZE_CASE_CHECK(DispID, Prefix, Property, Cast)\
    if (DispID == dispid || DISPID_UNKNOWN == dispid) {\
        /*## GET Appearance property */\
        HRESULT hr = pQFilter->get_##Property( (Cast*)&m_##Prefix##Property##Orig );\
        ATLASSERT (SUCCEEDED (hr));\
        m_##Prefix##Property##New = m_##Prefix##Property##Orig;\
\
		/*## SET it */\
        if (IsWindow ()) {\
			m_chk##Property.SetCheck( (m_##Prefix##Property##Orig) ? BST_CHECKED : BST_UNCHECKED );\
        }\
    }
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define INITIALIZE_CASE_FREE_INT(DispID, ControlID, Prefix, Property, Cast)\
    if (DispID == dispid || DISPID_UNKNOWN == dispid) {\
        /*## GET Appearance property */\
        HRESULT hr = pQFilter->get_##Property( (Cast*)&m_##Prefix##Property##Orig );\
        ATLASSERT (SUCCEEDED (hr));\
        m_##Prefix##Property##New = m_##Prefix##Property##Orig;\
\
		/*## SET it */\
        if (IsWindow ()) {\
			SetDlgItemInt( ControlID, m_##Prefix##Property##Orig );\
        }\
    }
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define INITIALIZE_CASE_BOOL(DispID, Prefix, Property, Cast)\
    if (DispID == dispid || DISPID_UNKNOWN == dispid) {\
        /*## GET Appearance property */\
        HRESULT hr = pQFilter->get_##Property( (Cast*)&m_##Prefix##Property##Orig );\
        ATLASSERT (SUCCEEDED (hr));\
		m_##Prefix##Property##Orig = m_##Prefix##Property##Orig ? (1) : (0);\
        m_##Prefix##Property##New = m_##Prefix##Property##Orig;\
\
		/*## SET it */\
        if (IsWindow ()) {\
			m_tab##Property.SetCurSel( m_##Prefix##Property##Orig );\
        }\
    }
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define SET_CASE(DispID, Prefix, Property, Cast)\
        /*## Update the Appearance property, if requested and required */\
        if ((DispID == dispid || DISPID_UNKNOWN == dispid) && (m_flags & DispID##_CHANGED)){\
            pQFilter->put_##Property( (Cast)m_##Prefix##Property##New );	/* Update property */\
            m_##Prefix##Property##Orig = m_##Prefix##Property##New;		/* Prop page and property synced */\
            m_flags &= ~DispID##_CHANGED;								/* Clear changed flag */\
        }
//## ====================================================================================

//## [MCosmin] 13-May-2002 ==============================================================
#define FOCUS_ENTRY(__DISPID__, __CONTROL_ID__)\
        case __DISPID__:\
			if (!GetDlgItem(__CONTROL_ID__)) break;\
            ::SetFocus(GetDlgItem(__CONTROL_ID__));\
            break;\
//## ====================================================================================

//#######################################################################################
#endif //__MACROS_PP_H_
//#######################################################################################
