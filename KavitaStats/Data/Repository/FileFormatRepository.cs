using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using KavitaStats.Entities;
using KavitaStats.Entities.V2;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Data.Repository;

public interface IFileFormatRepository
{
    void Attach(FileFormat fileFormat);
    void Delete(FileFormat fileFormat);
    void Delete(IEnumerable<FileFormat> fileFormats);
    Task<FileFormat> Find(string extension);
    Task<IEnumerable<FileFormat>> FindAll();
}

public class FileFormatRepository : IFileFormatRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public FileFormatRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Attach(FileFormat fileFormat)
    {
        _context.FileFormat.Attach(fileFormat);
    }

    public void Delete(FileFormat fileFormat)
    {
        _context.FileFormat.Remove(fileFormat);
    }

    public void Delete(IEnumerable<FileFormat> fileFormats)
    {
        _context.FileFormat.RemoveRange(fileFormats);
    }

    public Task<FileFormat> Find(string extension)
    {
        return _context.FileFormat.SingleOrDefaultAsync(c => c.Extension.Equals(extension));
    }

    public async Task<IEnumerable<FileFormat>> FindAll()
    {
        return await _context.FileFormat.ToListAsync();
    }
}