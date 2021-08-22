namespace GameSpace.Common
{
    public class GlobalConstants
    {
        public const int IdMaxLength = 40;
        public const int Unit = 1;
        public const int NonNegative = 0;

        public class User
        {
            public const int NicknameMaxLength = 30;
            public const int NicknameMinLength = 2;

            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;
        }

        public class Tournament
        {
            public const int NameMaxLength = 20;
            public const int NameMinLength = 6;

            public const int TeamsMaxCount = 8;
            public const int TeamsMinCount = 4;

            public const int CheckInPeriodMaxMinutes = 15;
            public const int CheckInPeriodMinMinutes = 5;

            public const int GoToGamePeriodMaxMinutes = 15;
            public const int GoToGamePeriodMinMinutes = 5;

            public const int MaxDifferenceDaysInSchedule = 40;
            public const int MinDifferenceDaysInSchedule = 2;
        }

        public class ProfileInfo
        {
            public const int BiographyMaxLength = 120;
        }

        public class Contact
        {
            public const int MaxEmailLength = 200;
            public const int MaxPhoneNumber = 15;
        }

        public class GamingAccount
        {
            public const int SummonerNameMaxLength = 16;
            public const int SummonerNameMinLength = 3;
            public const int AccountIdMaxLength = 56;
            public const int RankLength = 16;
        }

        public class Team
        {
            public const int MaxPlayersCount = 8;
            public const int TeamNameMaxLength = 20;
            public const int TeamNameMinLength = 2;

            public const int MaxDescriptionLength = 100;

            public const int MaxCityLength = 35;
            public const int MaxZipCodeLength = 12;
            public const int MaxCountryLength = 70;

            public const string DefaultTeamImageFileName = "12422580181795848664NYCS-bull-trans-T.svg.med";
            public const string PngExtantion = ".png";

            public const int MaxTeamSize = 6;
            public const int MaxMessageLength = 100;
        }

        public class Language
        {
            public const int LanguageMaxLength = 15;
            public const int LanguageMinLength = 2;
        }
    }
}