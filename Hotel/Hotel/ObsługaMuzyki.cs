using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Media;


namespace Hotel
{
    /// <summary>
    /// Klasa statyczna odpowiedzialna za ustawienia muzyki.
    /// </summary>
    public static class ObsługaMuzyki
    {
        /// <summary>
        /// Metoda ma na celu odtwarzanie muzyki w tle po uruchomieniu aplikacji.
        /// </summary>
        /// <param name="filePath"> Ścieżka względna do pliku. </param>
        public static void GrajMuzykeWTle(string filePath)
        {
            // "Paul Gonsalves - Low Gravy.wav"
            SoundPlayer musicPlayer = new SoundPlayer();
            musicPlayer.SoundLocation = filePath;
            musicPlayer.PlayLooping();
        }

    }
}
