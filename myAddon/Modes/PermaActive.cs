#region

using Settings = myAddon.Config.Misc;

#endregion

namespace myAddon.Modes
{
    public sealed class PermaActive : ModeBase
    {
        public override bool ShouldBeExecuted()
        {
            // Since this is permaactive mode, always execute the loop
            return true;
        }

        public override void Execute()
        {
        }
    }
}