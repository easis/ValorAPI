namespace ValorAPI.Lib.Data.DTO.Match
{
    public class PlayerRoundStatsDto
    {
        public string puuid;

        public KillDto[] kills;

        public DamageDto[] damage;

        public int score;

        public EconomyDto economy;

        public AbilityDto ability;
    }
}
