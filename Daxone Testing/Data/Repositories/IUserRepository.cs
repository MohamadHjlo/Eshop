using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Daxone_Testing.Models;
using Daxone_Testing.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;


namespace Daxone_Testing.Data.Repositories
{
    public interface IUserRepository
    {
        UserMiniCartViewModel GetMiniCartfUser(string userId);
    }

    public class UserRepository : IUserRepository
    {
        private DaxoneTestingContext _context;

        public UserRepository(DaxoneTestingContext context)
        {
            _context = context;
        }

        public UserMiniCartViewModel GetMiniCartfUser(string userId)

        {
            var factor = _context.Factors
                .Where(f => userId == f.UserId && f.IsFinaly == false)
                .Include(f => f.FactorDetails)
                .ThenInclude(f => f.Product)
                .ToList();

            var miniOrders = factor.Select(f => f.FactorDetails.OrderByDescending(a=>a.DetailId).Take(3).ToList()).ToList();

            var cart = new UserMiniCartViewModel()
            {
                Factors = factor,
                FactorDitailsList = miniOrders,
            };


            return cart;
        }

    }

}
