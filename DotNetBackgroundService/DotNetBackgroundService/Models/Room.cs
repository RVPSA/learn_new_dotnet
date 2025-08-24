namespace DotNetBackgroundService.Models
{
    public class Room
    {
        public int NumberOfOccupents { get; set; } = 0;
        public List<string> Furnitures { get; set; } = new List<string>();
        public bool HasLight { get; set; } = true;
        public bool IsLightOn { get; set; } = false;

        //Checking Light On/ Off
        public bool IsDark() {
            return IsLightOn ? false : true;
        }

        public List<string> GetAllFurnitures() {
            return Furnitures;
        }

        public List<string> AddFurniture(string furnitureName) {
            Furnitures.Add(furnitureName);
            return Furnitures;
        }
    }
}
