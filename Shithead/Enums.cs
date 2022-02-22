using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Shithead.Enums
{
    public enum Shape 
    {
        D,
        H,
        S,
        C
    };

    public enum Undo 
    { 
        DrawCard,
        ComputerThrowCard,
        PlayerThrowCard,
        ComputerTakeCard,
        PlayerTakeCard,
        ThrowTen 
    };

    public enum Level 
    {
        Easy,
        Normal,
        Hard 
    }

    public enum ShowComputerCards
    {
        Yes,
        No 
    };
}
