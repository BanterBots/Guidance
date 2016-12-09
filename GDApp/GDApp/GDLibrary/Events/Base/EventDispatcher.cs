using System.Collections.Generic;
using Microsoft.Xna.Framework;
using GDApp;

namespace GDLibrary
{
    public class EventDispatcher : GameComponent
    {
        private static Stack<EventData> stack;
        private static HashSet<EventData> uniqueSet;


        //one delegate for each category type
        public delegate void MainMenuEventHandler(EventData eventData);
        public delegate void UIMenuEventHandler(EventData eventData);
        public delegate void VideoEventHandler(EventData eventData);
        public delegate void SoundEventHandler(EventData eventData);
        public delegate void TextRenderEventHandler(EventData eventData);
        public delegate void ZoneEventHandler(EventData eventData);
        public delegate void CameraEventHandler(EventData eventData);
        public delegate void PlayerEventHandler(EventData eventData);
        public delegate void NonPlayerEventHandler(EventData eventData);
        public delegate void PickupEventHandler(EventData eventData);

        //normally at least one event for each category type
        public event MainMenuEventHandler MainMenuChanged;
        public event UIMenuEventHandler UIMenuChanged;
        public event VideoEventHandler VideoChanged;
        public event SoundEventHandler SoundChanged;
        public event TextRenderEventHandler TextRenderChanged;
        public event ZoneEventHandler ZoneChanged;
        public event CameraEventHandler CameraChanged;
        public event PlayerEventHandler PlayerChanged;
        public event NonPlayerEventHandler NonPlayerChanged;
        public event PickupEventHandler PickupChanged;

        public EventDispatcher(Main game, int initialSize)
            : base(game)
        {
            stack = new Stack<EventData>(initialSize);
            uniqueSet = new HashSet<EventData>(new EventDataEqualityComparer());
        }

        public static void Publish(EventData eventData)
        {
            //this prevents the same event being added multiple times within a single update e.g. 10x bell ring sounds
            if (!uniqueSet.Contains(eventData))
            {
                stack.Push(eventData);
                uniqueSet.Add(eventData);
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (int i = 0; i < stack.Count; i++)
                Process(stack.Pop());

            stack.Clear();
            uniqueSet.Clear();

            base.Update(gameTime);
        }

        private void Process(EventData eventData)
        {
            //Switch - See https://msdn.microsoft.com/en-us/library/06tc147t.aspx
            //one case for each category type
            switch (eventData.EventCategoryType)
            {
                case EventCategoryType.MainMenu:
                    OnMainMenu(eventData);
                    break;

                case EventCategoryType.UIMenu:
                    OnUIMenu(eventData);
                    break;

                case EventCategoryType.Video:
                    OnVideo(eventData);
                    break;

                case EventCategoryType.Sound:
                    OnSound(eventData);
                    break;

                case EventCategoryType.TextRender:
                    OnTextRender(eventData);
                    break;

                case EventCategoryType.Zone:
                    OnZone(eventData);
                    break;

                case EventCategoryType.Camera:
                    OnCamera(eventData);
                    break;

                case EventCategoryType.Player:
                    OnPlayer(eventData);
                    break;

                case EventCategoryType.NonPlayer:
                    OnNonPlayer(eventData);
                    break;

                case EventCategoryType.Pickup:
                    OnPickup(eventData);
                    break;
                    
                //add a case to handle the On...() method for each type

                default:
                    break;
            }
        }

        //called when a pickup is collected
        private void OnPickup(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (PickupChanged != null)
                PickupChanged(eventData);
        }

        //called when a nonplayer event needs to be generated e.g. non player dies
        private void OnNonPlayer(EventData eventData)
        {
            if (NonPlayerChanged != null)
                NonPlayerChanged(eventData);
        }

        //called when a player event needs to be generated e.g. win
        private void OnPlayer(EventData eventData)
        {
            if (PlayerChanged != null)
                PlayerChanged(eventData);
        }

        //called when a zone trigger event needs to be generated
        private void OnZone(EventData eventData)
        {
            if (ZoneChanged != null)
                ZoneChanged(eventData);
        }

        //called when a sound event needs to be generated e.g. play explosion
        private void OnSound(EventData eventData)
        {
            if (SoundChanged != null)
                SoundChanged(eventData);
        }

        //called when a menu event needs to be generated e.g. click inventory item
        protected virtual void OnUIMenu(EventData eventData)
        {
            if (UIMenuChanged != null)
                UIMenuChanged(eventData);
        }

        //called when a menu event needs to be generated e.g. click exit
        protected virtual void OnMainMenu(EventData eventData)
        {
            if (MainMenuChanged != null)
                MainMenuChanged(eventData);
        }
        //called when a camera event needs to be generated
        protected virtual void OnCamera(EventData eventData)
        {
            if (CameraChanged != null)
                CameraChanged(eventData);
        }

        //called when a video event needs to be generated e.g. play, pause, restart
        protected virtual void OnVideo(EventData eventData)
        {
            if (VideoChanged != null)
                VideoChanged(eventData);
        }

        //called when a text renderer event needs to be generated e.g. alarm in sector 2
        protected virtual void OnTextRender(EventData eventData)
        {
            //non-null if an object has subscribed to this event
            if (TextRenderChanged != null)
                TextRenderChanged(eventData);
        }
    }
}
