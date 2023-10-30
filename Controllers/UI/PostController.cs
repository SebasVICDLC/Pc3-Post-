using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImplementaPost.DTO;
using ImplementaPost.Integrations;
using System.Text;
using System.Text.Json;

namespace ImplementaPost.Controllers
{
    public class PostController : Controller
{
    private readonly JsonplaceholderAPIIntegration _jsonPlaceholder;

    public PostController(JsonplaceholderAPIIntegration jsonPlaceholder)
    {
        _jsonPlaceholder = jsonPlaceholder;
    }

    public async Task<IActionResult> Index()
    {
        var posts = await _jsonPlaceholder.GetAllPosts();
        return View(posts);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(PostDTO newPost)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var createdPost = await _jsonPlaceholder.CreatePost(newPost);

                if (createdPost != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo crear el post.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error en la creación del post: " + ex.Message);
            }
        }

        return View(newPost);
    }

    public async Task<IActionResult> Details(int id)
    {
        var post = await _jsonPlaceholder.GetPostDetails(id);

        if (post == null)
        {
            return NotFound();
        }

        return View(post);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var existingPost = await _jsonPlaceholder.GetPostDetails(id);

        if (existingPost == null)
        {
            return NotFound();
        }

        return View(existingPost);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PostDTO updatedPost)
    {
        if (id != updatedPost.id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var updated = await _jsonPlaceholder.UpdatePost(updatedPost);

                if (updated != null)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "No se pudo actualizar el post.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error en la actualización del post: " + ex.Message);
            }
        }

        return View(updatedPost);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var response = await _jsonPlaceholder.DeletePostAsync(id);

        if (response.IsSuccessStatusCode)
        {
            return RedirectToAction("Index");
        }
        else
        {
            var errorMessage = await response.Content.ReadAsStringAsync();
            ModelState.AddModelError(string.Empty, "Error al eliminar el post: " + errorMessage);
            return RedirectToAction("Details", new { id = id });
        }
    }
}
}