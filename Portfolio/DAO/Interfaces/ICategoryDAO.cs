using Portfolio.Models;

namespace Portfolio.DAO.Interfaces
{
    public interface ICategoryDAO
    {
        Task<int> AddCategoryAsync(Category category);
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<bool> UpdateCategoryAsync(Category category);
        Task<bool> DeleteCategoryAsync(int categoryId);
        Task<IEnumerable<Category>> GetCategoriesWithStatsAsync();
    }

}
