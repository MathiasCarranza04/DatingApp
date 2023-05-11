using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper) //recibimos el context de la database
        {
            _mapper = mapper;
            _context = context;

        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
            .Where(x => x.UserName == username) //donde el user de la db sea igual al del param
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync(); //retorno user del mapper
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);  //retorno user de database usando el id recibido
        }

        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
            .Include(p => p.Photos)
            .SingleOrDefaultAsync(x => x.UserName == username); //busco en tabla Users con username recibido
        }

        public async Task<IEnumerable<AppUser>> GetUsersAsync() //le pido que tambie retorne la lista de Photos
        {
            return await _context.Users
            .Include(p => p.Photos)
            .ToListAsync();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0; //esto es booleando, si da 0 no se guardaron datos, da false, si es mayor da true
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified; //informamos al entity tracker que una entity fue updateada
        }
    }
}