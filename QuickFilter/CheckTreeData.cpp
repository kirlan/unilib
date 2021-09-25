//#######################################################################################
//## CheckTreeData.cpp : implementation file
//## [Magerusan G. Cosmin] 13-apr-2002 (Created)
//## [Magerusan G. Cosmin]  5-feb-2003 (Adapted for ATL)
//#######################################################################################
#include "stdafx.h"

#include "CheckTreeData.h"
//## ====================================================================================
CCheckTreeNode::CCheckTreeNode(LPWSTR lpszString)
{
	//## DEFAULT
	nID = INVALID_ID;
	nLevel = ROOT_LEVEL;
	bChecked = FALSE;
	bIsLeaf = FALSE;

	//## SET String
	strCaption = lpszString;
}	
//#######################################################################################
CCheckTreeData::CCheckTreeData()
{
	//## INITIALIZE Data
	Reset();
}
//## ====================================================================================
CCheckTreeData::~CCheckTreeData()
{
	//## RESET
	Reset();
}
//## ====================================================================================
void CCheckTreeData::Reset()
{
	//## RESET Content
	clear();

	//## INITIALIZE
	m_nMaxLevel = ROOT_LEVEL;
}
//#######################################################################################
void CCheckTreeData::AddString(LPWSTR lpszString, long nID, long nLevel)
{
	//## ASSERT
	ATLASSERT(nLevel < TREE_MAX_LEVELS);

	//## INITIALIZE Node
	CCheckTreeNode node(lpszString);
	node.nID = nID;
	node.nLevel = nLevel;
	node.bIsLeaf = (nID != INVALID_ID) ? TRUE : FALSE;

	//## UPDATE MaxLevel
	if (nLevel > m_nMaxLevel) m_nMaxLevel = nLevel;

	//## ADD Node
	push_back( node );
}
//#######################################################################################
BOOL CCheckTreeData::UpdateChecksDown(long nIndex)
{
	//## GET Current level
	long nIndexLevel = at(nIndex).nLevel;
	BOOL bIndexChecked = at(nIndex).bChecked;

	//## SCAN & Update
	BOOL bChange = FALSE;
	for(long i=max(nIndex+1, 0); (i < (long)size()) && (at(i).nLevel > nIndexLevel); i++)
		if (CLEAN_BOOL(at(i).bChecked) != CLEAN_BOOL(bIndexChecked)){
			at(i).bChecked = bIndexChecked;
			bChange = TRUE;
		}

	//## RETURN
	return bChange;
}
//## ====================================================================================
BOOL CCheckTreeData::UpdateChecksUp(long nLevel)
{
	//## DECLARE
	long nChecked = 0L, nUnChecked = 0L, nItemLevel = 0L;

	//## SCAN up -> down
	BOOL bChange = FALSE;
	for(long i = (size() - 1); i >= 0; i--){
		//## GET Item Level
		nItemLevel = at( i ).nLevel;

		//## COUNT checked/unchecked items
		if (nItemLevel == nLevel)
			if (at( i ).bChecked) nChecked++;
				else nUnChecked++;

		//## UP node
		if ((nItemLevel + 1) == nLevel){
			if ((nChecked > 0) || (nUnChecked > 0)){
				if (nUnChecked == 0) 
					if (!at( i ).bChecked){
						at( i ).bChecked= TRUE;
						bChange = TRUE;
					}
				if (nUnChecked > 0) 
					if (at( i ).bChecked){
						at( i ).bChecked = FALSE;
						bChange = TRUE;
					}
			}
			nChecked = nUnChecked = 0L;
		}
	}

	//## RETURN
	return bChange;
}
//## ====================================================================================
BOOL CCheckTreeData::UpdateChecksUp()
{
	//## UPDATE Levels
	BOOL bChange = FALSE;
	for(long i=m_nMaxLevel; i > 0; i--){
		if (UpdateChecksUp( i )) bChange = TRUE;
	}

	//## RETURN
	return bChange;
}
//#######################################################################################
BOOL CCheckTreeData::UpdateChecks(long nIndex)
{
	//## PROPAGATE Checks DOWN
	BOOL bChange = UpdateChecksDown(nIndex);

	//## PROPAGATE Checks UP
	return ( bChange & UpdateChecksUp() );
}
//#######################################################################################
void CCheckTreeData::CheckAll(BOOL bCheck)
{
	//## CHECK All Items
	for(long i=0; i<(long)size(); i++){
		at(i).bChecked = bCheck;
	}
}
//## ====================================================================================
BOOL CCheckTreeData::GetCheck(long nID)
{
	//## GET check
	for(long i=0; i<(long)size(); i++)
		if (at(i).nID == nID)
			return at(i).bChecked;

	//## ITEM not found
	return FALSE;
}
//## ====================================================================================
BOOL CCheckTreeData::GetCheckAtIndex(long nIndex)
{
	return at(nIndex).bChecked;
}
//## ====================================================================================
BOOL CCheckTreeData::SetCheck(long nID, BOOL bCheck)
{
	//## CHECK Item
	BOOL bChange = FALSE;
	long i;
	for(i=0; i<(long)size(); i++)
		if (at(i).nID == nID){
			if (CLEAN_BOOL(at(i).bChecked) != CLEAN_BOOL(bCheck)){
				at(i).bChecked = bCheck;
				bChange = TRUE;
			}
			break;
		}

	//## UPDATE Checks on levels, starting with node i
	if (i < (long)size())
		bChange &= UpdateChecks(i);
	return bChange;
}
//#######################################################################################
CComBSTR CCheckTreeData::GetCheckedTexts()
{
	//## ASSERT
	if (size() <= 0) return TEXT("");

	//## CASE: when only a single element is in the list
	CComBSTR bstr;
	if ((size() == 2) && (at(ROOT_INDEX).bChecked)){
		bstr = at(1).strCaption;
		return bstr;
	}

	//## CHECK if all items were Checked
	if (at(ROOT_INDEX).bChecked){
		bstr = ROOT_CAPTION;
		return bstr;
	}

	//## COLLECT all Checked items in a single string
	long nCount = 0;
	for(long i=0; i<(long)size(); i++)
		if ((at(i).bIsLeaf) && (at(i).bChecked)){
			if (bstr.Length() > 0) bstr += ", ";
			bstr += at(i).strCaption;
			nCount++;
		}

	//## ADJUST String
	if (nCount > 1){
		CComBSTR bstrReturn = "(";
		bstrReturn +=  bstr;
		bstrReturn += ")";
		return bstrReturn;
	}

	//## RETURN
	return bstr;
}
//## ====================================================================================
CComBSTR CCheckTreeData::GetCheckedIDs()
{
	//## ASSERT
	if (size() <= 0) return TEXT("");

	//## CHECK if all items were checked
	CComBSTR bstr;
	if (at(ROOT_INDEX).bChecked){
		bstr = ROOT_CAPTION;
		return bstr;
	}

	//## COLLECT all checked items in a single string
	long nCount = 0;
	for(long i=0; i<(long)size(); i++)
		if ((at(i).bIsLeaf) && (at(i).bChecked)){
			if (bstr.Length() > 0) bstr += ", ";

			static OLECHAR buffer[128];
			_ltow( at(i).nID, buffer, 10 );
			bstr += buffer;
			nCount++;
		}

	//## ADJUST String
	if (nCount > 0){
		CComBSTR bstrReturn = "(";
		bstrReturn +=  bstr;
		bstrReturn += ")";
		return bstrReturn;
	}

	//## RETURN
	return bstr;
}
//#######################################################################################
