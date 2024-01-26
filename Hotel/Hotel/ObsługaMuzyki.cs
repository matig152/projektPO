using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;


namespace Hotel
{
    public static class ObsługaMuzyki
    {
        public static void GrajMuzykeWTle(string filePath)
        {
            // "Paul Gonsalves - Low Gravy.wav"
            SoundPlayer musicPlayer = new SoundPlayer();
            musicPlayer.SoundLocation = filePath;
            musicPlayer.PlayLooping();
        }

    }
}
