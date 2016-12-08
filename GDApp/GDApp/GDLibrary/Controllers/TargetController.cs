
namespace GDLibrary
{
    public class TargetController : Controller
    {
        #region Fields
        private Actor targetActor;
        #endregion

        #region Properties
        public Actor TargetActor
        {
            get
            {
                return targetActor;
            }
            set
            {
                targetActor = value;
            }
        }

        #endregion

        public TargetController(string id, Actor parentActor, Actor targetActor)
            : base(id, parentActor)
        {
            this.targetActor = targetActor;
        }

        public override object Clone()
        {
            return new TargetController("clone - " + this.ID,
                this.ParentActor, //shallow - reset normally
                this.targetActor); //shallow - cloned rail should have same target
        }
    }
}
