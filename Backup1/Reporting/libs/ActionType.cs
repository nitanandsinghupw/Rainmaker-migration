using System;

namespace Rainmaker.Reports 
	{
	public enum ActionType 
		{ 
		GET_SHIFT_LIST,							// 0
		CREATE_REPORT,							// 1
		GET_AGENT_LIST,							// 2
		GET_QUERY_LIST,							// 3
		CREATE_QUERY_VIEW,					// 4
		UPDATE_FIELD,								// 5
		GET_CSV,										// 6
		UPDATE_OPTION,							// 7
		UPDATE_COLUMN_HIDDEN,				// 8
		UPDATE_COLUMN_WIDTH, 				// 9
		RESET_HIDDEN,				 				// 10
		GET_SETTINGS_LIST,	 				// 11
		CREATE_SETTINGS,		 				// 12
		DELETE_SETTINGS,		 				// 13
		SAVE_SETTINGS,			 				// 14
		LOAD_SETTINGS				 				// 15
		};
	}
