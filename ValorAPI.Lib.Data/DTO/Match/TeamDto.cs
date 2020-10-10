using System;
using System.Collections.Generic;
using System.Text;

namespace ValorAPI.Lib.Data.DTO.Match
{
    public class TeamDto
    {
        /// <summary>
        /// This is an arbitrary string. Red and Blue in bomb modes. The puuid of the player in deathmatch. 
        /// </summary>
        public string TeamId;

        public bool Won;

        public int RoundsPlayed;

        public int RoundsWon;

        /// <summary>
        /// Team points scored. Number of kills in deathmatch. 
        /// </summary>
        public int NumPoints;
    }
}
