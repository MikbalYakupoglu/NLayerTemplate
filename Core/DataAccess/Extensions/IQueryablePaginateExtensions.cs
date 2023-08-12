using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Core.DataAccess.Extensions
{
    public static class IQueryablePaginateExtensions
    {
        public static async Task<List<T>> ToPaginateAsync<T>(this IQueryable<T> source, int page, int size)
        {
            List<T> items = await source.Skip(page * size).Take(size).ToListAsync();
            return items;
        }
    }
}
