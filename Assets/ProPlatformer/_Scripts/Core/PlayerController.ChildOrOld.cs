namespace Myd.Platform
{
    public enum ChildOrOld
    {
        Child,
        Old
    }
    public partial class PlayerController
    {
        public ChildOrOld childOrOld = ChildOrOld.Child;
    }
}