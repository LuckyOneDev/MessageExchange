namespace WebApi.DAL
{
    public interface IMessageRepository
    {
        public void AddMessage(ApiMessage message);

        public IEnumerable<ApiMessage> GetMessages(GetMessagesModel model);

        public void EnsureCreated();
    }
}
