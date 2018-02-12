
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace SocialApp.API.Helpers
{
    public class PagedList<T>
    {
        public List<T> Items { get; private set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            if( pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than zero!");
            }
            if( pageNumber <= 0)
            {
                // Note that it is assumed that the first pageNumber starts at 1
                throw new ArgumentOutOfRangeException("Current page can't be negative!");
            }

            CurrentPage = pageNumber;
            TotalCount = count;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Items = new List<T>();
            Items.AddRange(items);
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source,
            int pageNumber, int pageSize)
        {
            // This is an async helper method for creating a new PagedList
            
            if( pageSize <= 0)
            {
                throw new ArgumentOutOfRangeException("Page size must be greater than zero!");
            }
            if( pageNumber <= 0)
            {
                // Note that it is assumed that the first pageNumber starts at 1
                throw new ArgumentOutOfRangeException("Current page can't be negative!");
            }

            var count = await source.CountAsync();
            var items = await source.Skip((pageNumber-1)*pageSize).Take(pageSize).ToListAsync();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
