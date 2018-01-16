using System;
using System.Drawing;
using System.Drawing.Drawing2D;

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
    public class CyberLockScreenRenderer : LockScreenRenderer, IDisposable
    {
        private readonly Pen padPen, gesturePen;
        private readonly SolidBrush activeBrush = new SolidBrush(Color.Empty);

        public CyberLockScreenRenderer()
        {
            padPen = new Pen(Color.Empty, 5);
            gesturePen = new Pen(Color.Empty, 10);
            gesturePen.StartCap = gesturePen.EndCap = LineCap.Round;
            gesturePen.LineJoin = LineJoin.Round;
            ApplyThemeToUtencils();
        }

        private void ApplyThemeToUtencils()
        {
            Color c = Color.Black;
            gesturePen.Color = Color.FromArgb(200, 0, 0, 0);
            padPen.Color = activeBrush.Color = c;
        }

        public override void RenderBackground(Graphics graphics, RectangleF bounds, Color backColor)
        {
            graphics.Clear(Color.FromArgb(255, 255, 255));
        }

        public override void RenderGesture(Graphics graphics, PointF[] polygon)
        {
            graphics.DrawLines(gesturePen, polygon);
        }

        public override void RenderPad(Graphics graphics, RectangleF bounds, GesturePadState state, int index)
        {
            graphics.SmoothingMode = SmoothingMode.AntiAlias;
            graphics.DrawEllipse(padPen, bounds);
            if (state.HasFlag(GesturePadState.Inputted))
            {
                float inflationAmount = bounds.Width*0.3f;
                var innerRect = bounds;
                innerRect.Inflate(-inflationAmount, -inflationAmount);
                graphics.FillEllipse(activeBrush, innerRect);
            }
        }

        public void Dispose()
        {
            padPen.Dispose();
            gesturePen.Dispose();
            activeBrush.Dispose();
        }
    }
}