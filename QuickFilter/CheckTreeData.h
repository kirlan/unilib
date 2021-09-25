//#######################################################################################
//## CheckTreeData.h : header file
//## [Magerusan G. Cosmin] 20-apr-2002 (Created)
//## [Magerusan G. Cosmin]  5-feb-2003 (Adapted for ATL)
//#######################################################################################
#if !defined(__CHECKTREEDATA_H__)
#define __CHECKTREEDATA_H__

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
//## ====================================================================================
#define INVALID_ID 0xFFFFFFFFL
//## ====================================================================================
#define ROOT_LEVEL 0L
#define ROOT_INDEX 0L
#define ROOT_CAPTION TEXT("*")
#define TREE_MAX_LEVELS 22L
//#######################################################################################
class CCheckTreeNode
{
//## CONSTRUCTOR
public:
	CCheckTreeNode(LPWSTR lpszString = L"");

//## STATE
public:
	CComBSTR strCaption;
	long nID;
	long nLevel;
	BOOL bChecked;
	BOOL bIsLeaf;
};
//#######################################################################################
class CCheckTreeData : public std::vector < CCheckTreeNode > 
{	
//## CONSTRUCTOR
public:
	CCheckTreeData();
	virtual ~CCheckTreeData();

//## ATTRIBUTES
private:
	long m_nMaxLevel;

//## METHODS
private:
	BOOL UpdateChecksDown(long nIndex);
	BOOL UpdateChecksUp(long nLevel);
	BOOL UpdateChecksUp();

public:
	void Reset();
	void AddString(LPWSTR lpszString, long nID = INVALID_ID, long nLevel = ROOT_LEVEL + 1);
	long GetMaxLevel(){ return m_nMaxLevel; };

//## SELECTION Specific
public:
	BOOL UpdateChecks(long nIndex);
	void CheckAll(BOOL bSelect);
	BOOL GetCheck(long nID);
	BOOL GetCheckAtIndex(long nIndex);
	BOOL SetCheck(long nID, BOOL bSelect);
	CComBSTR GetCheckedTexts();
	CComBSTR GetCheckedIDs();
};
//#######################################################################################
#endif // !defined(__CHECKTREEDATA_H__)
//#######################################################################################
