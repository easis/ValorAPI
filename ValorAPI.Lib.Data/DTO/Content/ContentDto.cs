namespace ValorAPI.Lib.Data.DTO.Content
{
    public class ContentDto : IResponse
    {
        public string version;

        public ContentItemDto[] characters;

        public ContentItemDto[] maps;

        public ContentItemDto[] chromas;

        public ContentItemDto[] skins;

        public ContentItemDto[] skinLevels;

        public ContentItemDto[] equips;

        public ContentItemDto[] gameModes;

        public ContentItemDto[] sprays;

        public ContentItemDto[] sprayLevels;

        public ContentItemDto[] charms;

        public ContentItemDto[] charmLevels;

        public ContentItemDto[] playerCards;

        public ContentItemDto[] playerTitles;
    }
}
