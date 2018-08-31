namespace WebAPI.Models
{
    public class HumanPlayer : IPlayer
    {
        public string Id { get; }
        public bool IsHuman { get { return true; } }

        public HumanPlayer(string id)
        {
            this.Id = id;
        }
    }
}
