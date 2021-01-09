using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        // Optimization code instead of using GetUserByUsernameAsync
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            // if we are not using AutoMapper then we have to do such this bellow
            //return await _context.Users.Where(x => x.UserName == username).Select(user => new MemberDto
            //{
            //    Id = user.Id,
            //    Username = user.UserName
            //}).SingleOrDefaultAsync();

            // adding automapper to do the mapping for us
            return await _context.Users.Where(x => x.UserName == username)
                // it gets the configuration we provided in the AutoMapperProfiles.cs
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
        // Optimized code instead of using GetUsers
        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.Include(p => p.Photos).SingleOrDefaultAsync(x => x.UserName == username);
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            // greate than zero changes have been saved then return true otherwise return false
            return await _context.SaveChangesAsync() > 0;
        }
        // update our user
        public void Update(AppUser user)
        {
            // in here we are not actually changing anything in the database
            // but what we are going to do is mark this entity as it has been modified
            // this lets entityframe work updates and add a flag to and says yes it has been modified
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}
