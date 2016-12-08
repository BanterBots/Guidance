using GDApp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace GDLibrary
{
    //To use this class you will need to MANUALLY add the video.dll in the Dependencies folder.
    //Add under GDApp\References i.e. where we added SkinnedModel.dll
    public class Video3DController : Controller
    {
        private enum State : sbyte
        {
            Playing,
            Paused,
            Stopped,
            NeverPlayed
        }

        #region Variables
        private State videoState;
        private VideoPlayer videoPlayer;
        private Video video;
        private Texture2D startTexture;
        private string videoName;
        private float volumeIncrement;
        private float startVolume;

        #endregion

        #region Properties
        public VideoPlayer VideoPlayer
        {
            get
            {
                return videoPlayer;
            }
        }
        public Video Video
        { 
            get
            {
                return video;
            }
            set
            {
                video = value; 
            }
        }
        public string name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }
        #endregion

        public Video3DController(string id, Actor parentActor, Texture2D startTexture, 
            Video video, string videoName, float volumeIncrement, float startVolume)
            : base(id, parentActor)
        {
            //used to idenitify video when we receive events (i.e. am i supposed to play my video?)
            this.videoName = videoName;
            //amount by which to raise/lower video volume
            this.volumeIncrement = volumeIncrement;
            //video WMV file
            this.video = video;
            
            //set initial texture?
            this.startTexture = startTexture;
            SetTexture(startTexture);

            //set initial volume 0 - 1
            this.startVolume = startVolume;

            //register for events
            game.EventDispatcher.VideoChanged += EventDispatcher_Video;

            Set(State.NeverPlayed);
        }

        private void Set(State videoState)
        {
            this.videoState = videoState;
        }

        public virtual void EventDispatcher_Video(EventData eventData)
        {
            VideoEventData e = eventData as VideoEventData;

            //use the ID to send event to a particular controller based on controller ID and for a specific video name
            if (e.ID.Equals(this.ID) && e.Name.Equals(this.videoName)) 
                ProcessEvent(e);
        }

        private void ProcessEvent(VideoEventData videoEventData)
        {
            if (this.videoPlayer == null)
            {
                this.videoPlayer = new VideoPlayer();
                this.videoPlayer.Volume = 0.1f;
            }

            if (videoEventData.EventType == EventType.OnPlay)
            {

                if (this.videoPlayer.State != MediaState.Playing)
                {
                    this.videoPlayer.Play(video);
                    this.videoState = State.Playing;
                }
            }
            else if (videoEventData.EventType == EventType.OnPause)
            {
                if (this.videoPlayer.State == MediaState.Playing)
                {
                    this.videoPlayer.Pause();
                    this.videoState = State.Paused;
                }
            }
            else if (videoEventData.EventType == EventType.OnStop)
            {
                if ((this.videoPlayer.State == MediaState.Playing) || (this.videoPlayer.State == MediaState.Paused))
                {
                    this.videoPlayer.Stop();
                    this.videoState = State.Stopped;
                }
            }
            else if (videoEventData.EventType == EventType.OnVolumeUp)
            {
                float newVolume = this.videoPlayer.Volume + volumeIncrement;
                this.videoPlayer.Volume = (newVolume <= 1) ? newVolume : 1;
            }
            else if (videoEventData.EventType == EventType.OnVolumeDown)
            {
                float newVolume = this.videoPlayer.Volume - volumeIncrement;
                this.videoPlayer.Volume = (newVolume >= 0) ? newVolume : 0;
            }
        }

        private void SetTexture(Texture2D texture)
        {
            if (this.ParentActor is TexturedPrimitiveObject) //its either a billboard, a simple textured primitive, or a model
                (this.ParentActor as TexturedPrimitiveObject).Texture = texture;
            else
                (this.ParentActor as ModelObject).Texture = texture;
        }
        public override void Update(GameTime gameTime)
        {
            if(videoState == State.Playing)
               SetTexture(videoPlayer.GetTexture());
            else if (videoState == State.Stopped)
                SetTexture(startTexture);
        }


        public void Dispose()
        {
            this.videoPlayer.Dispose();
        }
    }
}
