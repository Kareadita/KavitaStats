using System.Threading.Tasks;
using AutoMapper;
using KavitaStats.Data.Repository;
using KavitaStats.Entities;
using Microsoft.AspNetCore.Identity;

namespace KavitaStats.Data
{
    public interface IUnitOfWork
    {
        IColorRepository ColorRepository { get; }
        IPageSplitRepository PageSplitRepository { get; }
        IMangaReaderLayoutModeRepository MangaReaderLayoutModeRepository { get; }
        IFileFormatRepository FileFormatRepository { get; }
        bool Commit();
        Task<bool> CommitAsync();
        bool HasChanges();
        bool Rollback();
        Task<bool> RollbackAsync();

    }
    public class UnitOfWork : IUnitOfWork
    {
        
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public UnitOfWork(DataContext context, IMapper mapper, UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }


        public IColorRepository ColorRepository => new ColorRepository(_context, _mapper);
        public IPageSplitRepository PageSplitRepository => new PageSplitRepository(_context, _mapper);
        public IMangaReaderLayoutModeRepository MangaReaderLayoutModeRepository => new MangaReaderLayoutModeRepository(_context, _mapper);
        public IFileFormatRepository FileFormatRepository => new FileFormatRepository(_context, _mapper);

        /// <summary>
        /// Commits changes to the DB. Completes the open transaction.
        /// </summary>
        /// <returns></returns>
        public bool Commit()
        {
            return _context.SaveChanges() > 0;
        }
        /// <summary>
        /// Commits changes to the DB. Completes the open transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> CommitAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Is the DB Context aware of Changes in loaded entities
        /// </summary>
        /// <returns></returns>
        public bool HasChanges()
        {
            return _context.ChangeTracker.HasChanges();
        }

        /// <summary>
        /// Rollback transaction
        /// </summary>
        /// <returns></returns>
        public async Task<bool> RollbackAsync()
        {
            await _context.DisposeAsync();
            return true;
        }
        /// <summary>
        /// Rollback transaction
        /// </summary>
        /// <returns></returns>
        public bool Rollback()
        {
            _context.Dispose();
            return true;
        }
    }
}
