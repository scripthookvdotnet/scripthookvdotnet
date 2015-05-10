#pragma once

#include "Vector3.hpp"

namespace GTA
{
	public ref class Blip sealed
	{
	public:
		Blip(int id);

		property int ID
		{
			int get();
		}
		property Math::Vector3 Position 
		{
			Math::Vector3 get(); // Vector3 GET_BLIP_INFO_ID_COORD(Any p0) // 0xB7374A66
			void set(Math::Vector3 value); // void SET_BLIP_COORDS(Any p0, Any p1, Any p2, Any p3) // 0x680A34D4
		}

		bool Exists(); // BOOL DOES_BLIP_EXIST(Any p0) // 0xAE92DD96
		void SetAsFriendly(); // void SET_BLIP_AS_FRIENDLY(int BlipID, BOOL toggle) // 0xF290CFD8
		void SetAsHostile(); // void SET_BLIP_AS_FRIENDLY(int BlipID, BOOL toggle) // 0xF290CFD8
		void Remove(); // void REMOVE_BLIP(int BlipID) // 0xD8C3C1CD
		
		
		// BOOL IS_BLIP_ON_MINIMAP(Any p0) // 0x258CBA3A
		// void SET_BLIP_AS_SHORT_RANGE(Any p0, Any p1) // 0x5C67725E 
		// Vector3 GET_BLIP_COORDS(Any p0) // 0xEF6FF47B
		// void SHOW_NUMBER_ON_BLIP(Any p0, Any p1) // 0x7BFC66C6
		// void HIDE_NUMBER_ON_BLIP(Any p0) // 0x0B6D610D
		// void SET_NEW_WAYPOINT(Any p0, Any p1) // 0x8444E1F0
		// BOOL IS_WAYPOINT_ACTIVE() // 0x5E4DF47B
		// void SET_WAYPOINT_OFF() // 0xB3496E1B
		// void SET_POLICE_RADAR_BLIPS(BOOL Toggle) // 0x8E114B10
		// void SET_THIS_SCRIPT_CAN_REMOVE_BLIPS_CREATED_BY_ANY_SCRIPT(Any p0) // 0xD06F1720
		// void BLIP_SIREN(Any p0) // 0xC0EB6924
		// void SET_BLIP_ROUTE(Object blip, int enabled) // 0x3E160C90
		// Any GET_NUMBER_OF_ACTIVE_BLIPS() // 0x144020FA
		// Any GET_NEXT_BLIP_INFO_ID(Any p0) // 0x9356E92F
		// Any GET_FIRST_BLIP_INFO_ID(Any p0) // 0x64C0273D 
		// Object GET_BLIP_FROM_ENTITY(Entity entity) // 0x005A2A47
		//  Any ADD_BLIP_FOR_ENTITY(Entity entity) // 0x30822554
		// Any ADD_BLIP_FOR_COORD(float x, float y, float z) // 0xC6F43D0E
		// void SET_BLIP_SPRITE(Object blip, int spriteId) // 0x8DBBB0B9
		// Any GET_BLIP_SPRITE(Any p0) // 0x72FF2E73
		// void SET_BLIP_ALPHA(Any p0, Any p1) // 0xA791FCCD
		// Any GET_BLIP_ALPHA(Any p0) // 0x297AF6C8
		// BOOL DOES_PED_HAVE_AI_BLIP(Any p0) // 0x3BE1257F
	private:
		int mID;
	};
}