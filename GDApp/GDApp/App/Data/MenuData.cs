using System;
using Microsoft.Xna.Framework;


namespace GDLibrary
{
    public class MenuData
    {
        #region Main Menu Strings
        //all the strings shown to the user through the menu
        public static String Game_Title = "Guidance";
        public static String Menu_Play = "Play"; //"Play"
        public static string StringMenuRestart = "Restart"; //"Restart"
        public static String StringMenuSave = "Save";   //"Save"
        public static String StringMenuAudio = "Options";   //"Options"
        public static String StringMenuControls = "Controls";   //"Controls"
        public static String StringMenuExit = "Exit";   //"Exit"

        public static String StringMenuVolumeUp = "+";  //"+"
        public static String StringMenuVolumeDown = "-";    //"-"
        public static string StringMenuVolumeMute = "Mute On/Off";  //"Mute On/Off"
        public static String StringMenuBack = "Back";   //"Back"

        public static string StringMenuExitYes = "Yes"; //"Yes"
        public static string StringMenuExitNo = "No";   //"No"
        #endregion

        #region Colours, Padding, Texture transparency , Array Indices and Bounds
        public static Integer2 MenuTexturePadding = new Integer2(10, 10);
        public static Color MenuTextureColor = new Color(1, 1, 1, 0.9f);

        //the hover colours for menu items
        public static Color ColorMenuInactive = Color.Black;
        public static Color ColorMenuActive = Color.Red;

        //the position of the texture in the array of textures provided to the menu manager
        public static int TextureIndexMainMenu = 0;
        public static int TextureIndexAudioMenu = 1;
        public static int TextureIndexControlsMenu = 2;
        public static int TextureIndexExitMenu = 3;

        //bounding rectangles used to detect mouse over
        public static Rectangle BoundsMenuPlay = new Rectangle(370, 250, 290, 110); //x, y, width, height
        public static Rectangle BoundsMenuRestart = new Rectangle(50, 100, 290, 110);
        public static Rectangle BoundsMenuAudio = new Rectangle(370, 410, 290, 110);
        public static Rectangle BoundsMenuControls = new Rectangle(50, 200, 290, 110);
        public static Rectangle BoundsMenuExit = new Rectangle(370, 575, 290, 110);

        public static Rectangle BoundsMenuBack = new Rectangle(370, 575, 290, 110);
        public static Rectangle BoundsMenuVolumeUp = new Rectangle(690, 250, 140, 100);
        public static Rectangle BoundsMenuVolumeDown = new Rectangle(220, 250, 140, 100);
        public static Rectangle BoundsMenuVolumeMute = new Rectangle(370, 410, 290, 110);

        public static Rectangle BoundsMenuExitYes = new Rectangle(157, 250, 220, 100);
        public static Rectangle BoundsMenuExitNo = new Rectangle(650, 250, 220, 100);
        #endregion

        #region UI Menu
        public static String UI_Menu_AddHouse = "Add House";
        public static string UI_Menu_AddBarracks = "Add Barracks";
        public static string UI_Menu_AddFence = "Add Fence";

        public static Rectangle UI_Menu_AddHouse_Bounds = new Rectangle(40, 380, 90, 20);
        public static Rectangle UI_Menu_AddBarracks_Bounds = new Rectangle(40, 400, 120, 20);
        public static Rectangle UI_Menu_AddFence_Bounds = new Rectangle(40, 420, 90, 20);


        #endregion

    }
}
