namespace ValorAPI.Lib.Data.DTO.Match
{
    public class KillDto
    {
        public int timeSinceGameStartMillis;

        public int timeSinceRoundStartMillis;

        /// <summary>
        /// PUUID
        /// </summary>
        public string killer;

        /// <summary>
        /// PUUID
        /// </summary>
        public string victim;

        public LocationDto victimLocation;

        /// <summary>
        /// List of PUUIDs
        /// </summary>
        public string[] assistants;

        public PlayerLocationsDto[] playerLocations;

        public FinishingDamageDto finishingDamage;
    }
}
