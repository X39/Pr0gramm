using Windows.UI.Xaml.Media;

namespace Pr0gramm.API.ProfileUtil
{
    public class Mark
    {
        public Mark(int i)
        {
            this.RankId = i;
        }

        public int RankId { get; private set; }
        public Brush Color
        {
            get
            {
                var color = new Windows.UI.Color();
                switch (this.RankId)
                {
                    case 0: //Neutral Fagg0t
                        color.A = 0xFF;
                        color.R = 0xFF;
                        color.G = 0xFF;
                        color.B = 0xFF;
                        return new SolidColorBrush(color);
                    case 1: //Newfag
                        color.A = 0xFF;
                        color.R = 0xe1;
                        color.G = 0x08;
                        color.B = 0xe9;
                        return new SolidColorBrush(color);
                    case 2: //Oldfag
                        color.A = 0xFF;
                        color.R = 0x5b;
                        color.G = 0xb9;
                        color.B = 0x1c;
                        return new SolidColorBrush(color);
                    case 3: //Admin
                        color.A = 0xFF;
                        color.R = 0xff;
                        color.G = 0x99;
                        color.B = 0x00;
                        return new SolidColorBrush(color);
                    case 4: //Banned
                        color.A = 0xFF;
                        color.R = 0x44;
                        color.G = 0x44;
                        color.B = 0x44;
                        return new SolidColorBrush(color);
                    case 5: //Moderator
                        color.A = 0xFF;
                        color.R = 0x00;
                        color.G = 0x8f;
                        color.B = 0xff;
                        return new SolidColorBrush(color);
                    case 6: //Fliesentisch
                        color.A = 0xFF;
                        color.R = 0x6c;
                        color.G = 0x43;
                        color.B = 0x2b;
                        return new SolidColorBrush(color);
                    case 7: //Lebende Legende
                    case 8: //Wichtel
                    case 9: //Edler Spender
                    default:
                        color.A = 0xFF;
                        color.R = 0x1c;
                        color.G = 0xb9;
                        color.B = 0x92;
                        return new SolidColorBrush(color);
                }
            }
        }
        public override string ToString()
        {
            switch (this.RankId)
            {
                case 0: //Neutral Fagg0t
                    return "SCHWUCHTEL";
                case 1: //Newfag
                    return "NEUSCHWUCHTEL";
                case 2: //Oldfag
                    return "ALTSCHWUCHTEL";
                case 3: //Admin
                    return "ADMIN";
                case 4: //Banned
                    return "BANNED";
                case 5: //Moderator
                    return "MODERATOR";
                case 6: //Fliesentisch
                    return "FLIESENTISCHBESITZER";
                case 7: //Lebende Legende
                    return "LEBENDE LEGENDE";
                case 8: //Wichtel
                    return "WICHTEL";
                case 9: //Edler Spender
                    return "EDLER SPENDER";
                default:
                    return "UNBEKANNT";
            }
        }
    }
}
