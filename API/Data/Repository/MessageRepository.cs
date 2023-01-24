using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class MessageRepository : IMessageRepository
    {

        private readonly IMapper _mapper;

        private readonly DataContext _context;
        public MessageRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;


        }
        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
        }

        public void DeleteMessage(Message message)
        {
            _context.Messages.Remove(message);
        }

        public async Task<Message> GetMessage(int messageId)
        {
            return await _context.Messages.FindAsync(messageId);
        }

        public async Task<PagedList<MessageDto>> GetMessagesForUser(MessageParams messageParams)
        {

            var messages = _context.Messages.OrderByDescending(m => m.MessageSent).AsQueryable();


            messages = messageParams.Container switch
            {
                "Inbox" => messages.Where(m => m.RecipientUsername == messageParams.Username),
                "Outbox" => messages.Where(m => m.SenderUsername == messageParams.Username),
                _ => messages.Where(m => m.RecipientUsername == messageParams.Username && m.DateRead == null),

            };

            return await PagedList<MessageDto>.CreateAsync(messages.ProjectTo<MessageDto>(_mapper.ConfigurationProvider),
                 messageParams.PageNumber,
                 messageParams.PageSize);

        }

        public async Task<IEnumerable<MessageDto>> GetMessageThreads(int currentUserId, int recipientId)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}