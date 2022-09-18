using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KavitaStats.Entities;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Data.Repository;

public interface IMangaReaderLayoutModeRepository
{
    void Attach(MangaReaderLayoutMode mangaReaderLayoutMode);
    void Delete(MangaReaderLayoutMode mangaReaderLayoutMode);
    void Delete(IEnumerable<MangaReaderLayoutMode> mangaReaderLayoutMode);
    Task<MangaReaderLayoutMode> Find(string mangaReaderLayoutMode);
    Task<IEnumerable<MangaReaderLayoutMode>> FindAll();
}

public class MangaReaderLayoutModeRepository : IMangaReaderLayoutModeRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public MangaReaderLayoutModeRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Attach(MangaReaderLayoutMode mangaReaderLayoutMode)
    {
        _context.MangaReaderLayoutMode.Attach(mangaReaderLayoutMode);
    }

    public void Delete(MangaReaderLayoutMode mangaReaderLayoutMode)
    {
        _context.MangaReaderLayoutMode.Remove(mangaReaderLayoutMode);
    }

    public void Delete(IEnumerable<MangaReaderLayoutMode> mangaReaderLayoutMode)
    {
        _context.MangaReaderLayoutMode.RemoveRange(mangaReaderLayoutMode);
    }

    public Task<MangaReaderLayoutMode> Find(string mangaReaderLayoutMode)
    {
        return _context.MangaReaderLayoutMode.SingleOrDefaultAsync(c => c.Equals(mangaReaderLayoutMode));
    }

    public async Task<IEnumerable<MangaReaderLayoutMode>> FindAll()
    {
        return await _context.MangaReaderLayoutMode.ToListAsync();
    }
}