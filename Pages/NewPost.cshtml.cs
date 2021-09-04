using System;
using System.Threading.Tasks;
using AuthSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AuthSystem.Pages
{
    public class NewPost : PageModel
    {
        [BindProperty] 
        public string title { set; get; }
        [BindProperty] 
        public string content { set; get; }
        [BindProperty] 
        public string subforum { set; get; }

        public void OnGet()
        {
            
        }

        public void OnPost()
        {
            // string posterid, string title, string content, DateTime postdate, string sub, int postid
            var userid = DbHandler.GetUserIdFromName(@User.Identity.Name).Result;
            var postId = DbHandler.GetNumberOfPosts().Result;
            DbHandler.WritePostToDb(userid, title, content, subforum, postId);
            // Subforum = "General" because I can't seem to get my multiple options to pass correctly.
        }
    }
}