using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data.Repository
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;


        public LikesRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;

        }


        public async Task<UserLike> GetUserLike(int SourceUserId, int TargetUserId)
        {
            return await _context.Likes.FindAsync(SourceUserId, TargetUserId);
        }

        public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
        {
            var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();
            var likes = _context.Likes.AsQueryable();


            if (likesParams.Predicate == "liked")
            {
                likes = likes.Where(like => like.SourceUserId == likesParams.UserId);
                users = likes.Select(like => like.TargetUser);
            }

            if (likesParams.Predicate == "likedBy")
            {
                likes = likes.Where(like => like.TargetUserId == likesParams.UserId);
                users = likes.Select(like => like.SourceUser);
            }

            var likedUsers = users.Select(user => new LikeDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                City = user.City,
                KnownAs = user.KnownAs,
                Age = user.DateOfBirth.CalculateAge(),
                PhotoUrl = user.Photos.FirstOrDefault(p => p.IsMain).Url
            });

            return await PagedList<LikeDto>.CreateAsync(likedUsers,
                 likesParams.PageNumber,
                 likesParams.PageSize);
        }

        public async Task<AppUser> GetUserWithLikedBy(int userId)
        {
            return await _context.Users.Include(u => u.LikedByUsers).FirstOrDefaultAsync(lu => lu.Id == userId);
        }


        public async Task<AppUser> GetUserWithLikes(int userId)
        {
            return await _context.Users.Include(u => u.LikedUsers).FirstOrDefaultAsync(lu => lu.Id == userId);
        }


    }
}