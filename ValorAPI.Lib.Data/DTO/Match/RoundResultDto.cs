using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace ValorAPI.Lib.Data.DTO.Match
{
    public class RoundResultDto
    {
        public int roundNum;

        public string roundResult;

        public string roundCeremony;

        public string winningTeam;

        /// <summary>
        /// PUUID of player 
        /// </summary>
        public string bombPlanter;

        /// <summary>
        /// PUUID of player 
        /// </summary>
        public string bombDefuser;

        public int plantRoundTime;

        public PlayerLocationsDto[] plantPlayerLocations;

        public LocationDto plantLocation;

        public string plantSite;

        public int defuseRoundTime;

        public PlayerLocationsDto[] defusePlayerLocations;

        public LocationDto defuseLocation;

        public PlayerRoundStatsDto[] playerStats;

        public string roundResultCode;
    }
}
