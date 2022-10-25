using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Dto.User;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Enums;
using SchoolOfDevs.Exceptions;
using SchoolOfDevs.Helpers;
using BC= BCrypt.Net.BCrypt;
namespace SchoolOfDevs.Services
{
    public interface IUserService
    {
        public Task<UserResponse> Create(UserRequest user);
        public Task<UserResponse> GetById(int Id);
        public Task<List<UserResponse>> GetAll();
        public Task Update(UserRequest userIn, int id);
        public Task Delete(int id);
    }
    public class UserService : IUserService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserResponse> Create(UserRequest userRequest)
        {
            if (!userRequest.Password.Equals(userRequest.ConfirmPassword))
            {
                throw new BadRequestException("Password does not match ConfirmPassword");
            }
            User userDb = await _context.Users
                .AsNoTracking() //Exibe uma excessão caso tentem trackear mais de 1 elemento no banco
                .SingleOrDefaultAsync(u=> u.UserName == userRequest.UserName);
            if(userDb is not null)
            {
                throw new BadRequestException($"UserName {userRequest.UserName} already exist.");
            }
            
            User user = _mapper.Map<User>(userRequest);
            if(user.TypeUser != TypeUser.Teacher && userRequest.CoursesStudingIds.Any())
            {

                user.CoursesStudind = new List<Course>(); // instanciando lista de cursos
                List<Course> courses = await _context.Courses //Recebendo os cursos que tenho o Id igual ao da Requisição
                    .Where(e => userRequest.CoursesStudingIds
                    .Contains(e.Id))
                    .ToListAsync();

                foreach (Course course in courses)
                {
                    user.CoursesStudind.Add(course);
                }
            }
            userRequest.Password = BC.HashPassword(userRequest.Password);

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return _mapper.Map<UserResponse>(user);
        }

        public async Task Delete(int id)
        {
            User userDb = await _context.Users
                .SingleOrDefaultAsync(u => u.Id == id);
            if(userDb is null)
            {
                throw new KeyNotFoundException($"User{id} not found");
            }
            _context.Users.Remove(userDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<UserResponse>> GetAll()
        {
            List<User> users = await _context.Users.ToListAsync();
            return users.Select(e => _mapper.Map<UserResponse>(e)).ToList();
        }

        public async Task<UserResponse> GetById(int id)
        {
            User userDb = await _context.Users
                .Include(e => e.CoursesStudind) //Student
                .Include(e => e.CoursesTeaching) //Teacher
                .SingleOrDefaultAsync(u => u.Id == id);
            if (userDb is null)
            {
                throw new KeyNotFoundException($"User{id} not found");
            }
            return _mapper.Map<UserResponse>(userDb);
        }

        public async Task Update(UserRequestUpdate userRequest, int id)
        {
            if(userRequest.Id != id)
            {
                throw new BadRequestException("Route id differs User id");
            }
            else if (!userRequest.Password.Equals(userRequest.ConfirmPassword))
            {
                throw new BadRequestException("Password does not match ConfirmPassword");
            }
            User userDb = await _context.Users
                .Include(e => e.CoursesStudind) // para não perder os dados
               .SingleOrDefaultAsync(u => u.Id == id);
            if (userDb is null)
            {
                throw new KeyNotFoundException($"User{id} not found");
            }else if(!BC.Verify(userRequest.CurrentPassword, userDb.Password)) //Verifica se a senha que está sendo enviada é igual a do banco(Usando a criptografia)
            {
                throw new BadRequestException("Incorrect Password");
            }


            userDb.Password = BC.HashPassword(userRequest.Password);
           
            _context.Entry(userDb).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
