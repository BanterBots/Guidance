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
        public static string StringMenuSplitScreen = "SplitScreen";   //"SplitScreen"
        public static string StringMenuOnline = "Online";   //"Online"
        public static string StringMenuHost = "Host";
        public static string StringMenuClient = "Client";
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
        internal static int TextureIndexOnlineMenu = 4;

        //bounding rectangles used to detect mouse over
        public static Rectangle BoundsMenuPlay = new Rectangle(295, 200, 230, 85); //x, y, width, height
        public static Rectangle BoundsMenuSplitScreen = new Rectangle(600, 200, 230, 85);
        public static Rectangle BoundsMenuOnline = new Rectangle(30, 200, 230, 85);
        public static Rectangle BoundsMenuRestart = new Rectangle(50, 100, 230, 85);
        public static Rectangle BoundsMenuAudio = new Rectangle(295, 330, 230, 85);
        public static Rectangle BoundsMenuControls = new Rectangle(295, 200, 230, 85);
        public static Rectangle BoundsMenuExit = new Rectangle(295, 460, 230, 85);

        public static Rectangle BoundsMenuBack = new Rectangle(295, 460, 230, 85);
        public static Rectangle BoundsMenuVolumeUp = new Rectangle(540, 200, 100, 90);
        public static Rectangle BoundsMenuVolumeDown = new Rectangle(175, 200, 100, 90);
        public static Rectangle BoundsMenuVolumeMute = new Rectangle(295, 330, 230, 85);

        public static Rectangle BoundsMenuExitYes = new Rectangle(130, 200, 180, 85);
        public static Rectangle BoundsMenuExitNo = new Rectangle(530, 200, 180, 85);

        public static Rectangle BoundsMenuHost = new Rectangle(175, 200, 220, 100);
        public static Rectangle BoundsMenuClient = new Rectangle(540, 200, 220, 100);
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
