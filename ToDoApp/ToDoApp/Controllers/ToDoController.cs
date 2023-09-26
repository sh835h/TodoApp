using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApp.Models;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly TodoItemRepository _repository;

    public TodoController(TodoItemRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<TodoItem>> GetAll()
    {
        var items = _repository.GetAll();
        return Ok(items);
    }

    [HttpGet("{id}")]
    public ActionResult<TodoItem> GetById(int id)
    {
        var item = _repository.GetById(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost]
    public ActionResult<TodoItem> Create(TodoItem item)
    {
        if (item == null)
        {
            return BadRequest();
        }

        _repository.Add(item);

        return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, TodoItem item)
    {
        if (id != item.Id)
        {
            return BadRequest();
        }

        try
        {
            _repository.Update(item);
        }
        catch (Exception)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var existingItem = _repository.GetById(id);
        if (existingItem == null)
        {
            return NotFound();
        }

        _repository.Delete(id);
        return NoContent();
    }
}
