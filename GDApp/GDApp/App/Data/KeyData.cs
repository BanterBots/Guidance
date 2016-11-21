
using Microsoft.Xna.Framework.Input;
namespace GDApp
{
    public class KeyData
    {
        #region First Person Camera or Player Move Keys and Indices
        public static int KeysIndexMoveForward = 0;
        public static int KeysIndexMoveBackward = 1;

        public static int KeysIndexRotateLeft = 2;
        public static int KeysIndexRotateRight = 3;

        public static int KeysIndexMoveJump = 4;
        public static int KeysIndexMoveCrouch = 5;

        public static Keys[] MoveKeys 
                            = {
                                  Keys.W, Keys.S, //forward, bacward
                                  Keys.A, Keys.D,  //turn left, right
                                  Keys.Space, Keys.C}; //jump, crouch

        public static Keys[] MoveKeysOther
                         = {
                                  Keys.T, Keys.G, //forward, bacward
                                  Keys.F, Keys.H,  //turn left, right
                                  Keys.Space, Keys.C}; //jump, crouch

        public static Keys[] MoveKeysAnimated
                         = {
                                  Keys.I, Keys.K, //forward, bacward
                                  Keys.J, Keys.L,  //turn left, right
                                  Keys.Space, Keys.C}; //jump, crouch


        public static Keys KeyPauseShowMenu = Keys.Escape;
      
        #endregion


    }
}
