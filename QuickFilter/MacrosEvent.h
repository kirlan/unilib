//#######################################################################################
//## MacrosEvent.h :	My Macros for Events
//## [MCosmin] 18-May-2002 
//#######################################################################################
#ifndef __MACROS_EVENT_H_
#define __MACROS_EVENT_H_

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT(Event, DispID)\
    VOID Fire_On##Event()\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                DISPPARAMS disp = { NULL, NULL, 0, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT1(Event, DispID, type_parameter0, parameter0)\
    VOID Fire_On##Event(type_parameter0 parameter0)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[1];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[0] = parameter0;\
                DISPPARAMS disp = { pvars, NULL, 1, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT1_EX(Event, DispID, type_parameter0, parameter0, vtAtribute0, vtType0)\
    VOID Fire_On##Event(type_parameter0 parameter0)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[1];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[0].vtAtribute0 = parameter0;\
				pvars[0].vt = vtType0;\
                DISPPARAMS disp = { pvars, NULL, 1, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT2(Event, DispID, type_parameter0, parameter0, type_parameter1, parameter1)\
    VOID Fire_On##Event(type_parameter0 parameter0, type_parameter1 parameter1)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[2];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[1] = parameter0;\
                pvars[0] = parameter1;\
                DISPPARAMS disp = { pvars, NULL, 2, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT2_EX(Event, DispID, type_parameter0, parameter0, vtAtribute0, vtType0, type_parameter1, parameter1, vtAtribute1, vtType1)\
    VOID Fire_On##Event(type_parameter0 parameter0, type_parameter1 parameter1)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[2];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[1].vtAtribute0 = parameter0;\
				pvars[1].vt = vtType0;\
                pvars[0].vtAtribute1 = parameter1;\
				pvars[0].vt = vtType1;\
                DISPPARAMS disp = { pvars, NULL, 2, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT3_EX(Event, DispID, type_parameter0, parameter0, vtAtribute0, vtType0, type_parameter1, parameter1, vtAtribute1, vtType1, type_parameter2, parameter2, vtAtribute2, vtType2)\
    VOID Fire_On##Event(type_parameter0 parameter0, type_parameter1 parameter1, type_parameter2 parameter2)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[3];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[2].vtAtribute0 = parameter0;\
				pvars[2].vt = vtType0;\
                pvars[1].vtAtribute1 = parameter1;\
				pvars[1].vt = vtType1;\
                pvars[0].vtAtribute2 = parameter2;\
				pvars[0].vt = vtType2;\
                DISPPARAMS disp = { pvars, NULL, 3, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 1-June-2002 ==============================================================
#define FIRE_EVENT5_EX(Event, DispID, type_parameter0, parameter0, vtAtribute0, vtType0, type_parameter1, parameter1, vtAtribute1, vtType1, type_parameter2, parameter2, vtAtribute2, vtType2, type_parameter3, parameter3, vtAtribute3, vtType3, type_parameter4, parameter4, vtAtribute4, vtType4)\
    VOID Fire_On##Event(type_parameter0 parameter0, type_parameter1 parameter1, type_parameter2 parameter2, type_parameter3 parameter3, type_parameter4 parameter4)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[5];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[4].vtAtribute0 = parameter0;\
				pvars[4].vt = vtType0;\
                pvars[3].vtAtribute1 = parameter1;\
				pvars[3].vt = vtType1;\
                pvars[2].vtAtribute2 = parameter2;\
				pvars[2].vt = vtType2;\
                pvars[1].vtAtribute3 = parameter3;\
				pvars[1].vt = vtType3;\
                pvars[0].vtAtribute4 = parameter4;\
				pvars[0].vt = vtType4;\
                DISPPARAMS disp = { pvars, NULL, 5, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT3(Event, DispID, type_parameter0, parameter0, type_parameter1, parameter1, type_parameter2, parameter2)\
    VOID Fire_On##Event(type_parameter0 parameter0, type_parameter1 parameter1, type_parameter2 parameter2)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[3];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[2] = parameter0;\
                pvars[1] = parameter1;\
                pvars[0] = parameter2;\
                DISPPARAMS disp = { pvars, NULL, 3, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//## [MCosmin] 18-May-2002 ==============================================================
#define FIRE_EVENT4(Event, DispID, type_parameter0, parameter0, type_parameter1, parameter1, type_parameter2, parameter2, type_parameter3, parameter3)\
    VOID Fire_On##Event(type_parameter0 parameter0, type_parameter1 parameter1, type_parameter2 parameter2, type_parameter3 parameter3)\
    {\
        T* pT = static_cast<T*>(this);\
\
        int nConnectionIndex;\
        CComVariant* pvars = new CComVariant[4];\
        int nConnections = m_vec.GetSize();\
\
        for (nConnectionIndex = 0; nConnectionIndex < nConnections; nConnectionIndex++)\
        {\
            pT->Lock();\
            CComPtr<IUnknown> sp = m_vec.GetAt(nConnectionIndex);\
            pT->Unlock();\
            IDispatch* pDispatch = reinterpret_cast<IDispatch*>(sp.p);\
            if (pDispatch != NULL)\
            {\
                pvars[3] = parameter0;\
                pvars[2] = parameter1;\
                pvars[1] = parameter2;\
                pvars[0] = parameter3;\
                DISPPARAMS disp = { pvars, NULL, 4, 0 };\
                pDispatch->Invoke(DispID, IID_NULL, LOCALE_USER_DEFAULT, DISPATCH_METHOD, &disp, NULL, NULL, NULL);\
            }\
        }\
        delete[] pvars;\
    }
//## ====================================================================================

//#######################################################################################
#endif //__MACROS_EVENT_H_
//#######################################################################################
