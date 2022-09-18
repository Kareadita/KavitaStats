using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KavitaStats.Entities;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Data.Repository;

public interface IColorRepository
{
    void Attach(Color color);
    void Delete(Color color);
    void Delete(IEnumerable<Color> colors);
    Task<Color> Find(string colorName);
    Task<IEnumerable<Color>> FindAll();
}

public class ColorRepository : IColorRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public ColorRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Attach(Color color)
    {
        _context.Color.Attach(color);
    }

    public void Delete(Color color)
    {
        _context.Color.Remove(color);
    }

    public void Delete(IEnumerable<Color> colors)
    {
        _context.Color.RemoveRange(colors);
    }

    public Task<Color> Find(string colorName)
    {
        return _context.Color.SingleOrDefaultAsync(c => c.Equals(colorName));
    }

    public async Task<IEnumerable<Color>> FindAll()
    {
        return await _context.Color.ToListAsync();
    }
}