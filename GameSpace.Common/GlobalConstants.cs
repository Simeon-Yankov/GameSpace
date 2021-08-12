namespace GameSpace.Common
{
    public class GlobalConstants
    {
        //Global
        public const int IdMaxLength = 40;
        //public const double MaxDecimalValue = (double)decimal.MaxValue;
        //public const double MaxDoublelValue = double.MaxValue;
        //public const int MaxIntValue = int.MaxValue;
        //public const int MaxRate = 100;
        public const int NonNegative = 0;

        public class User
        {
            public const int NicknameMaxLength = 30;
            public const int NicknameMinLength = 2;


            public const int PasswordMaxLength = 100;
            public const int PasswordMinLength = 6;
        }

        //ProfileInfo
        public const int BiographyMaxLength = 120;

        //PersonalInfo
        public const int MaxNameLength = 50;

        //Contact
        public const int MaxEmailLength = 200;
        public const int MaxPhoneNumber = 15;

        //GamingAccounts
        public const int SummonerNameMaxLength = 16;
        public const int SummonerNameMinLength = 3;
        public const int AccountIdMaxLength = 56;
        public const int RankLength = 16;

        //Team
        public const int MaxPlayersCount = 8;
        public const int MaxTeamName = 20;
        public const int MinTeamName = 2;

        //TeamInfo
        public const int MaxDescriptionLength = 100;


        public const int MaxCityLength = 35;
        public const int MaxZipCodeLength = 12;
        public const int MaxCountryLength = 70;

        public const string DefaultTeamImageFileName = "12422580181795848664NYCS-bull-trans-T.svg.med";
        public const string PngExtantion = ".png";

        public const int MaxTeamSize = 6;
        public const int MaxMessageLength = 100;

        public class Language
        {
            public const int LanguageMaxLength = 15;
            public const int LanguageMinLength = 2;
        }
    }
}