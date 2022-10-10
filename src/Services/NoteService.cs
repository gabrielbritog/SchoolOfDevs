using Microsoft.EntityFrameworkCore;
using SchoolOfDevs.Entities;
using SchoolOfDevs.Helpers;

namespace SchoolOfDevs.Services
{
    public interface INoteService
    {
        public Task<Note> Create(Note note);
        public Task<Note> GetById(int Id);
        public Task<List<Note>> GetAll();
        public Task Update(Note noteIn, int id);
        public Task Delete(int id);
    }
    public class NoteService : INoteService
    {
        private readonly DataContext _context;
        public NoteService(DataContext context)
        {
            _context = context;
        }

        public async Task<Note> Create(Note note)
        {         
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
            return note;
        }

        public async Task Delete(int id)
        {
            Note noteDb = await _context.Notes
                .FirstOrDefaultAsync(x => x.Id == id);
            if (noteDb is null)
            {
                throw new Exception("Note not found");
            }
            _context.Notes.Remove(noteDb);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Note>> GetAll() => await _context.Notes.ToListAsync();

        public async Task<Note> GetById(int id)
        {
            Note noteDb = await _context.Notes
                .FirstOrDefaultAsync(x => x.Id == id);
            if(noteDb is null)
            {
                throw new Exception("Note not found");
            }
            return noteDb;       
        }

        public async Task Update(Note noteIn, int id)
        {
            if(noteIn.Id != id)
            {
                throw new Exception("Route id differs User id");
            }

            Note noteDb = await _context.Notes
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            if(noteDb is null)
            {
                throw new Exception("Note not found");
            }
            noteIn.CreatedAt = noteDb.CreatedAt;

            _context.Entry(noteIn).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
