namespace GameCenter.Models
{
    public class GamesActors
    {
        public int PersonId { get; set; }
        public int GameId { get; set; }      
        public Person Person { get; set; }
        public Game Game { get; set; }
        public string RoleInGame { get; set; } 
        public int Order { get; set; }
    }
}