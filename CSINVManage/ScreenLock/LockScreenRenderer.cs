using System;
using System.Drawing;

/******************* CREATED BY ************************
*                                                      *
*   _____      _                _____          _       *
*  / ____|    | |              / ____|        | |      *
* | |    _   _| |__   ___ _ __| |     ___   __| | ___  *
* | |   | | | | '_ \ / _ \ '__| |    / _ \ / _` |/ _ \ *
* | |___| |_| | |_) |  __/ |  | |___| (_) | (_| |  __/ *
*  \_____\__, |_.__/ \___|_|   \_____\___/ \__,_|\___| *
*        __/ |                                         *
*       |___/                                          *
*                                                      *
*       https://www.youtube.com/user/CyberCode0        *
*                                                      *
********************************************************      
*/

namespace GestureLockApp.GestureLockControl
{
    [Flags]
    public enum GesturePadState
    {
        None = 0,
        Hovered = 1,
        Inputted = 2,
        LastInputted = 4,
        FirstInputted = 8
    }

    public abstract class LockScreenRenderer
    {
        public abstract void RenderBackground(Graphics graphics, RectangleF bounds, Color backColor);

        public abstract void RenderGesture(Graphics graphics, PointF[] polygon);

        public abstract void RenderPad(Graphics graphics, RectangleF bounds, GesturePadState state, int index);
    }
}