using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using KavitaStats.Entities;
using Microsoft.EntityFrameworkCore;

namespace KavitaStats.Data.Repository;

public interface IPageSplitRepository
{
    void Attach(PageSplit pageSplit);
    void Delete(PageSplit pageSplit);
    void Delete(IEnumerable<PageSplit> pageSplits);
    Task<PageSplit> Find(string pageSplit);
    Task<IEnumerable<PageSplit>> FindAll();
}

public class PageSplitRepository : IPageSplitRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public PageSplitRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public void Attach(PageSplit pageSplit)
    {
        _context.PageSplit.Attach(pageSplit);
    }

    public void Delete(PageSplit pageSplit)
    {
        _context.PageSplit.Remove(pageSplit);
    }

    public void Delete(IEnumerable<PageSplit> pageSplits)
    {
        _context.PageSplit.RemoveRange(pageSplits);
    }

    public Task<PageSplit> Find(string pageSplitName)
    {
        return _context.PageSplit.SingleOrDefaultAsync(c => c.Equals(pageSplitName));
    }

    public async Task<IEnumerable<PageSplit>> FindAll()
    {
        return await _context.PageSplit.ToListAsync();
    }
}