using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Daxone_Testing.Models;
using Daxone_Testing.Models.ViewModels;


namespace Daxone_Testing.Data.Repositories
{
   public interface IGroupRepository//مخصوص استفاده دسته بندی در نو بار و نمایش تعداد ایتم ها تو اون کتگوری
   {

       IEnumerable<Category> getAllCategories();

       IEnumerable<ShowGroupViewModel> getGroupForShowCategory();


   }

   public class GroupRepository: IGroupRepository
   {
       private DaxoneTestingContext _context;

       public GroupRepository(DaxoneTestingContext context)
       {
           _context = context;
       }
       public IEnumerable<Category> getAllCategories()
       {
           return _context.Category;
       }

       public IEnumerable<ShowGroupViewModel> getGroupForShowCategory()
       {
           return _context.Category
               .Select(s => new ShowGroupViewModel()
               {
                   GroupId = s.Id,
                   GrpName = s.Name,
                   ProductCount = _context.CategoryToProducts.Count(g => g.CategoryId == s.Id)
               }).ToList(); 
       }
   }
}
