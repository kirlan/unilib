template < class GDIObjectType >
class CGDIObject
{
public:
	
	CGDIObject( GDIObjectType hGDIObject = NULL )
	{
		_ASSERTE( hGDIObject );
		m_hGDIObject = hGDIObject;
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
		_ASSERTE( m_hGDIObject );
		::DeleteObject( m_hGDIObject );
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
		return static_cast< GDIObjectType >( m_hGDIObject );
	}

private:
	GDIObjectType m_hGDIObject;
};
