using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Models;

public class TodoItemRepository
{
    private readonly TodoDBContext _context;

    public TodoItemRepository(TodoDBContext context)
    {
        _context = context;
    }

    public IEnumerable<TodoItem> GetAll()
    {
        return _context.TodoItems.ToList();
    }

    public TodoItem GetById(int id)
    {
        return _context.TodoItems.Find(id);
    }

    public void Add(TodoItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }
        if (_context.TodoItems.Any(i => i.Id == item.Id))
        {
            
            throw new InvalidOperationException("Item with the same Id already exists.");
        }
        _context.TodoItems.Add(item);
        _context.SaveChanges();
    }

    public void Update(TodoItem item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(item));
        }

        _context.Entry(item).State = EntityState.Modified;
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var existingItem = _context.TodoItems.Find(id);
        if (existingItem == null)
        {
            throw new ArgumentException("Item not found", nameof(id));
        }

        _context.TodoItems.Remove(existingItem);
        _context.SaveChanges();
    }
}
