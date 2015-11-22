using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using CRUDAjaxTable.Models;

namespace CRUDAjaxTable.Data
{
    public class OperationRepository : IRepository<Operation>
    {
        private bool _disposed;
        private readonly OperationDbContext _dbContext;

        public OperationRepository()
        {
            _dbContext = new OperationDbContext();
        }
        public async Task<IEnumerable<Operation>> GetAllAsync()
        {
            return await _dbContext.Operations.ToArrayAsync();
        }

        public async Task<Operation> GetAsync(int id)
        {
            var item = await _dbContext.Operations.FirstOrDefaultAsync(r => r.Id == id);

            var valuesAsArray = Enum.GetNames(typeof(TypeOperation));
            item.AllOperations = valuesAsArray;
            return item;

        }

        public async Task<Operation> AddAsync(Operation operation)
        {
            Author existingAuthor =
                await _dbContext.Authors.FirstOrDefaultAsync(r =>
                r.Name.ToUpper() == operation.Author.Name.ToUpper());
            if (existingAuthor != null)
            {
                operation.Author = existingAuthor;
                _dbContext.Operations.Add(operation);
                await _dbContext.SaveChangesAsync();
                return operation;
            }
            _dbContext.Operations.Add(operation);
            await _dbContext.SaveChangesAsync();
            return operation;

        }

        public async Task<Operation> UpdateAsync(Operation item)
        {
            var updateValue = await _dbContext.Operations.FirstOrDefaultAsync(r => r.Id == item.Id);
            if (updateValue != null)
            {
                updateValue.Description = item.Description;
                updateValue.Author = item.Author;
                updateValue.Cost = item.Cost;
                updateValue.Description = item.Description;
                updateValue.TypeOperation = item.TypeOperation;

                _dbContext.Entry(updateValue).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return updateValue;
            }
            return null;
        }

        public async Task<object> Delete(int id)
        {
            var deletedValue = await _dbContext.Operations.FirstOrDefaultAsync(r => r.Id == id);
            if (deletedValue != null)
            {
                _dbContext.Operations.Remove(deletedValue);
                return await _dbContext.SaveChangesAsync();
            }
            return null;
        }
        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                    _dbContext.Dispose();
            }
            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}