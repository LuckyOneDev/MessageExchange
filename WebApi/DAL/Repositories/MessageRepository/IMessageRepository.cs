using WebApi.DAL.Models;

namespace WebApi.DAL.Repositories.MessageRepository
{
    public interface IMessageRepository
    {
        public void AddMessage(ApiMessage message);

        public IEnumerable<ApiMessage> GetMessages(GetMessagesModel model);

        public void EnsureCreated();
    }
}
