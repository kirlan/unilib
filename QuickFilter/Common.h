//#######################################################################################
//## Common.cpp : Implementation of CQFilter
//## Author: Turc Alexandru
//## Modified by: Magerusan G. Cosmin (5-feb-2003)
//#######################################################################################
#ifndef __Common_h__
#define __Common_h__

#include "StdAfx.h"

/*
Used for automatic management of GDI objects
*/
template < class GDIObjectType >
class CGDIObject
{
public:
	
	CGDIObject( GDIObjectType hGDIObject = NULL )
	{
		_ASSERTE( hGDIObject );
		m_hObj = hGDIObject;
	}

	CGDIObject( const CGDIObject< GDIObjectType >& gdiObject )
	{
		/*
			Do not copy this object
		*/
		_ASSERTE( FALSE );
	}

	~CGDIObject()
	{
		_ASSERTE( m_hObj );
		::DeleteObject( m_hObj );
	}

	CGDIObject< GDIObjectType >& operator=( const CGDIObject< GDIObjectType >& gdiObject )
	{
		/*
		Do not assign this object
		*/
		_ASSERTE( FALSE );
		return *this;
	}

	operator GDIObjectType()
	{
		return static_cast< GDIObjectType >( m_hObj );
	}

	operator HANDLE()
	{
		return static_cast< HANDLE >( m_hObj );
	}

public:
	GDIObjectType m_hObj;
};

#endif // __Common_h__