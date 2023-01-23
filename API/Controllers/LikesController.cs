using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class LikesController : BaseApiController
    {
        private readonly ILikesRepository _likesRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public LikesController(IUserRepository userRepository, ILikesRepository likesRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _likesRepository = likesRepository;

        }

        [HttpPost("{username}")]
        public async Task<ActionResult> AddLike(string username)
        {
            var sourceUserId = User.GetUserId();

            var targetUser = await _userRepository.GetUserByUsernameAsync(username);

            if (targetUser == null) return NotFound("Target user does not exist");

            var sourceUser = await _likesRepository.GetUserWithLikes(sourceUserId);

            if (sourceUser.UserName == username) return BadRequest("You cannot like yourself");

            var userLike = await _likesRepository.GetUserLike(sourceUserId, targetUser.Id);

            if (userLike != null)
            {
                return BadRequest($"You have already liked {username}");
            }

            userLike = new UserLike()
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUser.Id
            };

            sourceUser.LikedUsers.Add(userLike);

            if (!await _userRepository.SaveAllAsync()) return BadRequest($"Failed to like user {username}");

            return Ok();

        }

        [HttpGet]
        public async Task<ActionResult<LikeDto>> GetUserLikes([FromQuery] LikesParams likesParams)
        {

            likesParams.UserId = User.GetUserId();
            
            if (string.IsNullOrEmpty(likesParams.Predicate) ||
                !(likesParams.Predicate.Equals("liked") ||
                likesParams.Predicate.Equals("likedBy")))
            {
                return BadRequest("Invalid predicate");
            }


            var users = await _likesRepository.GetUserLikes(likesParams);

            Response.AddPaginationHeader(new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages));

            return Ok(users);
        }

    }
}