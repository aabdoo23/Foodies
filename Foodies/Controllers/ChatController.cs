using Microsoft.AspNetCore.Mvc;
using Foodies.Models;
using Microsoft.EntityFrameworkCore;

namespace Foodies.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly FoodiesDbContext _context;

        public ChatController(FoodiesDbContext context)
        {
            _context = context;
        }

        // Get all chats for the customer/admin
        [HttpGet("getChats/{userId}")]
        public async Task<ActionResult<IEnumerable<Chat>>> GetChats(string userId)
        {
            var chats = await _context.Chats
                .Include(c => c.Messages)
                .Where(c => c.AdminId == userId || c.CustomerId == userId)
                .ToListAsync();

            return Ok(chats);
        }

        // Send a message
        [HttpPost("sendMessage")]
        public async Task<ActionResult> SendMessage(int chatId, string senderId, string messageContent)
        {
            var chat = await _context.Chats.FindAsync(chatId);

            if (chat == null) return NotFound();

            var message = new Message
            {
                ChatId = chatId,
                SenderId = senderId,
                Content = messageContent,
                SentAt = DateTime.Now
            };

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
