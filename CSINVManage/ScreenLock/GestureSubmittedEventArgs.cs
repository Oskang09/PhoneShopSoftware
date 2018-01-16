using System;
using System.Linq;

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
    public class PassCodeSubmittedEventArgs : EventArgs
    {
        public bool Valid { get; private set; }

        public int[] Code { get; private set; }

        public PassCodeSubmittedEventArgs(bool valid, int[] code)
        {
            Valid = valid;
            Code = code;
        }

        public override string ToString()
        {
            string strCode = Code.Aggregate("{", (current, num) => current + (num + ", "));
            strCode = strCode.TrimEnd(',', ' ') + "}";
            return string.Format("Valid: {0}, Code: {1}", Valid, strCode);
        }
    }
}