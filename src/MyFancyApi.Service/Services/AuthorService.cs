using Microsoft.EntityFrameworkCore;
using MyFancyApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.Services
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAuthors();
        Task<Author> GetAuthor(int id);
    }
    public class AuthorService : IAuthorService
    {
        private readonly LibContext _libContext;

        public AuthorService(LibContext libContext)
        {
            _libContext = libContext;
        }

        public async Task<Author> GetAuthor(int id)
        {
            return await _libContext.Authors.FindAsync(id);
        }

        public async Task<List<Author>> GetAuthors()
        {
            return await _libContext.Authors.ToListAsync();
        }
    }
}
