namespace ValorAPI.Lib.Data.Constant
{
    public class Region
    {
        public static Region ASIA_PACIFIC = new Region("AP", "Asia/Pacific");
        public static Region BRAZIL = new Region("BR", "Brazil");
        public static Region EUROPE = new Region("EU", "Europe");
        public static Region KOREA = new Region("KR", "Korea");
        public static Region LATIN_AMERICA = new Region("LATAM", "Latin America");
        public static Region NORTH_AMERICA = new Region("NA", "North America");

        public string Name { get; private set; }
        public string FullName { get; private set; }

        public Region(string name, string fullName)
        {
            this.Name = name;
            this.FullName = fullName;
        }

        public override string ToString() => this.Name;
    }
}
