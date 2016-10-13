/*
Function: 		Stores common hard-coded variable values used within the game e.g. key mappings, mouse sensitivity
Author: 		NMCG
Version:		1.0
Date Updated:	5/10/16
Bugs:			None
Fixes:			None
*/

namespace _3DTileEngine
{
    public class AppData
    {
        //defines how much the mouse has to move in pixels before a movement is registered - see MouseManager::HasMoved()
        public const float MouseSensitivity = 1;
    }
}
