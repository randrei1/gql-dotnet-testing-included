using Microsoft.EntityFrameworkCore;
using MyFancyApi.Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyFancyApi.Service.Services
{
    public interface IBookService
    {
        Task<List<Book>> GetBooks();
        Task<Book> GetBook(int id);
    }
    public class BookService : IBookService
    {
        private readonly LibContext _libContext;

        public BookService(LibContext libContext)
        {
            _libContext = libContext;
        }
        public async Task<Book> GetBook(int id)
        {
            return await _libContext.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetBooks()
        {
            return await _libContext.Books.ToListAsync();
        }
    }
}