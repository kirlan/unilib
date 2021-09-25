//#######################################################################################
//## QFilterEventsCP.h: declaration of events.
//#######################################################################################
#ifndef _QFILTEREVENTSCP_H_
#define _QFILTEREVENTSCP_H_
//## ====================================================================================
#include "MacrosEvent.h"
//#######################################################################################
template <class T>
class CProxy_IQFilterEvents : public IConnectionPointImpl<T, &DIID__IQFilterEvents, CComDynamicUnkArray>
{
public:
	FIRE_EVENT    (Change,		DISPID_QFILTER_CHANGE)
};
//#######################################################################################
#endif //_QFILTEREVENTSCP_H_
//#######################################################################################
