using System;

namespace Miner.Input
{
    public interface IInputManager
    {
        int SelectorRow { get; }
        int SelectorCol { get; }

        event SelectorMoved SelectorMoved;
    }

    public delegate void SelectorMoved(object sender, EventArgs e);
}
