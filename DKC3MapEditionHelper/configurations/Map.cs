namespace DKC3MapEditionHelper.configurations
{
    public class Map
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public string ChipName { get; set; }

        public string Key => $"map_{Number}";
    }
}