using AuthSystem.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace AuthSystem.Pages
{
    public class Forum : PageModel
    {
        [BindProperty]
        public string content { set; get; }
        public void OnGet()
        {
            
        }

        public void OnLoad()
        {

        }

        // string content, int postId, string userId
        
        public void OnPost(int postId)
        {
            var userid = DbHandler.GetUserIdFromName(@User.Identity.Name).Result;
            DbHandler.WriteCommentToDb(postId, content, userid);
            
        }
    }
}