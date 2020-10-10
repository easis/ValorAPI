namespace ValorAPI.Lib.Data.Constant
{
    public class Locale
    {
        // Referencia: https://developer.riotgames.com/docs/lol#data-dragon_data-assets
        // Regex:
        //  - Búsqueda:         ^(.+?)[\s]+(.+?)$
        //  - Sustitución:      public static readonly Locale \U\1\E = new Locale\("\1", "\2"\);

        public static readonly Locale CS_CZ = new Locale("cs_CZ", "Czech (Czech Republic)");
        public static readonly Locale EL_GR = new Locale("el_GR", "Greek (Greece)");
        public static readonly Locale PL_PL = new Locale("pl_PL", "Polish (Poland)");
        public static readonly Locale RO_RO = new Locale("ro_RO", "Romanian (Romania)");
        public static readonly Locale HU_HU = new Locale("hu_HU", "Hungarian (Hungary)");
        public static readonly Locale EN_GB = new Locale("en_GB", "English (United Kingdom)");
        public static readonly Locale DE_DE = new Locale("de_DE", "German (Germany)");
        public static readonly Locale ES_ES = new Locale("es_ES", "Spanish (Spain)");
        public static readonly Locale IT_IT = new Locale("it_IT", "Italian (Italy)");
        public static readonly Locale FR_FR = new Locale("fr_FR", "French (France)");
        public static readonly Locale JA_JP = new Locale("ja_JP", "Japanese (Japan)");
        public static readonly Locale KO_KR = new Locale("ko_KR", "Korean (Korea)");
        public static readonly Locale ES_MX = new Locale("es_MX", "Spanish (Mexico)");
        public static readonly Locale ES_AR = new Locale("es_AR", "Spanish (Argentina)");
        public static readonly Locale PT_BR = new Locale("pt_BR", "Portuguese (Brazil)");
        public static readonly Locale EN_US = new Locale("en_US", "English (United States)");
        public static readonly Locale EN_AU = new Locale("en_AU", "English (Australia)");
        public static readonly Locale RU_RU = new Locale("ru_RU", "Russian (Russia)");
        public static readonly Locale TR_TR = new Locale("tr_TR", "Turkish (Turkey)");
        public static readonly Locale MS_MY = new Locale("ms_MY", "Malay (Malaysia)");
        public static readonly Locale EN_PH = new Locale("en_PH", "English (Republic of the Philippines)");
        public static readonly Locale EN_SG = new Locale("en_SG", "English (Singapore)");
        public static readonly Locale TH_TH = new Locale("th_TH", "Thai (Thailand)");
        public static readonly Locale VN_VN = new Locale("vn_VN", "Vietnamese (Viet Nam)");
        public static readonly Locale ID_ID = new Locale("id_ID", "Indonesian (Indonesia)");
        public static readonly Locale ZH_MY = new Locale("zh_MY", "Chinese (Malaysia)");
        public static readonly Locale ZH_CN = new Locale("zh_CN", "Chinese (China)");
        public static readonly Locale ZH_TW = new Locale("zh_TW", "Chinese (Taiwan)");

        public string Name { get; private set; }
        public string FullName { get; private set; }

        public Locale(string name, string fullName)
        {
            this.Name = name;
            this.FullName = FullName;
        }

        public override string ToString() => this.Name;
    }
}
