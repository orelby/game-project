namespace WebAPI.Models
{
    public class GenericAIPlayer : IPlayer
    {
        public string Id { get; }
        public bool IsHuman { get { return false; } }

        public GenericAIPlayer(string id)
        {
            this.Id = id;
        }
    }
}
